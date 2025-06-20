using System.ComponentModel.DataAnnotations;

namespace JohnIsDev.Core.Models.Requests;

/// <summary>
/// 최상위 요청 모델 클래스 모델
/// </summary>
public class RequestBase
{
    /// <summary>
    /// 유효성 검사 모델
    /// </summary>
    private readonly List<ValidationResult> _validationResults = new ();

    /// <summary>
    /// 유효성 여부
    /// </summary>
    /// <returns></returns>
    public bool IsInValid()
    {
        var validationContext = new ValidationContext(this);
        _validationResults.Clear();
        bool isValid = Validator.TryValidateObject(this, validationContext, _validationResults, true);
        return !isValid;
    }

    /// <summary>
    /// 첫번째 에러 메세지를 가져온다.
    /// </summary>
    public string GetFirstErrorMessage()
    {
        // 유효한 에러메세지가 없는 경우 
        if (_validationResults.Count == 0)
            return "";

        return _validationResults[0].ErrorMessage ?? "";
    }
}