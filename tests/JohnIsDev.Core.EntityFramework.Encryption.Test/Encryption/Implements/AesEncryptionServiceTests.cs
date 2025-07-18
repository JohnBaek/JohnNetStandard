using System.Text;
using FluentAssertions;
using JohnIsDev.Core.EntityFramework.Encryption.Encryption.Implements;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;

namespace JohnIsDev.Core.EntityFramework.Encryption.Test.Encryption.Implements;

/// <summary>
/// Provides encryption and decryption functionalities using the AES (Advanced Encryption Standard) algorithm.
/// </summary>
public class AesEncryptionServiceTests
{
    private readonly Mock<ILogger<AesEncryptionService>> _mockLogger;
    private readonly Mock<IConfiguration> _mockConfiguration;
    private readonly AesEncryptionService _encryptionService;

    private readonly string _testKey;
    private readonly string _testIv;
    private readonly string _testPrefix;
    
    public AesEncryptionServiceTests()
    {
        _mockLogger = new Mock<ILogger<AesEncryptionService>>();
        _mockConfiguration = new Mock<IConfiguration>();

        // Key: 32 bytes (256 bits), IV: 16 bytes (128 bits)
        _testKey = Convert.ToBase64String(Encoding.UTF8.GetBytes("12345678901234567890123456789012")); // 32 bytes
        _testIv = Convert.ToBase64String(Encoding.UTF8.GetBytes("1234567890123456")); // 16 bytes
        _testPrefix = "[ENC]";

        var mockConfSection = new Mock<IConfigurationSection>();
        mockConfSection.Setup(a => a.Value).Returns(_testKey);
        _mockConfiguration.Setup(a => a.GetSection("Encryption:Key")).Returns(mockConfSection.Object);
        _mockConfiguration.Setup(c => c["Encryption:Key"]).Returns(_testKey);
        _mockConfiguration.Setup(c => c["Encryption:IV"]).Returns(_testIv);
        _mockConfiguration.Setup(c => c["Encryption:Prefix"]).Returns(_testPrefix);
        _encryptionService = new AesEncryptionService(_mockLogger.Object, _mockConfiguration.Object);
    }


    /// <summary>
    /// Validates that the encryption and decryption process using AES results in the original text being restored after a complete round-trip.
    /// </summary>
    /// <returns>
    /// A task representing the asynchronous test operation. Confirms that:
    /// - The encrypted text is not null.
    /// - The encrypted text differs from the original plaintext.
    /// - The encrypted text includes a predefined prefix.
    /// - Decrypting the encrypted text reproduces the original plaintext.
    /// </returns>
    [Fact]
    public async Task EncryptAndDecrypt_ShouldReturnOriginalText_WhenRoundTripIsSuccessful()
    {
        // Arrange
        var originalText = "Secret message";

        // Act
        var encryptedText = await _encryptionService.EncryptAsync(originalText);
        var decryptedText = await _encryptionService.DecryptAsync(encryptedText);

        // Assert
        encryptedText.Should().NotBeNull();
        encryptedText.Should().StartWith(_testPrefix);
        originalText.Should().NotBe(encryptedText);
        originalText.Should().Be(decryptedText);
    }

    /// <summary>
    /// Verifies that the encryption method returns the original input value when the input is null, empty, or consists only of whitespace.
    /// </summary>
    /// <param name="plainText">The input string to encrypt, which can be null, empty, or whitespace.</param>
    /// <returns>
    /// A task representing the asynchronous test operation. Confirms that the returned encrypted value is the same as the input value for null, empty, or whitespace inputs.
    /// </returns>
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task EncryptAsync_ShouldReturnOriginalValue_WhenValueIsNullOrWhitespace(string plainText)
    {
        // Act
        var result = await _encryptionService.EncryptAsync(plainText);

        // Assert
        plainText.Should().Be(result);
    }

    /// <summary>
    /// Verifies that if a value is already encrypted (indicated by the predefined prefix), the EncryptAsync method returns the original encrypted value without re-encrypting it.
    /// </summary>
    /// <returns>
    /// A task representing the asynchronous test operation. Ensures that:
    /// - The input string already prefixed as encrypted is not altered.
    /// - The output is identical to the input encrypted value.
    /// </returns>
    [Fact]
    public async Task EncryptAsync_ShouldReturnOriginalValue_WhenValueIsAlreadyEncrypted()
    {
        // Arrange
        var alreadyEncryptedText = $"{_testPrefix}someEncryptedDataHere";

        // Act
        var result = await _encryptionService.EncryptAsync(alreadyEncryptedText);

        // Assert
        alreadyEncryptedText.Should().Be(result);
    }

    /// <summary>
    /// Ensures that when a plaintext value, which is not encrypted, is passed to the decryption method,
    /// the original value is returned without modification.
    /// </summary>
    /// <returns>
    /// A task representing the asynchronous test operation. Verifies that:
    /// - The decryption process does not alter plaintext values.
    /// - The returned value matches the input plaintext.
    /// </returns>
    [Fact]
    public async Task DecryptAsync_ShouldReturnOriginalValue_WhenValueIsNotEncrypted()
    {
        // Arrange
        var plainText = "이것은 암호화된 텍스트가 아닙니다.";

        // Act
        var result = await _encryptionService.DecryptAsync(plainText);
        
        // Assert
        plainText.Should().Be(result);
    }

    /// <summary>
    /// Ensures the decryption process returns an empty string and logs an error when the provided ciphertext is invalid or malformed.
    /// </summary>
    /// <returns>
    /// A task representing the asynchronous test operation. Confirms that:
    /// - The returned result from decryption is an empty string.
    /// - An error is logged exactly once using the logging mechanism.
    /// </returns>
    [Fact]
    public async Task DecryptAsync_ShouldReturnEmptyAndLogError_WhenDecryptionFails()
    {
        // Arrange
        var malformedCipherText = $"{_testPrefix}this-is-not-valid-base64!";
        
        // Act
        var result = await _encryptionService.DecryptAsync(malformedCipherText);

        // Assert
        "".Should().Be(result);
        _mockLogger.Verify(
            logger => logger.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => true), // 메시지 내용은 상관없음
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)),
            Times.Once);
        
    }
}