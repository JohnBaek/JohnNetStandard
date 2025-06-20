namespace JohnIsDev.Core.Models.Common.Query;

/// <summary>
/// 쿼리 오더 정보
/// </summary>
public class QuerySortOrder
{
    /// <summary>
    /// 필드정보 
    /// </summary>
    public required string Field { get; set; }

    /// <summary>
    /// 오더정보
    /// </summary>
    public required EnumQuerySortOrder Order { get; set; }
}

/// <summary>
/// 소팅 오더 정보 
/// </summary>
public enum EnumQuerySortOrder
{
    /// <summary>
    /// Ascending 
    /// </summary>
    Asc ,
    /// <summary>
    /// Descending
    /// </summary>
    Desc ,
}