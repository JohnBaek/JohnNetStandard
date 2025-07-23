namespace JohnIsDev.Core.Models.Common.Enums;

/// <summary>
/// 쿼리 SearchType 
/// </summary>
public enum EnumQuerySearchType
{
    /// <summary>
    /// Contains Search
    /// </summary>
    Equals ,
     
    /// <summary>
    /// 스트링 Like 검색
    /// </summary>
    Like ,
    
    /// <summary>
    /// Greater Then
    /// </summary>
    GreaterThen ,
    
    /// <summary>
    /// Greater Then
    /// </summary>
    LessThen ,
    
    /// <summary>
    /// Equals Numeric
    /// </summary>
    EqualsNumeric ,
    NumericOrEnums,
        
    /// <summary>
    /// Range Date
    /// </summary>
    StartDate ,
    EndDate ,
    RangeDate
}