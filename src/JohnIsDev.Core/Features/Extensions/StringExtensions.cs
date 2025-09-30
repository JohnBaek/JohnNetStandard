using System.Net;
using System.Net.Mail;
using System.Net.Sockets;
using System.Text.RegularExpressions;

namespace JohnIsDev.Core.Features.Extensions;

/// <summary>
/// 문자열 확장
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// SHA 로 해싱한다.
    /// </summary>
    /// <param name="input">대상 문자열</param>
    /// <returns>해싱 결과</returns>
    public static string WithDateTime(this string input)
    {
        return $"[{DateTime.Now:yyyy-MM-dd hh:mm:ss}] {input}";
    }
    
    /// <summary>
    /// Guid 로 파싱한다.
    /// </summary>
    /// <param name="input">대상 문자열</param>
    /// <returns>해싱 결과</returns>
    public static Guid ToGuid(this string input)
    {
        if (Guid.TryParse(input, out Guid ourGuid))
        {
            return ourGuid;
        }

        return "00000000-0000-0000-0000-000000000000".ToGuid();
    }
    
    /// <summary>
    /// 데이터가 Null 또는 비어있는 경우 True
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static bool IsEmpty(this string input)
    {
        return string.IsNullOrWhiteSpace(input);
    }
    
    /// <summary>
    /// 데이터가 Null 또는 비어있는 경우 True
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static bool IsNotEmpty(this string input)
    {
        return !string.IsNullOrWhiteSpace(input);
    }

    /// <summary>
    /// String to Int
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static int ToInt(this string? input)
    {
        if (input == null)
            return 0;
            
        return int.Parse(input);
    }

    /// <summary>
    /// Checks if the current object, which is a regex pattern, matches the provided input string.
    /// </summary>
    /// <param name="pattern">The regex pattern to match against the input string.</param>
    /// <param name="input">The input string to be checked against the regex pattern.</param>
    /// <returns>True if the regex pattern matches the input string; otherwise, false.</returns>
    public static bool IsMatch(this string pattern, string input)
    {
        Regex regex = new Regex(pattern: pattern);
        return regex.Match(input).Success;
    }
    
    /// <summary>
    /// Checks if the current object 
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static bool IsEmail(this string input)
    {
        string pattern = @"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$";
        return input.IsMatch(pattern);
    }
    
    /// <summary>
    /// Validate Email value
    /// </summary>
    /// <param name="input">Input Email</param>
    /// <returns>Validation</returns>
    public static bool IsValidEmailWithDomain(this string input)
    {
        try
        {
            var addr = new MailAddress(input);
            string host = addr.Host;

            // 도메인 존재 확인
            IPHostEntry entry = Dns.GetHostEntry(host);

            // MX 레코드 검사 (간단 예시)
            var addresses = Dns.GetHostAddresses(host);
            return addresses.Any(address => address.AddressFamily == AddressFamily.InterNetwork);
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Removes any non-alphabetic characters from the input string, retaining only letters and Korean characters.
    /// </summary>
    /// <param name="input">The input string to normalize.</param>
    /// <returns>The normalized string containing only alphabetic and Korean characters, or an empty string if an error occurs.</returns>
    public static string NormalizeText(this string input)
    {
        string pattern = "[^a-zA-Z가-힣]";
        try
        {
            return Regex.Replace(input, pattern, "").ToUpper();
        }
        catch (Exception e)
        {
            return "";
        }
    }
    
    
    
    /// <summary>
    /// Validate Phone Number
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static bool IsValidPhoneNumber(this string input)
    {
        // Replace hyphens
        string onlyDigits = input.Replace("-", "");
        var regex = new Regex(@"^[0-9]+$");
        return regex.IsMatch(onlyDigits);
    }
}