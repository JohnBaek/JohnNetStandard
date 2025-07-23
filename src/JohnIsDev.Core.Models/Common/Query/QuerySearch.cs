using JohnIsDev.Core.Models.Common.Enums;

namespace JohnIsDev.Core.Models.Common.Query;

/// <summary>
/// 검색 정보 정의
/// </summary>
public class QuerySearch
{
    /// <summary>
    /// 필드정보 
    /// </summary>
    public required string Field { get; set; }


    /// <summary>
    /// 키워드정보
    /// </summary>
    private string? _keyword;
    
    /// <summary>
    /// 키워드 정보 
    /// </summary>
    public string? Keyword
    {
        get => _keyword?.Replace("\\b","").Replace("\b","");
        set => _keyword = value;
    }
    
    /// <summary>
    /// Numeric Type
    /// </summary>
    public EnumQuerySearchType NumericType {
        get;
        set;
    }
    
    /// <summary>
    /// RangeDate Type
    /// </summary>
    public EnumQuerySearchType RangeDateType {
        get;
        set;
    }

    /// <summary>
    /// Search Start Date
    /// </summary>
    public DateTime StartDate { get; set; }
    
    /// <summary>
    /// Search End Date
    /// </summary>
    public DateTime EndDate { get; set; }
}