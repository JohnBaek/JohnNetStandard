using Microsoft.Extensions.Logging;

namespace JohnIsDev.Core.Features.Extensions;

/// <summary>
/// 예외처리 확장
/// </summary>
public static class ExceptionExtensions
{
    /// <summary>
    /// 에러 로그를 기록한다.
    /// </summary>
    /// <param name="exception"></param>
    /// <param name="logger"></param>
    public static void LogError(this Exception exception, ILogger logger)
    {
        logger.LogError(exception, exception.Message);
    }
    
    /// <summary>
    /// Wanring 로그를 기록한다.
    /// </summary>
    /// <param name="exception"></param>
    /// <param name="logger"></param>
    public static void LogWarning(this Exception exception, ILogger logger)
    {
        logger.LogWarning(exception, exception.Message);
    }
    
    /// <summary>
    /// Information 로그를 기록한다.
    /// </summary>
    /// <param name="exception"></param>
    /// <param name="logger"></param>
    public static void LogInformation(this Exception exception, ILogger logger)
    {
        logger.LogInformation(exception, exception.Message);
    }
}