using System.Security.Cryptography;
using JohnIsDev.Core.EntityFramework.Encryption.Attributes;
using JohnIsDev.Core.EntityFramework.Encryption.Encryption.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace JohnIsDev.Core.EntityFramework.Encryption.Encryption.Implements;

/// <summary>
/// Provides encryption and decryption functionalities using the AES (Advanced Encryption Standard) algorithm.
/// </summary>
public class AesEncryptionService : IEncryptionService
{
    /// <summary>
    /// Logger instance used to record and log messages, errors, and other information
    /// related to the operations performed by the AesEncryptionService class.
    /// Supports diagnostic and debugging functionality for encryption and decryption processes.
    /// </summary>
    private readonly ILogger<AesEncryptionService> _logger;

    /// <summary>
    /// The secret cryptographic key used by the AES encryption algorithm
    /// for securing sensitive data during encryption and decryption processes.
    /// </summary>
    private readonly byte[] _key;

    /// <summary>
    /// The initialization vector (IV) utilized in the AES encryption process.
    /// Ensures that encrypting the same data with the same key produces different ciphertexts,
    /// enhancing the security of the encryption algorithm.
    /// </summary>
    private readonly byte[] _iv;

    /// <summary>
    /// A string used as a prefix in the encryption process to differentiate
    /// or identify encrypted data. This helps manage and process the data effectively
    /// during encryption and decryption operations within the AesEncryptionService.
    /// </summary>
    private readonly string _prefix;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="logger">Logger instance used to record and log messages</param>
    /// <param name="configuration">Configuration instance</param>
    public AesEncryptionService(ILogger<AesEncryptionService> logger, IConfiguration configuration)
    {
        _logger = logger;
        _key = Convert.FromBase64String(configuration["Encryption:Key"]!);
        _iv = Convert.FromBase64String(configuration["Encryption:IV"]!);
        _prefix = configuration["Encryption:Prefix"]!;
    }

    /// <summary>
    /// Encrypts the specified plaintext value and returns the encrypted result.
    /// </summary>
    /// <param name="value">The plaintext string to encrypt.</param>
    /// <returns>The encrypted string result.</returns>
    public async Task<string> EncryptAsync(string value)
    {
        try
        {
            // Returns original text If it has no value
            if(string.IsNullOrWhiteSpace(value))
                return value;
            
            // Checks If its value has encrypted already 
            // Checks with prefix
            if (value.StartsWith(_prefix))
                return value;
            
            using Aes aes = Aes.Create();
            
            // Sets encryption objects
            aes.Key = _key;
            aes.IV = _iv;
            
            ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            using MemoryStream memoryStream = new();
            
            await using CryptoStream cryptoStream = new (memoryStream, encryptor, CryptoStreamMode.Write);
            await using (StreamWriter streamWriter = new StreamWriter(cryptoStream))
                await streamWriter.WriteAsync(value);
            
            string ciphertext = Convert.ToBase64String(memoryStream.ToArray());
            return $"{_prefix}{ciphertext}";
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return "";
        }
    }

    /// <summary>
    /// Decrypts the specified encrypted text and restores the original plaintext.
    /// </summary>
    /// <param name="cipherText">The encrypted text to be decrypted. If the text is null, empty, or does not contain the expected prefix, the original input is returned as-is.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the decrypted plaintext if successful, or an empty string in case of an error.</returns>
    public async Task<string> DecryptAsync(string cipherText)
    {
        try
        {
            // Returns original text If it has no value
            if(string.IsNullOrWhiteSpace(cipherText))
                return cipherText;
            
            // Returns original Text If it Has no Prefix due to it would be normal text 
            if (!cipherText.StartsWith(_prefix))
                return cipherText;
            
            byte[] cipherTextWithoutPrefixBytes = Convert.FromBase64String(cipherText.Substring(_prefix.Length)); 
            using Aes aes = Aes.Create();
            
            // Sets encryption objects
            aes.Key = _key;
            aes.IV = _iv;
            
            ICryptoTransform decrypt = aes.CreateDecryptor(aes.Key, aes.IV);
            using MemoryStream memoryStream = new MemoryStream(cipherTextWithoutPrefixBytes);
            await using CryptoStream cryptoStream = new CryptoStream(memoryStream, decrypt, CryptoStreamMode.Read);
            using StreamReader streamReader = new StreamReader(cryptoStream);

            return await streamReader.ReadToEndAsync();
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return "";
        }
    }

    /// <summary>
    /// Encrypts a plaintext string using AES encryption.
    /// </summary>
    /// <param name="value">The plaintext value to be encrypted.</param>
    /// <returns>The encrypted string prefixed with an identifier, or the original value if it is null, empty,
    /// already encrypted, or an error occurs during encryption.</returns>
    public string Encrypt(string value)
    {
        try
        {
            // Returns original text If it has no value
            if(string.IsNullOrWhiteSpace(value))
                return value;
            
            // Checks If its value has encrypted already 
            // Checks with prefix
            if (value.StartsWith(_prefix))
                return value;
            
            using Aes aes = Aes.Create();
            
            // Sets encryption objects
            aes.Key = _key;
            aes.IV = _iv;
            
            ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            using MemoryStream memoryStream = new();
            
            using CryptoStream cryptoStream = new (memoryStream, encryptor, CryptoStreamMode.Write);
            using (StreamWriter streamWriter = new StreamWriter(cryptoStream))
                streamWriter.Write(value);
            
            string ciphertext = Convert.ToBase64String(memoryStream.ToArray());
            return $"{_prefix}{ciphertext}";
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return "";
        }
    }

    /// <summary>
    /// Decrypts the provided cipher text using AES decryption and returns the original text.
    /// If the input is null, empty, or not prefixed, the original input is returned unchanged.
    /// </summary>
    /// <param name="cipherText">The encrypted text to be decrypted.</param>
    /// <returns>The decrypted original text, or the input unchanged if it does not meet decryption conditions.</returns>
    public string Decrypt(string cipherText)
    {
        try
        {
            // Returns original text If it has no value
            if(string.IsNullOrWhiteSpace(cipherText))
                return cipherText;
            
            // Returns original Text If it Has no Prefix due to it would be normal text 
            if (!cipherText.StartsWith(_prefix))
                return cipherText;
            
            byte[] cipherTextWithoutPrefixBytes = Convert.FromBase64String(cipherText.Substring(_prefix.Length)); 
            using Aes aes = Aes.Create();
            
            // Sets encryption objects
            aes.Key = _key;
            aes.IV = _iv;
            
            ICryptoTransform decrypt = aes.CreateDecryptor(aes.Key, aes.IV);
            using MemoryStream memoryStream = new MemoryStream(cipherTextWithoutPrefixBytes);
            using CryptoStream cryptoStream = new CryptoStream(memoryStream, decrypt, CryptoStreamMode.Read);
            using StreamReader streamReader = new StreamReader(cryptoStream);

            return streamReader.ReadToEnd();
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return "";
        }
    }
}