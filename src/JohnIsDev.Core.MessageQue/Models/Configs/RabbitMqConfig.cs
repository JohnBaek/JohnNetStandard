namespace JohnIsDev.Core.MessageQue.Models.Configs;

/// <summary>
/// 
/// </summary>
public class RabbitMqConfig : MessageBusConfig
{
    /// <summary>
    /// 
    /// </summary>
    public string HostName { get; set; } = "localhost";
    
    /// <summary>
    /// 
    /// </summary>
    public int Port { get; set; } = 5672;
    
    /// <summary>
    /// 
    /// </summary>
    public string UserName { get; set; } = "guest";
    
    /// <summary>
    /// 
    /// </summary>
    public string Password { get; set; } = "guest";
    
    /// <summary>
    /// 
    /// </summary>
    public string VirtualHost { get; set; } = "/";
    
    /// <summary>
    /// 
    /// </summary>
    public string ExchangeType { get; set; } = "direct";
}