namespace JohnIsDev.Core.Mcp.Models.Requests;

/// <summary>
/// Represents a request within the MCP process with specific behaviors, providers,
/// and previous contents.
/// </summary>
public class RequestMcp
{
    /// <summary>
    /// Gets or sets the behavior type or action associated with the current response.
    /// This property is used to describe the nature or context of the response,
    /// detailing how it relates to the originating request or system interactions.
    /// </summary>
    public string Behavior { get; set; } = "";

    /// <summary>
    /// Gets or sets the query string associated with the request.
    /// This property defines the specific input or search parameters
    /// related to the MCP process and aids in determining the request's intent.
    /// </summary>
    public string Query { get; set; } = "";
    
    /// <summary>
    /// Gets or sets a collection of previous response objects related to the current instance.
    /// These represent historical data or prior states associated with the current response object.
    /// </summary>
    public List<string> PreviousContents { get; set; } = [];

    /// <summary>
    /// Gets or sets a value indicating whether the current request contains context
    /// derived from previous contents. This property is used to determine if
    /// historical or earlier data is relevant or included in the current processing context.
    /// </summary>
    public bool IsContainsPreviousContext { get; set; } = false;
}