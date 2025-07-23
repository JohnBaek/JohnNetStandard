namespace JohnIsDev.Core.Mcp.Models.Responses;

/// <summary>
/// Represents a response that includes an identifier, parent identifier,
/// the associated answer, and a list of previous related responses.
/// </summary>
/// <typeparam name="T">The type of the answer content.</typeparam>
public class ResponseMcp<T>
{
    /// <summary>
    /// Gets or sets the unique identifier for the current response object.
    /// This serves as a primary key for distinguishing individual instances within a collection or system.
    /// </summary>
    public string Id { get; set; } = "";

    /// <summary>
    /// Gets or sets the behavior type or action associated with the current response.
    /// This property is used to describe the nature or context of the response,
    /// detailing how it relates to the originating request or system interactions.
    /// </summary>
    public string Behavior { get; set; } = "";

    /// <summary>
    /// Gets or sets the provider associated with the current response object.
    /// This is could be an AI Agent provider such as ChatGPT and Gemini 
    /// </summary>
    public string Provider { get; set; } = "";

    /// <summary>
    /// Gets or sets the identifier for the machine learning model or system
    /// used to generate or process the response. This provides context
    /// about the source or technology behind the response.
    /// </summary>
    public string Model { get; set; } = "";

    /// <summary>
    /// Gets or sets the identifier of the parent response object.
    /// This links the current instance to its direct predecessor in a hierarchical structure or workflow.
    /// </summary>
    public string ParentId { get; set; } = "";

    /// <summary>
    /// Gets or sets the answer associated with the current response.
    /// </summary>
    /// <remarks>
    /// This property represents the main content or result encapsulated by the response.
    /// The generic type <typeparamref name="T"/> defines the type of the answer.
    /// </remarks>
    public T? Answer { get; set; }

    /// <summary>
    /// Gets or sets the raw representation of the answer provided in the response.
    /// This property contains the unprocessed or original format of the answer content.
    /// </summary> 
    public string AnswerRaw { get; set; } = "";

    /// <summary>
    /// Gets or sets a collection of previous response objects related to the current instance.
    /// These represent historical data or prior states associated with the current response object.
    /// </summary>
    public List<ResponseMcp<T>> PreviousContents { get; set; } = [];
}