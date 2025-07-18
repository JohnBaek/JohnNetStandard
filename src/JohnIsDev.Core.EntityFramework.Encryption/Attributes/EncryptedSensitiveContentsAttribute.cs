namespace JohnIsDev.Core.EntityFramework.Encryption.Attributes;

/// <summary>
/// Represents an attribute to denote that the property contains sensitive
/// contents that should be encrypted in the context of data storage or processing.
/// </summary>
/// <remarks>
/// This attribute can be applied to properties within a class that require
/// encryption to protect sensitive information. It is often used in conjunction
/// with implementations that handle data encryption and decryption transparently.
/// </remarks>
/// <example>
/// This attribute is primarily used for identifying properties that secure
/// encryption needs to be applied to at runtime or during data manipulation.
/// </example>
[AttributeUsage(AttributeTargets.Property)]
public class EncryptedSensitiveContentsAttribute : Attribute
{
}