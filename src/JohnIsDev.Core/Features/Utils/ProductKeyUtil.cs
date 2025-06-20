namespace JohnIsDev.Core.Features.Utils;

/// <summary>
/// 
/// </summary>
public static class ProductKeyUtil
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="cateogryKey"></param>
    /// <param name="productSequnce"></param>
    /// <returns></returns>
    public static string GetProductKey(string cateogryKey, int productSequnce)
    {
        if (string.IsNullOrWhiteSpace(cateogryKey) || cateogryKey.Length != 3)
        {
            throw new ArgumentException("카테고리 키는 3자리 문자열이어야 합니다.");
        }
        
        // 현재 날짜 가져오기 (yyyyMMdd 형식)
        string datePart = DateTime.Now.ToString("yyMMdd");
        
        // 시퀀스를 12자리로 포맷팅
        string sequenceString = productSequnce.ToString("D12");
        
        // 최종 제품 키 조합
        return $"{cateogryKey}-{datePart}-{sequenceString}";
    }
}