namespace JohnIsDev.Core.Features.Debounce;

/// <summary>
/// Common thread Debounce class
/// </summary>
// ReSharper disable once IdentifierTypo
public class Debouncer
{
    /// <summary>
    /// Thread 타이머 
    /// </summary>
    private Timer? _timer;
    
    /// <summary>
    /// 실행 대리자
    /// </summary>
    private readonly Action _action;

    /// <summary>
    /// 생성자
    /// </summary>
    /// <param name="action"></param>
    // ReSharper disable once IdentifierTypo
    public Debouncer(Action action)
    {
        _action = action;
    }


    /// <summary>
    /// 디바운서를 실행한다.
    /// </summary>
    /// <param name="debouncePeriod"></param>
    public void Trigger(TimeSpan debouncePeriod)
    {
        // 실행시간이 완료되기전에 들어오면 해제한다.
        _timer?.Dispose();
        
        // 타이머를 시작한다.
        _timer = new Timer(_ => _action(), null, debouncePeriod, Timeout.InfiniteTimeSpan);
    }
}