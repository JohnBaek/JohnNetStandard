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
    /// <param name="topic"></param>
    /// <param name="routingKey"></param>
    /// <param name="exchangeType"></param>
    /// <param name="messageHandler"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    Task SubscribeAsync<T>(string topic, string routingKey,string exchangeType, Func<T,string, Task<bool>> messageHandler);
}