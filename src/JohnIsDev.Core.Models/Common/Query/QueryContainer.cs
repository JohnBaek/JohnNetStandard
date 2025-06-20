namespace JohnIsDev.Core.Models.Common.Query;

/// <summary>
/// 쿼리 컨테이너 
/// </summary>
public class QueryContainer<T> 
    where T : class 
{
    /// <summary>
    /// 생성자
    /// </summary>
    /// <param name="totalCount">전체 갯수</param>
    /// <param name="queryable">쿼리</param>
    /// <param name="requestQuery">쿼리정보</param>
    public QueryContainer(int totalCount, IQueryable<T>? queryable, RequestQuery requestQuery)
    {
        TotalCount = totalCount;
        Queryable = queryable;
        Skip = requestQuery.Skip;
    }

    /// <summary>
    /// 스킵
    /// </summary>
    public int Skip { get; set; } = 0;

    /// <summary>
    /// 페이지 카운트 
    /// </summary>
    public int PageCount { get; set; } = 20;
    
    /// <summary>
    /// 전체 수
    /// </summary>
    public int TotalCount { get; set; } = 0;

    /// <summary>
    /// 응답 데이터 쿼리
    /// </summary>
    public IQueryable<T>? Queryable { get; set; }
}