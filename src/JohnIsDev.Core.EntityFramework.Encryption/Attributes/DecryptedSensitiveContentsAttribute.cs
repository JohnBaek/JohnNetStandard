namespace JohnIsDev.Core.EntityFramework.Encryption.Attributes;

/// <summary>
/// An attribute used to indicate that a property contains decrypted sensitive information.
/// This attribute can be applied to properties to signify that the value of the decorated
/// property is sensitive and has been decrypted from a secure source. It can serve as
/// an indicator for developers or frameworks working with the class to handle such
/// properties with additional security or care.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class DecryptedSensitiveContentsAttribute : Attribute
{
}