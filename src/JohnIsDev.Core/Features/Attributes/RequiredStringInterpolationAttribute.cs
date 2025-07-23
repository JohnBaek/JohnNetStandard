using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Resources;

namespace JohnIsDev.Core.Features.Attributes;

/// <summary>
/// Attribute with string interpolation applied
/// </summary>
public class RequiredStringInterpolationAttribute : RequiredAttribute
{
    /// <summary>
    /// Caller
    /// </summary>
    private readonly ResourceManager _resourceManager; 
    
    /// <summary>
    /// resourceName
    /// </summary>
    private readonly string _resourceName;  
    
    /// <summary>
    /// interpolate Message
    /// </summary>
    private readonly string _interpolate;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="resourceManager">resourceManager</param>
    /// <param name="resourceName">resourceName</param>
    /// <param name="interpolate">interpolate</param>
    public RequiredStringInterpolationAttribute(ResourceManager resourceManager, string resourceName, string interpolate)
    {
        _resourceManager = resourceManager;
        _resourceName = resourceName;
        _interpolate = interpolate;
    }

    /// <summary>
    /// Format Error Message
    /// </summary>
    /// <param name="name">name</param>
    /// <exception cref="InvalidOperationException"></exception>
    public override string FormatErrorMessage(string name)
    {
        // Get Resource
        string? errorMessage = _resourceManager.GetString(_resourceName);
        
        // If Invalid
        if (errorMessage == null)
            throw new InvalidOperationException($"Resource key '{_resourceName}' was not found in resource file.");
        
        // Return Formated Message
        return string.Format(CultureInfo.CurrentCulture, errorMessage, _interpolate);
    }
}