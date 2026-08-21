namespace Hellclient.World.Infras.Components;

public class DebounceTimer
{
    public DebounceTimer(TimeSpan interval)
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
    public void Start()
    {
        if (_timer is not null)
        {
            _timer.Start();
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
            _timer.Stop();
            _timer?.Dispose();
            _timer = null;
        }
    }
    public void Bind(Action<DebounceTimer> callback)
    {
        lock (_lock)
        {
            if (_timer is null)
            {
                return;
            }
            _timer.Elapsed += (sender, e) =>
            {
                Task.Run(() => callback(this));
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
        MaxDuration = TimeSpan.Zero;
        Callback = callback;
    }
    //Duration debounce duration
    TimeSpan Duration { get; set; }
    //MaxDuration max lifetime debouce can live.
    TimeSpan MaxDuration { get; set; }

    private DateTime? _deadLine = null;
    //Leading if the callback should be called before the duration
    public bool Leading { get; set; }

    private DebounceTimer? _timer;
    public Action? Callback { get; set; }
    public static TimeSpan MinTimeSpan { get; } = TimeSpan.FromMilliseconds(1);

    public bool Reset()
    {
        lock (this)
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
                _deadLine = null;
            }
            _timer.Reset((Duration > MinTimeSpan ? Duration : MinTimeSpan).TotalMilliseconds);
            return true;
        }
    }
    public bool Exec()
    {
        lock (this)
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
                }
                else
                {
                    duration = Duration;
                }
                if (duration >= MinTimeSpan)
                {
                    _timer.Reset(duration.TotalMilliseconds);
                }
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
            _timer = new DebounceTimer(Duration);

            if (MaxDuration > TimeSpan.Zero)
            {
                _deadLine = DateTime.Now.Add(MaxDuration);
            }
            else
            {
                _deadLine = null;
            }

            if (Leading)
            {
                Callback?.Invoke();
            }
            _timer.Bind(run);
            _timer.Start();
            return Leading;
        }
    }
    private void run(DebounceTimer timer)
    {
        lock (this)
        {
            if (_timer is not null)
            {
                _timer?.Discard();
                _timer = null;
            }
            timer.Discard();
            if (!Leading)
            {
                Callback?.Invoke();
            }
        }
    }
    public void Discard()
    {
        lock (this)
        {

            if (_timer is null)
            {
                return;
            }
            _timer?.Discard();
            _timer = null;
        }
    }
}