using JohnIsDev.Core.MessageQue.Models.Enums;

namespace JohnIsDev.Core.MessageQue.Models.Configs;

/// <summary>
/// 
/// </summary>
public class MessageBusConfig
{
    /// <summary>
    /// 
    /// </summary>
    public string ConnectionString { get; set; } = "";

    /// <summary>
    /// 
    /// </summary>
    public EnumMessageBusType Type { get; set; }
}