namespace JohnIsDev.Core.MessageQue.Models.Options;

/// <summary>
/// 
/// </summary>
public class MessageOptions
{
    /// <summary>
    /// 
    /// </summary>
    public bool Persistent { get; set; } = true;
    
    /// <summary>
    /// 
    /// </summary>
    public TimeSpan? TTL { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    public Dictionary<string, object> Headers { get; set; } = new();
    
    /// <summary>
    /// 
    /// </summary>
    public int Priority { get; set; } = 0;
}