using System.Diagnostics.CodeAnalysis;
using System.IO.Compression;
using System.Text;
using JohnIsDev.Core.Models.Common.Enums;
using JohnIsDev.Core.Models.Responses;
using Microsoft.Extensions.Logging;

namespace JohnIsDev.Core.Features.Utils;

/// <summary>
/// Represents a file Utils
/// </summary>
[SuppressMessage("ReSharper", "UseUtf8StringLiteral")]
public class FileUtils(ILogger<FileUtils> logger)
{
    /// <summary>
    /// Known Magic numbers for files
    /// </summary>
    private static readonly Dictionary<string, List<byte[]>> KnownSignatures = new()
    {
        { "pdf", [new byte[] { 0x25, 0x50, 0x44, 0x46 }] },
        { "png", [new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A }] },
        { "jpg", [
            new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 },
            new byte[] { 0xFF, 0xD8, 0xFF, 0xE1 },
            new byte[] { 0xFF, 0xD8, 0xFF, 0xDB },
            new byte[] { 0xFF, 0xD8, 0xFF, 0xEE } // 추가 JPEG 시그니처
        ]},
        { "xlsx", [new byte[] { 0x50, 0x4B, 0x03, 0x04 }] }, // ZIP 시그니처이므로 추가 검증 필요
        { "docx", [new byte[] { 0x50, 0x4B, 0x03, 0x04 }] }, // DOCX 추가
        { "zip", [new byte[] { 0x50, 0x4B, 0x03, 0x04 }] }
    };

    /// <summary>
    /// IsValidMagicNumber
    /// </summary>
    /// <param name="bytes">bytes</param>
    /// <param name="expectedExtension">expectedExtension</param>
    /// <returns></returns>
    public ResponseData<bool> IsValidMagicNumber(byte[] bytes, string expectedExtension)
    {
        try
        {
            if (bytes.Length < 4)
                return new ResponseData<bool>(EnumResponseResult.Error,"FileNotValid","올바른 파일이 아닙니다.");
            
            // Cut useless character
            string extension = expectedExtension.ToLower().Replace(".", "");
            switch (extension)
            {
                // In case Text file
                case "txt":
                    return ValidateTextFile(bytes);
                // In case XLSX file
                case "xlsx":
                    return ValidateOfficeDocument(bytes, "xl/");
            }

            // Validate a magic number
            if (!KnownSignatures.TryGetValue(extension, out var willTestSignatureBytes))
            {
                logger.LogWarning($"Unknown file extension for magic number validation: {extension}");
                return new ResponseData<bool>(EnumResponseResult.Error,"FileNotValid","올바른 파일이 아닙니다.");
            }
            
            // Get all testable Signatures

            // Tests all Signature
            foreach (byte[] signature in willTestSignatureBytes)
            {
                // pass If less 
                if (bytes.Length < signature.Length) 
                    continue;
                
                bool matches = !signature.Where((t, i) => bytes[i] != t).Any();
                if (matches)
                    return new ResponseData<bool>(EnumResponseResult.Success, "", "");
            }

            return new ResponseData<bool>(EnumResponseResult.Error, "FileNotValid", "올바른 파일이 아닙니다.");
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            return new ResponseData<bool>(EnumResponseResult.Error,"FileNotValid","에러가 발생했습니다.");
        }
    }

    /// <summary>
    /// ValidateTextFile
    /// </summary>
    /// <param name="bytes"></param>
    /// <returns></returns>
    private ResponseData<bool> ValidateTextFile(byte[] bytes)
    {
        try
        {
            // BOM 검사
            if (HasUtf8Bom(bytes) || HasUtf16Bom(bytes))
                return new ResponseData<bool>(EnumResponseResult.Success, "", "");

            // UTF-8 유효성 검사
            try
            {
                var decoder = Encoding.UTF8.GetDecoder();
                decoder.Fallback = DecoderFallback.ExceptionFallback;
            
                char[] chars = new char[decoder.GetCharCount(bytes, 0, bytes.Length)];
                decoder.GetChars(bytes, 0, bytes.Length, chars, 0);
            
                return new ResponseData<bool>(EnumResponseResult.Success, "", "");
            }
            catch (DecoderFallbackException)
            {
                return new ResponseData<bool>(EnumResponseResult.Error, "InvalidTextFile", "유효하지 않은 텍스트 파일입니다.");
            }
        }
        catch (Exception e)
        {
            logger.LogError(e, "텍스트 파일 검증 중 오류 발생");
            return new ResponseData<bool>(EnumResponseResult.Error, "ValidationError", "파일 검증 중 오류가 발생했습니다.");
        }
    }

    /// <summary>
    /// Office 문서 내부 구조 검증
    /// </summary>
    private ResponseData<bool> ValidateOfficeDocument(byte[] bytes, string expectedPath)
    {
        try
        {
            using var stream = new MemoryStream(bytes);
            using var archive = new ZipArchive(stream, ZipArchiveMode.Read);
        
            // XLSX should have xl/ Folder
            // DOCX should have word/ Folder
            bool isValid = archive.Entries.Any(entry => entry.FullName.StartsWith(expectedPath));
            return new ResponseData<bool>(isValid ? EnumResponseResult.Success : EnumResponseResult.Error,"","");
        }
        catch(Exception e)
        {
            logger.LogError(e, e.Message);
            return new ResponseData<bool>(EnumResponseResult.Error,"","");
        }
    }
    
    private static bool HasUtf8Bom(byte[] bytes) =>
        bytes is [0xEF, 0xBB, 0xBF, ..];

    private static bool HasUtf16Bom(byte[] bytes) =>
        bytes.Length >= 2 && ((bytes[0] == 0xFF && bytes[1] == 0xFE) || (bytes[0] == 0xFE && bytes[1] == 0xFF));
}