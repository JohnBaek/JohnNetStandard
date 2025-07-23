using JohnIsDev.Core.Mcp.Models.Requests;
using JohnIsDev.Core.Mcp.Models.Responses;
using JohnIsDev.Core.Models.Responses;

namespace JohnIsDev.Core.Mcp.McpProvider.Interfaces;

/// <summary>
/// Represents an interface for an MCP client, which provides the capability to interact
/// with an MCP service using a predefined provider, API key, and endpoint.
/// </summary>
public interface IMcpClient
{
    /// <summary>
    /// Gets or sets the provider associated with the current response object.
    /// This is could be an AI Agent provider such as ChatGPT and Gemini 
    /// </summary>
    public string _provider { get; set; }

    /// <summary>
    /// Gets or sets the API key used for authenticating requests to the provider.
    /// This key is typically required to access the provider's services in a secure manner.
    /// </summary>
    public string _apiKey { get; set; }

    /// <summary>
    /// Gets or sets the endpoint for the current MCP (Managed Communication Protocol) client.
    /// The endpoint determines the base URL or address used for communication with the service provider.
    /// </summary>
    public string _endPoint { get; set; }

    /// <summary>
    /// Sends a query to the MCP service asynchronously and processes the response.
    /// Allows execution of optional actions after receiving the response.
    /// </summary>
    /// <typeparam name="T">The type of the response content expected from the service.</typeparam>
    /// <param name="request">The request object containing the query and related information for the MCP service.</param>
    /// <param name="afterActions">
    /// An optional asynchronous function that is executed after receiving the response.
    /// This function can be used for additional processing of the response.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous operation, containing a <see cref="ResponseData{T}"/>
    /// object with <see cref="ResponseMcp{T}"/> as its data type.
    /// </returns>
    public Task<ResponseData<ResponseMcp<T>>> SendQueryAsync<T>(
        RequestMcp request, Func<ResponseData<ResponseMcp<T>>, Task>? afterActions = null) where T : class;
}