using System.ComponentModel.DataAnnotations;
using JohnIsDev.Core.Features.Extensions;

namespace JohnIsDev.Core.Features.Attributes;

/// <summary>
/// A custom model validation attribute to validate email format
/// only when a value is provided
/// </summary>
public class PhoneNumberAttribute : ValidationAttribute
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
            string? phoneNumber = value as string;
            if(phoneNumber is null)
                return ValidationResult.Success;
            
            return phoneNumber.IsValidPhoneNumber() ? ValidationResult.Success : new ValidationResult(ErrorMessage);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        return null;
    }
}