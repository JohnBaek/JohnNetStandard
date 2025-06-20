namespace JohnIsDev.Core.Models.Responses;

/// <summary>
/// 응답 기본 설정 데이터
/// </summary>
public class ResponseCommonWriter 
{
    /// <summary>
    /// 등록일 (필수)
    /// </summary>
    public DateTime? RegDate { get; set; }
    
    /// <summary>
    /// 수정일 (필수)
    /// </summary>
    public DateTime? ModDate { get; set; }

    /// <summary>
    /// 등록자 아이디 
    /// </summary>
    public Guid? RegId { get; set; }
    
    /// <summary>
    /// 수정자 아이디
    /// </summary>
    public Guid? ModId { get; set; }
    
    /// <summary>
    /// 등록자명 
    /// </summary>
    public string? RegName { get; set;}

    /// <summary>
    /// 수정자명 
    /// </summary>
    public string? ModName { get; set;}
}