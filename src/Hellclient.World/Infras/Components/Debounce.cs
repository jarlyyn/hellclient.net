namespace Hellclient.World.Infras.Components;


public class Debounce

{
    public Debounce(TimeSpan duration, Action callback)
    {
        Duration = duration;
        Callback = callback;
    }
    CancellationTokenSource? cts = null;

    public TimeSpan Duration { get; set; }
    public Action Callback { get; set; }
    private readonly object _lockObj = new object();
    public void Exec()
    {
        lock (_lockObj)
        {
            cts?.Cancel();
            cts = new CancellationTokenSource();
            Task.Delay(Duration, cts.Token).ContinueWith(task =>
            {
                if (!task.IsCanceled)
                {
                    Callback?.Invoke();
                }
            }, TaskScheduler.Default);
        }
    }
    public void Discard()
    {
        lock (_lockObj)
        {
            cts?.Cancel();
            cts = null;
        }
    }
}
