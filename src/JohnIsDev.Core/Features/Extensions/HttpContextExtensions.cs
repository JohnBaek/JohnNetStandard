using Microsoft.AspNetCore.Http;

namespace JohnIsDev.Core.Features.Extensions;

/// <summary>
/// Extension for HttpContext
/// </summary>
public static class HttpContextExtensions
{
    /// <summary>
    /// Gets the remote client IP address, considering X-Forwarded-For header if present.
    /// </summary>
    /// <param name="context">The HttpContext instance.</param>
    /// <returns>The client IP address as a string, or null if not found.</returns>
    public static string? GetClientIpAddress(this HttpContext context)
    {
        try
        {
            // Get IP Address from X-Forwarded-For in request headers
            string? xForwardedFor = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();

            // If X-Forwarded-For is present, use the first IP (comma-separated)
            if (string.IsNullOrWhiteSpace(xForwardedFor)) 
                return context.Connection.RemoteIpAddress?.ToString();
            
            string? ip = xForwardedFor.Split(',').FirstOrDefault()?.Trim();
            return !string.IsNullOrWhiteSpace(ip) ? ip :
                // Fallback to RemoteIpAddress
                context.Connection.RemoteIpAddress?.ToString();
        }
        catch
        {
            // In case of any exception, return null
            return null;
        }
    }
}