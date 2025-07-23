namespace JohnIsDev.Core.EntityFramework.Encryption.Encryption.Interfaces;

/// <summary>
/// Represents a service for handling encryption and decryption operations.
/// Provides functionality to securely encrypt and decrypt data.
/// Designed to be implemented for use cases requiring data protection.
/// </summary>
public interface IEncryptionService
{
    /// <summary>
    /// Encrypts the specified plaintext value and returns the encrypted result.
    /// </summary>
    /// <param name="value">The plaintext string to encrypt.</param>
    /// <returns>The encrypted string result.</returns>
    Task<string> EncryptAsync(string value);

    /// <summary>
    /// Decrypts the specified encrypted value and returns the decrypted result.
    /// </summary>
    /// <param name="cipherText">The encrypted string to decrypt.</param>
    /// <returns>The decrypted string result.</returns>
    Task<string> DecryptAsync(string cipherText);
    
    
    /// <summary>
    /// Encrypts the specified plaintext value and returns the encrypted result.
    /// </summary>
    /// <param name="value">The plaintext string to encrypt.</param>
    /// <returns>The encrypted string result.</returns>
    string Encrypt(string value);

    /// <summary>
    /// Decrypts the specified encrypted value and returns the decrypted result.
    /// </summary>
    /// <param name="cipherText">The encrypted string to decrypt.</param>
    /// <returns>The decrypted string result.</returns>
    string Decrypt(string cipherText);
}