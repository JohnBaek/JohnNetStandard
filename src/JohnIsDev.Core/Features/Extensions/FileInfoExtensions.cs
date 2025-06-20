using System.Security.Cryptography;

namespace JohnIsDev.Core.Features.Extensions;


/// <summary>
/// Extensions of FileInfo
/// </summary>
public static class FileInfoExtensions
{
    /// <summary>
    /// Returns CheckSum
    /// </summary>
    /// <param name="fileInfo"></param>
    /// <returns></returns>
    public static string CalculateChecksum(this FileInfo fileInfo)
    {
        using var sha256 = SHA256.Create();
        using var fileStream = fileInfo.OpenRead();
        
        var hash = sha256.ComputeHash(fileStream);
        return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
    }
}