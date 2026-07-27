using Hellclient.World.Types;

namespace Hellclient.World.Infras.Components;

public class Queue
{
    public bool Pending { get; set; } = false;
    public List<Command> List { get; set; } = new();

    public PeriodicTimer? Timer { get; set; }

    public void StartTimer(TimeSpan interval,Action callback)
    {
        if (Timer == null)
        {
            Timer = new PeriodicTimer(interval);
            Task.Run(async () =>
            {
                while (await Timer.WaitForNextTickAsync())
                {
                    callback();
                }
            });
        }
    }
    public void StopTimer()
    {
        if (Timer != null)
        {
            Timer.Dispose();
            Timer = null;
        }
    }

}