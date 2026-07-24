namespace Hellclient.World.Infras.Components;

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

    private System.Timers.Timer? _timer;
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
        _timer.Interval = (Duration > MinTimeSpan ? Duration : MinTimeSpan).TotalMilliseconds;
        _timer.Stop();
        _timer.Start();
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
            _timer.Interval = duration.TotalMilliseconds;
            success = true;
        }
        if (success)
        {
            return true;
        }
        if (_timer is not null)
        {
            Reset();
            return false;
        }
        _timer = new System.Timers.Timer(Duration.TotalMilliseconds)
        {
            AutoReset = false
        };
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
        _timer.Elapsed += async (sender, e) => await run(sender as System.Timers.Timer);
        return Leading;
    }
    private async Task run(System.Timers.Timer? timer)
    {
        if (timer is null)
        {
            return;
        }
        timer.Dispose();
        Callback?.Invoke();

    }
    public void Discard()
    {
        _timer?.Dispose();
        _timer = null;
    }
}