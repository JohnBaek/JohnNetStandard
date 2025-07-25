namespace JohnIsDev.Core.MessageQue.Models.Enums;

/// <summary>
/// 
/// </summary>
public enum EnumMessageBusType
{
    /// <summary>
    /// 
    /// </summary>
    RabbitMQ,
    
    /// <summary>
    /// 
    /// </summary>
    Kafka,
    
    /// <summary>
    /// 
    /// </summary>
    AzureServiceBus,
    
    /// <summary>
    /// 
    /// </summary>
    Redis,
    
    /// <summary>
    /// 
    /// </summary>
    InMemory
}