using System.Text;
using System.Text.Json.Serialization;
using JohnIsDev.Core.MessageQue.Interfaces;
using JohnIsDev.Core.MessageQue.Models;
using JohnIsDev.Core.MessageQue.Models.Configs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace JohnIsDev.Core.MessageQue.Implements;

/// <summary>
/// The RabbitMqMessageBus class provides functionality for managing and interacting with a message queue
/// using RabbitMQ. It serves as a message bus implementation to enable communication between different
/// parts of the application or distributed system.
/// Should be singleton
/// </summary>
public class RabbitMqMessageBus : IMessageBus
{
    /// <summary>
    /// Represents the logging mechanism used to capture, record, and output diagnostic or operational
    /// information for the <see cref="RabbitMqMessageBus"/>. This variable is of type
    /// <see cref="ILogger{RabbitMqMessageBus}"/> and is utilized to log messages such as errors,
    /// warnings, or informational details during the operation of the message bus,
    /// including publishing and subscribing to messages.
    /// </summary>
    private readonly ILogger<RabbitMqMessageBus> _logger;

    /// <summary>
    /// Represents the connection interface to the RabbitMQ server. This variable is of type
    /// <see cref="IConnection"/> and is used by the <see cref="RabbitMqMessageBus"/> to establish
    /// and manage communication with the RabbitMQ message broker, including creating channels,
    /// publishing messages, and subscribing to queues. The connection is intended to persist
    /// throughout the lifecycle of the <see cref="RabbitMqMessageBus"/> to avoid the overhead
    /// of repeatedly opening and closing connections.
    /// </summary>
    private readonly IConnection _connection;

    /// <summary>
    /// Represents the configuration settings required to establish a connection and interact
    /// with the RabbitMQ message broker. This includes configuration options such as the host name,
    /// port, user credentials, virtual host, and exchange type. This variable is of type
    /// <see cref="RabbitMqConfig"/> and is used internally by the <see cref="RabbitMqMessageBus"/>
    /// to configure its connection to RabbitMQ.
    /// </summary>
    private readonly RabbitMqConfig _config;

    /// <summary>
    /// Maintains a collection of subscribed channels represented by <see cref="IChannel"/> instances
    /// that are utilized for managing and processing message subscriptions within the RabbitMQ message
    /// bus system. This list is used to track active subscription channels and ensure proper handling
    /// of incoming messages for various topics and routing keys.
    /// </summary>
    private readonly List<IChannel> _subscribeChannels = [];

    /// <summary>
    /// Represents a collection of asynchronous event-based consumers, specifically of type
    /// <see cref="AsyncEventingBasicConsumer"/>. These consumers are responsible for receiving
    /// and processing messages from RabbitMQ. The collection is utilized to manage and coordinate
    /// multiple consumers within the scope of the message bus, enabling subscriptions to various
    /// message queues and topics.
    /// </summary>
    private readonly List<AsyncEventingBasicConsumer> _consumers = [];
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="connection"></param>
    /// <param name="config"></param>
    public RabbitMqMessageBus(ILogger<RabbitMqMessageBus> logger, IConnection connection, RabbitMqConfig config)
    {
        _logger = logger;
        _connection = connection;
        _config = config;
    }


    /// <summary>
    /// Publishes a message to a specified topic and routing key in the RabbitMQ exchange. This method allows
    /// sending messages to RabbitMQ with a defined exchange type and routing key.
    /// </summary>
    /// <typeparam name="T">The type of the message to be published.</typeparam>
    /// <param name="topic">The topic or exchange name where the message will be published.</param>
    /// <param name="routingKey">The routing key that determines how the message will be routed.</param>
    /// <param name="exchangeType">The type of the RabbitMQ exchange (e.g., direct, fanout, topic).</param>
    /// <param name="message">The message payload to be published.</param>
    /// <returns>A Task that represents the asynchronous operation for message publishing.</returns>
    public async Task PublishAsync<T>(string topic, string routingKey, string exchangeType, T message)
    {
        try
        {
            // Create Chanel
            await using IChannel channel = await _connection.CreateChannelAsync();

            // Declare an exchange 
            await channel.ExchangeDeclareAsync(topic, exchangeType, durable: true, autoDelete: false);

            // Serialize
            byte[] body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));
            
            _logger.LogInformation($"Publish to {topic} with routingKey {routingKey} with body {body}");

            // Publish to MQ
            await channel.BasicPublishAsync(
                exchange: topic,
                routingKey: routingKey,
                body: body
            );
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
        }
    }

    /// <summary>
    /// Subscribes to a specified topic and routing key in the message queue and processes messages using
    /// the provided message handler function. The subscription requires specifying the topic, routing key,
    /// exchange type, and the handler function for processing incoming messages.
    /// </summary>
    /// <typeparam name="T">The type of the message to be handled.</typeparam>
    /// <param name="topic">The topic or queue name to subscribe to.</param>
    /// <param name="routingKey">The routing key associated with the subscription.</param>
    /// <param name="exchangeType">The type of the exchange (e.g., direct, fanout, topic).</param>
    /// <param name="messageHandler">A function that handles the incoming message and returns a Task<bool>
    /// indicating whether the message was successfully handled or not.</param>
    /// <returns>A Task representing the asynchronous operation for message subscription.</returns>
    public async Task SubscribeAsync<T>(string topic, string routingKey, string exchangeType,
        Func<T, Task<bool>> messageHandler) 
    {
        try
        {
            IChannel channel = await _connection.CreateChannelAsync();
            
            // To store in memory implicitly to avoid GC
            _subscribeChannels.Add(channel);
            
            // Declare an exchange 
            await channel.ExchangeDeclareAsync(topic, exchangeType, durable: true, autoDelete: false);
            string queueName = $"{topic}_{typeof(T).Name}";
            
            // Declare a Queue and bind
            await channel.QueueDeclareAsync(queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
            await channel.QueueBindAsync(queueName, topic, routingKey);

            // Declare consumer and add memory 
            AsyncEventingBasicConsumer consumer = new AsyncEventingBasicConsumer(channel);
            _consumers.Add(consumer);
            
            _logger.LogInformation($"Subscribe to {topic} with routingKey {routingKey} with queueName {queueName}");
            
            consumer.ReceivedAsync += async (model, eventArgs) =>
            {
                try
                {
                    // Gets a body 
                    byte[] body = eventArgs.Body.ToArray();
                    string jsonRaw = Encoding.UTF8.GetString(body);
                    T? message = JsonConvert.DeserializeObject<T>(jsonRaw) ;      
                    
                    // Get a result 
                    bool isSuccess = message != null && await messageHandler(message);
                    
                    if(isSuccess)
                        await channel.BasicAckAsync(eventArgs.DeliveryTag, false);
                    else
                        await channel.BasicNackAsync(eventArgs.DeliveryTag, false, true);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, e.Message);
                }
            };
            
            // Consume
            await channel.BasicConsumeAsync(queue: queueName , autoAck: false , consumer: consumer);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
        }
    }

    /// <summary>
    /// Disposes of the resources used by the RabbitMqMessageBus instance, including the underlying RabbitMQ connection.
    /// This method should be called to release unmanaged resources and ensure proper cleanup.
    /// </summary>
    public void Dispose()
    {
        // Dispose all subscriptions
        foreach (var channel in _subscribeChannels)
        {
            try
            {
                channel?.Dispose();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error disposing subscribe channel: {Message}", e.Message);
            }
        }
        
        _subscribeChannels.Clear();
        _consumers.Clear();
        _connection.Dispose();
    }
}