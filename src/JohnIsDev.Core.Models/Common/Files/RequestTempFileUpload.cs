namespace JohnIsDev.Core.Models.Common.Files;

/// <summary>
/// FileUpload
/// </summary>
public class RequestTempFileUpload
{
    /// <summary>
    /// Name 
    /// </summary>
    public string FileName { get; set; } = "";

    /// <summary>
    /// Base64 String
    /// </summary>
    public string Base64String { get; set; } = "";
}