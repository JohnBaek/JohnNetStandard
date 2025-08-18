using JohnIsDev.Core.Models.Common.Enums;

namespace JohnIsDev.Core.Models.Responses;

/// <summary>
/// Represents a standardized response object that encapsulates the result of an operation, including a response code, message, and result type.
/// </summary>
public class Response
{
    /// <summary>
    /// Represents a generic response model intended to encapsulate the result of an operation,
    /// containing properties for standardized error handling, messages, and any other associated information.
    /// </summary>
    public Response()
    {
    }

    /// <summary>
    /// Represents a standardized response object that encapsulates the result of an operation,
    /// providing detailed information such as response code, message, and result type.
    /// </summary>
    public Response(EnumResponseResult result, string code, string message)
    {
        Code = code;
        Message = message;
        Result = result;
    }

    /// <summary>
    /// Encapsulates the outcome of an operation, detailing relevant information
    /// such as error states, status messages, or associated data.
    /// </summary>
    public Response(EnumResponseResult result)
    {
        Code = "";
        Message = "";
        Result = result;
    }

    /// <summary>
    /// 
    /// </summary>
    public string Code { get; set; } = "";
    
    /// <summary>
    /// 
    /// </summary>
    public string Message { get; set; } = "";

    /// <summary>
    /// Indicates whether the current response is associated with an authenticated user or process.
    /// </summary>
    public bool IsAuthenticated { get; set; } = false;

    /// <summary>
    /// Represents the result of an operation, indicating its state such as success, warning, or error.
    /// </summary>
    public EnumResponseResult Result { get; set; } = EnumResponseResult.Error;

    /// <summary>
    /// Is Response Success?
    /// </summary>
    public bool Success => Result == EnumResponseResult.Success;
    
    /// <summary>
    /// Is Response Error?
    /// </summary>
    public bool Error => Result == EnumResponseResult.Error;
    
    /// <summary>
    /// Is Response Warning?
    /// </summary>
    public bool Warning => Result == EnumResponseResult.Warning;

}