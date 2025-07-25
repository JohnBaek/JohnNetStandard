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
/// </summary>
public class RabbitMqMessageBus : IMessageBus
{
    /// <summary>
    /// 
    /// </summary>
    private readonly ILogger<RabbitMqMessageBus> _logger;
    
    /// <summary>
    /// 
    /// </summary>
    private readonly IConnection _connection;
    
    /// <summary>
    /// 
    /// </summary>
    private readonly RabbitMqConfig _config;

    /// <summary>
    /// 
    /// </summary>
    private IChannel _channel;
    
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
    /// 
    /// </summary>
    /// <param name="topic"></param>
    /// <param name="routingKey"></param>
    /// <param name="exchangeType"></param>
    /// <param name="message"></param>
    /// <typeparam name="T"></typeparam>
    public async Task PublishAsync<T>(string topic, string routingKey, string exchangeType, T message)
    {
        try
        {
            IChannel channel = await _connection.CreateChannelAsync();
            
            // Declare an exchange 
            await channel.ExchangeDeclareAsync(topic, exchangeType, durable: true, autoDelete: false);

            // Serialize
            byte[] body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));

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
            await using IChannel channel = await _connection.CreateChannelAsync();
            // Declare an exchange 
            await channel.ExchangeDeclareAsync(topic, exchangeType, durable: true, autoDelete: false);
            string queueName = $"{topic}_{typeof(T).Name}";
            
            // Declare a Queue and bind
            await channel.QueueDeclareAsync(queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
            await channel.QueueBindAsync(queueName, topic, routingKey);

            // Prepare a consumer
            AsyncEventingBasicConsumer consumer = new AsyncEventingBasicConsumer(channel);
            consumer.ReceivedAsync += async (model, eventArgs) =>
            {
                try
                {
                    // Gets a body 
                    byte[] body = eventArgs.Body.ToArray();
                    string jsonRaw = Encoding.UTF8.GetString(body);
                    T message = JsonConvert.DeserializeObject<T>(jsonRaw) ;                    
                    // Get a result 
                    bool isSuccess = await messageHandler(message);
                    
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
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
        }
    }
    
    public void Dispose()
    {
        _connection.Dispose();
    }
}