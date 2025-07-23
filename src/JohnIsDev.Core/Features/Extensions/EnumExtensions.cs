using System.ComponentModel;
using System.Reflection;
using JohnIsDev.Core.Features.Attributes;

namespace JohnIsDev.Core.Features.Extensions;

/// <summary>
/// Extensions of Enum
/// </summary>
public static class EnumExtensions
{
   /// <summary>
   /// To Returns Description Attribute value of Enum
   /// </summary>
   /// <param name="value"></param>
   /// <returns></returns>
    public static string GetDescription(this Enum value)
    {
        FieldInfo? field = value.GetType().GetField(value.ToString());
        
        // Is field null
        if (field == null)
            return "";
        
        // Get Description Attributes
        DescriptionAttribute? attribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
        return attribute == null ? value.ToString() : attribute.Description;
    }

    /// <summary>
    /// To Returns Colors Attribute value of Enum
    /// </summary>
    /// <param name="enumValue"></param>
    /// <returns></returns>
    public static string GetColor(this Enum enumValue)
    {
        FieldInfo? fieldInfo = enumValue.GetType().GetField(enumValue.ToString());
        if (fieldInfo == null)
            return "";

        if(fieldInfo.GetCustomAttributes(typeof(EnumColorAttribute), false).Length == 0)
            return "";
        
        EnumColorAttribute attribute = (EnumColorAttribute)fieldInfo.GetCustomAttributes(typeof(EnumColorAttribute), false).FirstOrDefault();
        return attribute.ColorCode; 
    }
}