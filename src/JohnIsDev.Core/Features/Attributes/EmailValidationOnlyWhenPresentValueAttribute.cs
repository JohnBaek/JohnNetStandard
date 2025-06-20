using System.ComponentModel.DataAnnotations;
using JohnIsDev.Core.Features.Extensions;

namespace JohnIsDev.Core.Features.Attributes;

/// <summary>
/// A custom model validation attribute to validate email format
/// only when a value is provided
/// </summary>
public class EmailValidationOnlyWhenPresentValueAttribute : ValidationAttribute
{
    /// <summary>
    /// Validates to provided value
    /// </summary>
    /// <param name="value">A value provided</param>
    /// <param name="validationContext">Context of validations</param>
    /// <returns></returns>
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        try
        {
            switch (value)
            {
                // Allow empty values
                case string email when string.IsNullOrWhiteSpace(email):
                    return ValidationResult.Success;
                
                // Validates email format if not empty
                case string email:
                {
                    return !email.IsValidEmailWithDomain() ? new ValidationResult(ErrorMessage) : ValidationResult.Success;
                }
                case HashSet<string> set:
                {
                    HashSet<string>? emails = set;
                    return emails.Any(email => !email.IsValidEmailWithDomain()) 
                        ? new ValidationResult(ErrorMessage) : ValidationResult.Success;
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        return null;
    }
}