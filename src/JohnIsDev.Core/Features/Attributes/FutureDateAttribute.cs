using System.ComponentModel.DataAnnotations;

namespace JohnIsDev.Core.Features.Attributes;

/// <summary>
/// Validates that the value is a future date.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class FutureDateAttribute : ValidationAttribute
{
    /// <summary>
    /// Determines whether the specified value is a valid future date.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="validationContext"></param>
    /// <returns></returns>
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        try
        {
            // Return success for non-DateTime values
            if(value is not DateTime date)
                return ValidationResult.Success;

            // Ensure the date is in the future
            if (date <= DateTime.Now)
                return new ValidationResult(ErrorMessage ?? "Date should be in the future");

            return ValidationResult.Success;
        }
        catch
        {
            return ValidationResult.Success;
        }
    }
}