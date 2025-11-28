using FluentAssertions;
using JohnIsDev.Core.Features.Attributes;
using JohnIsDev.Core.Features.Helpers;
using JohnIsDev.Core.Models.Common.Enums;
using JohnIsDev.Core.Models.Common.Query;

namespace JohnIsDev.Core.Test.Features.Helpers;

/// <summary>
/// Defines unit tests for the <see cref="EntityMapper"/> class,
/// ensuring the functionality of its methods for extracting query search metadata from attributes.
/// </summary>
public class EntityMapperTest
{
    /// <summary>
    /// Validates the functionality of extracting search metadata from attributes applied to the properties
    /// of a given class type using the <see cref="EntityMapper.ToEntry{T}"/> method.
    /// </summary>
    /// <remarks>
    /// The method tests the following scenarios:
    /// - Validation that properties with specific attributes can be successfully mapped to corresponding
    /// <see cref="RequestQuerySearchMeta"/> objects.
    /// - Validation of the correctness of the extracted search metadata, including the field name and its associated
    /// search type.
    /// </remarks>
    /// <exception cref="FluentAssertions.Execution.AssertionFailedException">
    /// Thrown if any of the assertions regarding extracted metadata fails.
    /// </exception>
    [Fact]
    public void ExtractSearchMetaFromAttributes_CanExtractSearchMetaFromAttributes()
    {
        // Arrange
        // Act
        List<RequestQuerySearchMeta> searchMetas = EntityMapper.ToEntry<TestClass>();
  
        // Assert
        searchMetas.FirstOrDefault(i => i.Field == "Like")?.SearchType.Should().Be(EnumQuerySearchType.Like);
        searchMetas.FirstOrDefault(i => i.Field == "Equals")?.SearchType.Should().Be(EnumQuerySearchType.Equals);
        searchMetas.FirstOrDefault(i => i.Field == "GreaterThen")?.SearchType.Should().Be(EnumQuerySearchType.GreaterThen);
        searchMetas.FirstOrDefault(i => i.Field == "LessThen")?.SearchType.Should().Be(EnumQuerySearchType.LessThen);
        searchMetas.FirstOrDefault(i => i.Field == "EqualsNumeric")?.SearchType.Should().Be(EnumQuerySearchType.EqualsNumeric);
        searchMetas.FirstOrDefault(i => i.Field == "NumericOrEnums")?.SearchType.Should().Be(EnumQuerySearchType.NumericOrEnums);
        searchMetas.FirstOrDefault(i => i.Field == "StartDate")?.SearchType.Should().Be(EnumQuerySearchType.StartDate);
        searchMetas.FirstOrDefault(i => i.Field == "EndDate")?.SearchType.Should().Be(EnumQuerySearchType.EndDate);
        searchMetas.FirstOrDefault(i => i.Field == "RangeDate")?.SearchType.Should().Be(EnumQuerySearchType.RangeDate);
    }


    /// <summary>
    /// Ensures that the <see cref="EntityMapper.ToEntry{T}"/> method caches metadata
    /// derived from the annotated properties of the specified type after the first invocation.
    /// </summary>
    /// <remarks>
    /// The method performs the following verifications:
    /// - Confirms that the result of the first call returns non-null metadata by inspecting the attributes of the provided type.
    /// - Validates that subsequent calls return cached metadata instead of recomputing it.
    /// - Verifies that the cached metadata reference remains identical for subsequent calls.
    /// - Asserts the equivalence of data between the first and subsequent calls, ensuring the metadata values are consistent.
    /// </remarks>
    /// <exception cref="FluentAssertions.Execution.AssertionFailedException">
    /// Thrown if any of the expected caching behavior or metadata equivalence assertions fail.
    /// </exception>
    [Fact]
    public void ExtractSearchMetaFromAttributes_ShouldCached()
    {
        // Call first - Should be Reflected
        var firstCall = EntityMapper.ToEntry<TestClass>();
        firstCall.Should().NotBeNull();
        
        // Call second - Should be Cached
        var secondCall = EntityMapper.ToEntry<TestClass>();
        secondCall.Should().NotBeNull();
        
        // ReferenceEquals
        ReferenceEquals(firstCall, secondCall).Should().BeTrue();
        
        // BeEquivalentTo - Compares to be same values
        secondCall.Should().BeEquivalentTo(firstCall);
    }
}

public class TestClass
{
    [QueryMetaConvert(EnumQuerySearchType.Like)]
    public string Like { get; set; } = "";
    
    [QueryMetaConvert(EnumQuerySearchType.Equals)]
    public string Equals { get; set; } = "";
    
    [QueryMetaConvert(EnumQuerySearchType.GreaterThen)]
    public string GreaterThen { get; set; } = "";
    
    [QueryMetaConvert(EnumQuerySearchType.LessThen)]
    public string LessThen { get; set; } = "";
    
    [QueryMetaConvert(EnumQuerySearchType.EqualsNumeric)]
    public string EqualsNumeric { get; set; } = "";
    
    [QueryMetaConvert(EnumQuerySearchType.NumericOrEnums)]
    public string NumericOrEnums { get; set; } = "";
    
    [QueryMetaConvert(EnumQuerySearchType.StartDate)]
    public string StartDate { get; set; } = "";
    
    [QueryMetaConvert(EnumQuerySearchType.EndDate)]
    public string EndDate { get; set; } = "";
    
    [QueryMetaConvert(EnumQuerySearchType.RangeDate)]
    public string RangeDate { get; set; } = "";
}