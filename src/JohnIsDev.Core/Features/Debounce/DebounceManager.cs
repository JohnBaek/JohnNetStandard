namespace JohnIsDev.Core.Features.Debounce;

/// <summary>
/// 전역 Debounce 상태 관리자
/// </summary>
public class DebounceManager
{
    /// <summary>
    /// 소유한 디바운서 
    /// </summary>
    // ReSharper disable once IdentifierTypo
    private readonly Dictionary<string, Debouncer> _debouncers = new();

    /// <summary>
    /// Locking 용 임시 오브젝트 
    /// </summary>
    private readonly object _lock = new();

    /// <summary>
    /// 디바운서를 세팅한다.
    /// </summary>
    /// <param name="key"></param>
    /// <param name="debouncer"></param>
    public void SetDebouncer(string key, Debouncer debouncer)
    {
        // 락이 되어있지 않다면 
        lock (_lock)
        {
            // 디바운서 추가
            _debouncers[key] = debouncer;
        }
    }

    /// <summary>
    /// 디바운서를 가져온다.
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    // ReSharper disable once IdentifierTypo
    public Debouncer GetDebouncer(string key)
    {
        // 락이 되어있지 않다면 
        lock (_lock)
        {
            return _debouncers.GetValueOrDefault(key);
        }
    }

    /// <summary>
    /// 디바운서를 동작한다.
    /// </summary>
    /// <param name="key"></param>
    /// <param name="debouncePeriod"></param>
    /// <param name="action"></param>
    public void TriggerDebouncer(string key, TimeSpan debouncePeriod, Action action)
    {
        // 락이 되어있지 않다면 
        lock (_lock)
        {
            // 등록된 디바운서가 없으면
            if (!_debouncers.ContainsKey(key))
            {
                _debouncers[key] = new Debouncer(action);
            }

            // 정해진 시간만큼 동작한다.
            _debouncers[key].Trigger(debouncePeriod);
        }
    }
}