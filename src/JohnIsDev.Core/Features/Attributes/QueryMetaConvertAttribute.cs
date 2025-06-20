using JohnIsDev.Core.Models.Common.Enums;

namespace JohnIsDev.Core.Features.Attributes;

/// <summary>
/// Represents a custom attribute used for query metadata configuration.
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
public class QueryMetaConvertAttribute : Attribute
{
    /// <summary>
    /// Defines the type of search behavior for a query.
    /// Determines how the query should filter or match the data (e.g., Equals, Like, GreaterThan).
    /// </summary>
    public EnumQuerySearchType SearchType { get; set; }

    /// <summary>
    /// Indicates whether the associated property can be used for sorting in a query.
    /// Determines if sorting functionality is applicable to the property within a query context.
    /// </summary>
    public bool IsSortable { get; }

    /// <summary>
    /// Represents a custom attribute used for configuring query metadata. It allows specifying the type of query search and
    /// whether the property is sortable.
    /// </summary>
    public QueryMetaConvertAttribute(EnumQuerySearchType searchType, bool isSortable = false)
    {
        SearchType = searchType;
        IsSortable = isSortable;
    }
}