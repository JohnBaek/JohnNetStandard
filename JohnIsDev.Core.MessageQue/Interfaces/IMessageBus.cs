using JohnIsDev.Core.MessageQue.Models;

namespace JohnIsDev.Core.MessageQue.Interfaces;

/// <summary>
/// Represents a message bus interface for asynchronous message publishing
/// </summary>
public interface IMessageBus : IDisposable
{
    /// <summary>
    /// Publishes a message to the specified topic asynchronously
    /// </summary>
    /// <typeparam name="T">The type of message to publish</typeparam>
    /// <param name="topic">The topic to publish the message to</param>
    /// <param name="message">The message content to publish</param>
    /// <param name="options">Optional message configuration settings</param>
    /// <returns>A task representing the asynchronous publish operation</returns>
    Task PublishAsync<T>(string topic, T message, MessageOptions? options = null);

    /// <summary>
    /// Publishes a message to the specified topic with a routing key asynchronously.
    /// </summary>
    /// <typeparam name="T">The type of message to publish</typeparam>
    /// <param name="topic">The topic to publish the message to</param>
    /// <param name="routingKey">The routing key used for message delivery</param>
    /// <param name="message">The message content to publish</param>
    /// <param name="options">Optional message configuration settings</param>
    /// <returns>A task representing the asynchronous publish operation</returns>
    Task PublishAsync<T>(string topic, string routingKey, T message, MessageOptions? options = null);

    /// <summary>
    /// Subscribes to a specified topic asynchronously and processes messages using the provided message handler.
    /// </summary>
    /// <typeparam name="T">The type of messages to subscribe to</typeparam>
    /// <param name="topic">The topic to subscribe to</param>
    /// <param name="messageHandler">A function that processes incoming messages and returns a boolean indicating the success of the operation</param>
    /// <param name="options">Optional subscription configuration settings</param>
    /// <returns>A task representing the asynchronous subscription operation</returns>
    Task SubscribeAsync<T>(string topic, Func<T, Task<bool>> messageHandler, SubscriptionOptions? options = null);

    /// <summary>
    /// Subscribes to a specified topic asynchronously to handle incoming messages
    /// </summary>
    /// <typeparam name="T">The type of message to handle</typeparam>
    /// <param name="topic">The topic to subscribe to</param>
    /// <param name="routingKey">The routing key which can specific event</param>
    /// <param name="messageHandler">The function to process received messages, returning a task with a boolean indicating success or failure</param>
    /// <param name="options">Optional subscription configuration settings</param>
    /// <returns>A task that represents the asynchronous subscription operation</returns>
    Task SubscribeAsync<T>(string topic, string routingKey, Func<T, Task<bool>> messageHandler,
        SubscriptionOptions? options = null);
}