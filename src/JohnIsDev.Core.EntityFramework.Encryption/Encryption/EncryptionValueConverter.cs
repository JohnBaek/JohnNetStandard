using JohnIsDev.Core.EntityFramework.Encryption.Encryption.Interfaces;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace JohnIsDev.Core.EntityFramework.Encryption.Encryption;

/// <summary>
/// A value converter designed for encrypting and decrypting string values
/// when using Entity Framework Core.
/// </summary>
/// <remarks>
/// This class enables secure encryption and decryption of string properties
/// during database operations. It uses an implementation of the
/// <see cref="IEncryptionService"/> interface to handle encryption and decryption process.
/// The encryption is applied when saving data to the database, and the decryption
/// is performed when reading data from the database.
/// </remarks>
public class EncryptionValueConverter : ValueConverter<string, string>
{
    
    /// <summary>
    /// A value converter class used for encrypting and decrypting string values
    /// in the context of Entity Framework Core.
    /// </summary>
    /// <remarks>
    /// This class extends the <see cref="ValueConverter{TModel, TProvider}"/> class
    /// and provides functionality to securely transform string values during
    /// database read and write operations. It uses an implementation of
    /// <see cref="IEncryptionService"/> to perform encryption and decryption.
    /// </remarks>
    public EncryptionValueConverter(IEncryptionService encryptionService, ConverterMappingHints? mappingHints = null)
        : base(
            // Converting to database
            i => encryptionService.Encrypt(i),    
            // Converting from database
            i => encryptionService.Decrypt(i),    
            mappingHints
        )
    { }
}