using Hellclient.World.States;
using Hellclient.World.Types;

namespace Hellclient.World.Features.Services;


public interface IQueueService
{
    public void InstallTo(WorldContext context);
    public void Append(WorldContext context, Command cmd);
    public int Flush(WorldContext context, bool force);
    public void LockQueue(WorldContext context);
    public List<Command> ListQueue(WorldContext context);
}

public class QueueService : IQueueService
{
    public void InstallTo(WorldContext context)
    {
        context.EventBus.QueueDelayUpdatedEvent += (sender, args) =>
        {
            Task.Run(() => _onDelayUpdate(context));
        };
    }
    public MetronomeService MetronomeService { get; set; } = new MetronomeService();

    public void LockQueue(WorldContext context)
    {
        foreach (var cmd in context.Queue.List)
        {
            cmd.Locked = true;
        }
    }
    public List<Command> ListQueue(WorldContext context)
    {
        return context.Queue.List;
    }
    public int Flush(WorldContext context, bool force)
    {
        context.Queue.List.Clear();
        int l = 0;
        if (force)
        {
            l = context.Queue.List.Count;
            context.Queue.List.Clear();
        }
        else
        {
            var result = new List<Command>();
            foreach (var cmd in context.Queue.List)
            {
                if (cmd.Locked)
                {
                    result.Add(cmd);
                }
                else
                {
                    l++;
                }
            }
            context.Queue.List = result;
        }
        if (context.Queue.List.Count == 0)
        {
            context.Queue.Pending = false;
        }
        return l;
    }
    public void Append(WorldContext context, Command cmd)
    {
        var cmds = cmd.Split("\n");
        foreach (var c in cmds)
        {
            context.Queue.List.Add(cmd);
        }
        if (!context.Queue.Pending)
        {
            delay(context);
        }
    }
    private void delay(WorldContext context)
    {
        var delay = context.Config.Data.QueueDelay;
        if (delay > 0)
        {
            context.Queue.Pending = true;
            context.Queue.StartTimer(TimeSpan.FromMilliseconds(delay), () =>
            {
                context.Lock.Wait();
                try
                {

                    AfterDelay(context);
                }
                finally
                {
                    context.Lock.Release();
                }
            });
        }
        else
        {
            send(context);
        }

    }
    private void send(WorldContext context)
    {
        if (context.Queue.List.Count != 0)
        {
            var cmd = context.Queue.List[0];
            context.Queue.List.RemoveAt(0);
            MetronomeService.Send(context, cmd);
            if (context.Queue.List.Count != 0)
            {
                delay(context);
            }
        }
    }
    private void _onDelayUpdate(WorldContext context)
    {
        context.Lock.Wait();
        try
        {
            context.Queue.Pending = false;
            context.Queue.StopTimer();
            delay(context);
        }
        finally
        {
            context.Lock.Release();
        }
    }
    public void AfterDelay(WorldContext context)
    {
        send(context);
        if (context.Queue.List.Count == 0)
        {
            context.Queue.StopTimer();
        }
    }
}