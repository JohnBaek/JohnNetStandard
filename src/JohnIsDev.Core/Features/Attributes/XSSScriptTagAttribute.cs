using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace JohnIsDev.Core.Features.Attributes;

/// <summary>
/// Checks XSS Script Tag
/// </summary>
public class XSSScriptTagAttribute : ValidationAttribute
{
    /// <summary>
    /// 입력값에 XSS 위험 문자열이 포함되어 있으면 ValidationResult 반환
    /// </summary>
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        try
        {
            string? input = value as string;
            if (string.IsNullOrWhiteSpace(input))
                return ValidationResult.Success;

            // XSS 위험 태그/패턴 정규식 (필요시 추가)
            var xssPattern = new Regex(
                @"(<script\b[^>]*>(.*?)</script>)|(<iframe\b[^>]*>(.*?)</iframe>)|(<img\b[^>]*>)|(javascript:)",
                RegexOptions.IgnoreCase | RegexOptions.Singleline
            );

            return xssPattern.IsMatch(input)
                ? new ValidationResult(ErrorMessage ?? "Input contains forbidden script tags.")
                : ValidationResult.Success;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        return null;
    }
}