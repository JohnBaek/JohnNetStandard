namespace JohnIsDev.Core.Features.Attributes;

/// <summary>
/// Attribute for Enum for color 
/// </summary>
[AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
public class EnumColorAttribute : Attribute
{
    /// <summary>
    /// Code of colors. ex) FFFFFF ( Hex )
    /// </summary>
    public string ColorCode { get; }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="colorCode"></param>
    public EnumColorAttribute(string colorCode)
    {
        ColorCode = colorCode;
    }
}