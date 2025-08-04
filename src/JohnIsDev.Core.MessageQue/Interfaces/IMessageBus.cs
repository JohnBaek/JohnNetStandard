using JohnIsDev.Core.MessageQue.Models;
using RabbitMQ.Client;

namespace JohnIsDev.Core.MessageQue.Interfaces;

/// <summary>
/// Represents a message bus interface for asynchronous message publishing
/// </summary>
public interface IMessageBus : IDisposable
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="topic"></param>
    /// <param name="routingKey"></param>
    /// <param name="exchangeType"></param>
    /// <param name="message"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    Task PublishAsync<T>(string topic, string routingKey, string exchangeType, T message);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="queue"></param>
    /// <param name="routingKey"></param>
    /// <param name="exchangeType"></param>
    /// <param name="messageHandler"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    Task SubscribeAsync<T>(string queue, string routingKey,string exchangeType, Func<T,string, Task<bool>> messageHandler);


    // /// <summary>
    // /// Publishes a message to the specified topic and waits for a response asynchronously.
    // /// </summary>
    // /// <typeparam name="TResponse">The type of the expected response.</typeparam>
    // /// <param name="topic">The topic to which the message will be published.</param>
    // /// <param name="routingKey">The routing key used to route the message.</param>
    // /// <param name="exchangeType">The exchange type to be used for message publication.</param>
    // /// <param name="request">The request message to be sent. This parameter is optional.</param>
    // /// <param name="timeout">The time to wait for a response before timing out. Defaults to the value of <see cref="TimeSpan.Zero"/> if not specified.</param>
    // /// <returns>A task that represents the asynchronous operation. The task result contains the response of type <typeparamref name="TResponse"/>.</returns>
    // Task<TResponse> PublishAndWaitForResponseAsync<TResponse>(
    //     string topic,
    //     string routingKey,
    //     string exchangeType,
    //     object? request = null,
    //     TimeSpan timeout = default);


    /// <summary>
    /// Publishes a message for Remote Procedure Call (RPC) and waits for a response asynchronously.
    /// </summary>
    /// <typeparam name="TRequest">The type of the request message to publish.</typeparam>
    /// <typeparam name="TResponse">The type of the expected response message.</typeparam>
    /// <param name="topic">The topic to which the message will be published.</param>
    /// <param name="routingKey">The routing key used for routing the message.</param>
    /// <param name="exchangeType">The exchange type to be used for message publication.</param>
    /// <param name="message">The request message to be sent.</param>
    /// <param name="timeoutSec">The timeout in seconds to wait for a response before completing the operation. Default is 10 seconds.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the response message of type <typeparamref name="TResponse"/> or null if no response is received within the specified timeout.</returns>
    Task<TResponse?> PublishRpcAsync<TRequest, TResponse>(
        string topic,
        string routingKey,
        string exchangeType,
        TRequest message,
        int timeoutSec = 10);


    /// <summary>
    /// Subscribes to a queue for Remote Procedure Call (RPC) messages and processes them asynchronously.
    /// </summary>
    /// <typeparam name="TRequest">The type of the request message expected.</typeparam>
    /// <typeparam name="TResponse">The type of the response message to return.</typeparam>
    /// <param name="queue">The name of the queue to subscribe to.</param>
    /// <param name="routingKey">The routing key used for message binding in the exchange.</param>
    /// <param name="exchangeType">The type of the exchange used for message routing.</param>
    /// <param name="messageHandler">
    /// A delegate that represents a function to handle the incoming messages.
    /// It receives the request message of type <typeparamref name="TRequest"/> and its routing key,
    /// and returns a task containing the response message of type <typeparamref name="TResponse"/>.
    /// </param>
    /// <returns>A task that represents the asynchronous operation of subscribing and handling messages.</returns>
    Task SubscribeRpcAsync<TRequest, TResponse>(
        string queue,
        string routingKey,
        string exchangeType,
        Func<TRequest, string, Task<TResponse>> messageHandler);
}