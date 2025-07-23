using JohnIsDev.Core.Mcp.McpProvider.Interfaces;
using JohnIsDev.Core.Mcp.Models.Requests;
using JohnIsDev.Core.Mcp.Models.Responses;
using JohnIsDev.Core.Models.Common.Enums;
using JohnIsDev.Core.Models.Responses;
using Microsoft.Extensions.Logging;

namespace JohnIsDev.Core.Mcp.McpProvider.Implements;

/// <summary>
/// Represents a common implementation of the MCP client interface,
/// providing functionality to interact with MCP services.
/// </summary>
public class CommonMcpClient : IMcpClient
{
    /// <summary>
    /// Gets or sets the provider associated with the MCP client.
    /// The provider defines the service endpoint or configuration for
    /// interacting with the MCP services.
    /// </summary>
    public string _provider { get; set; }

    /// <summary>
    /// Gets or sets the API key used for authenticating requests to the MCP service.
    /// The API key is required for securing communications and identifying the client.
    /// </summary>
    public string _apiKey { get; set; }

    /// <summary>
    /// Gets or sets the endpoint URL associated with the MCP client.
    /// The endpoint specifies the base URL or address of the MCP service
    /// to which the client will send requests.
    /// </summary>
    public string _endPoint { get; set; }

    /// <summary>
    /// Gets or sets the type associated with the MCP client.
    /// The type specifies the category or classification of the client,
    /// often used to differentiate between various implementations or configurations.
    /// </summary>
    public ILogger<CommonMcpClient> _Logger { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="CommonMcpClient"/> class with specified parameters.
    /// </summary>
    /// <param name="provider">The provider for MCP services.</param>
    /// <param name="apiKey">The API key for authentication.</param>
    /// <param name="logger">Logger</param>
    public CommonMcpClient(string provider, string apiKey,  ILogger<CommonMcpClient> logger)
    {
        _provider = "";
        _apiKey = apiKey;
        _endPoint = "";
        _Logger = logger;
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="afterActions"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task<ResponseData<ResponseMcp<T>>> SendQueryAsync<T>(RequestMcp request, Func<ResponseData<ResponseMcp<T>>, Task>? afterActions = null) where T : class
    {

        return null;
        // try
        // {
        //     var payload = new
        //     {
        //         contents = request.Messages.Select(msg => new
        //         {
        //             parts = new[] { new
        //             {
        //                 text = msg.Text 
        //             }}
        //         }).ToArray()
        //     };
        //     HttpResponseMessage response = await httpClient.PostAsJsonAsync(RequestUrl, payload);
        //
        // }
        // catch (Exception e)
        // {
        //     _Logger.LogError(e, e.Message);
        //     return new ResponseData<ResponseMcp<T>>(EnumResponseResult.Error, "COMMON_DATABASE_ERROR", "");
        // }
    }
}