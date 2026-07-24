namespace Hellclient.World.Infras.Components;

public class DebounceTimer
{
    public DebounceTimer(double interval)
    {
        _timer = new System.Timers.Timer(interval)
        {
            AutoReset = false
        };
    }
    public void Reset(double interval)
    {
        lock (_lock)
        {
            if (_timer is not null)
            {
                _timer.Interval = interval;
                _timer.Stop();
                _timer.Start();
            }
        }
    }
    public void Discard()
    {
        lock (_lock)
        {
            if (_timer is null)
            {
                return;
            }
            _timer = null;
            _timer?.Dispose();
        }
    }
    public void Bind(Func<DebounceTimer, Task> callback)
    {
        lock (_lock)
        {
            if (_timer is null)
            {
                return;
            }
            _timer.Elapsed += async (sender, e) =>
            {
                await callback(this);
            };
        }
    }
    private System.Timers.Timer? _timer;
    private object _lock = new();

}
public class Debounce

{
    public Debounce(TimeSpan duration, Action callback)
    {
        Duration = duration;
        Callback = callback;
    }
    //Duration debounce duration
    TimeSpan Duration { get; set; }
    //MaxDuration max lifetime debouce can live.
    TimeSpan MaxDuration { get; set; }

    private DateTime? _deadLine;
    //Leading if the callback should be called before the duration
    public bool Leading { get; set; }

    private DebounceTimer? _timer;
    public Action? Callback { get; set; }
    public static TimeSpan MinTimeSpan { get; } = TimeSpan.FromMilliseconds(1);

    public bool Reset()
    {
        if (_timer is null)
        {
            return false;
        }
        if (MaxDuration > TimeSpan.Zero)
        {
            _deadLine = DateTime.Now.Add(MaxDuration);
        }
        else
        {
            _deadLine = DateTime.Now;
        }
        _timer.Reset((Duration > MinTimeSpan ? Duration : MinTimeSpan).TotalMilliseconds);
        return true;
    }
    public async Task<bool> Exec()
    {

        var success = false;
        TimeSpan duration;
        if (_timer is not null)
        {
            if (_deadLine is not null)
            {
                duration = _deadLine.Value - DateTime.Now;
                if (duration > Duration)
                {
                    duration = Duration;
                }
                if (duration <= MinTimeSpan)
                {
                    duration = MinTimeSpan;
                }
            }
            else
            {
                duration = Duration;
            }
            _timer.Reset(duration.TotalMilliseconds);
            success = true;
        }
        if (success)
        {
            return true;
        }
        if (_timer is not null)
        {
            return false;
        }
        _timer = new DebounceTimer(Duration.TotalMilliseconds);

        if (MaxDuration > TimeSpan.Zero)
        {
            _deadLine = DateTime.Now.Add(MaxDuration);
        }
        else
        {
            _deadLine = DateTime.Now;
        }

        if (Leading)
        {
            Callback?.Invoke();
        }
        _timer.Bind(run);
        return Leading;
    }
    private async Task run(DebounceTimer timer)
    {
        timer.Discard();
        Callback?.Invoke();

    }
    public void Discard()
    {
        if (_timer is null)
        {
            return;
        }
        _timer?.Discard();
        _timer = null;
    }
}