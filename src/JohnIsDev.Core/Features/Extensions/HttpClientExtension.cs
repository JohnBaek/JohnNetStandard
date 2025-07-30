using Newtonsoft.Json;

namespace JohnIsDev.Core.Features.Extensions;

/// <summary>
/// An Extension class for the HttpClient
/// </summary>
public static class HttpClientExtension
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="client"></param>
    /// <param name="url"></param>
    /// <typeparam name="TResponse"></typeparam>
    /// <returns></returns>
    public static async Task<TResponse?> PostAsync<TResponse>(this HttpClient client, string url , object request)
    {
        // Prepares a Request Object
        string requestJson = JsonConvert.SerializeObject(request);
        Console.WriteLine($"requestJson: {requestJson}");
        StringContent content = new StringContent(requestJson, System.Text.Encoding.UTF8, "application/json");
        
        // Invokes a Post request to endpoint
        HttpResponseMessage response = await client.PostAsync(url, content);
            
        // Checks on response status
        response.EnsureSuccessStatusCode();
            
        // Reads a response body 
        string responseBody = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<TResponse>(responseBody);
    }
}