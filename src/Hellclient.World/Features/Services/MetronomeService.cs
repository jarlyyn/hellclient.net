using Hellclient.World.States;
using Hellclient.World.Types;

namespace Hellclient.World.Features.Services;

public interface IMetronomeService
{
    public void SetInterval(WorldContext context, TimeSpan interval);
    public TimeSpan Interval(WorldContext context);
    public void Reset(WorldContext context);
    public int Beats(WorldContext context);
    public int SetBeats(WorldContext context, int beats);
    public void SetTick(WorldContext context, TimeSpan tick);
    public TimeSpan Tick(WorldContext context);
    public int Space(WorldContext context);
    public List<string> Queue(WorldContext context);
    public bool Discard(WorldContext context, bool force);
    public void LockQueue(WorldContext context);
    public void FullTick(WorldContext context);
    public void Full(WorldContext context);
    public void Send(WorldContext context, Command cmd);
    public Task Push(WorldContext context, List<Command> cmds, bool grouped);
    public void InstallTo(WorldContext context);
}
public class MetronomeService : IMetronomeService
{
    public IConnService ConnService { get; set; } = new ConnService();
    public void SetInterval(WorldContext context, TimeSpan interval)
    {
        context.Metronome.Interval = interval;
    }
    public TimeSpan Interval(WorldContext context)
    {
        return context.Metronome.Interval;
    }
    public int Beats(WorldContext context)
    {
        return getBeats(context);
    }
    public int SetBeats(WorldContext context, int beats)
    {
        context.Metronome.Beats = beats;
        return getBeats(context);
    }
    private int getBeats(WorldContext context)
    {
        var b = context.Metronome.Beats;
        if (b > 0)
        {
            return b;
        }
        return 1;
    }
    public void SetTick(WorldContext context, TimeSpan tick)
    {
        context.Metronome.Tick = tick;
    }
    private TimeSpan getTick(WorldContext context)
    {
        var t = context.Metronome.Tick;
        if (t <= TimeSpan.Zero)
        {
            return Metronome.DefaultCheckInterval;
        }
        return t;
    }
    public TimeSpan Tick(WorldContext context)
    {
        return context.Metronome.Tick;
    }
    public void clean(WorldContext context)
    {
        var t = DateTime.Now;

        context.Metronome.Sent.RemoveAll(e =>
        {
            if (t.Subtract(e) > getTick(context))
            {
                return true;
            }
            return false;
        });
    }
    public int Space(WorldContext context)
    {
        clean(context);
        var b = getBeats(context);
        var s = b - context.Metronome.Sent.Count;
        if (s < 0)
        {
            return 0;
        }
        return s;
    }
    public List<string> Queue(WorldContext context)
    {
        var result = new List<string>();
        context.Metronome.Queue.ForEach(cmds =>
        {
            cmds.ForEach(c => result.Add(c.Message));
        });
        return result;
    }
    public bool discard(WorldContext context, bool force)
    {
        var result = context.Metronome.Queue.Count;
        var q = new List<List<Command>>();
        if (!force)
        {
            context.Metronome.Queue.ForEach(cmds =>
            {
                var c = new List<Command>();
                cmds.ForEach(v =>
                {
                    if (!v.Locked)
                    {
                        c.Add(v);
                    }
                });
                if (c.Count > 0)
                {
                    q.Add(c);
                }
            });
        }
        context.Metronome.Queue = q;
        return result != q.Count;
    }
    public bool Discard(WorldContext context, bool force)
    {
        return discard(context, force);
    }
    public void LockQueue(WorldContext context)
    {
        context.Metronome.Queue.ForEach(cmds => cmds.ForEach(c => c.Locked = true));
    }
    public void fullTick(WorldContext context)
    {
        var t = DateTime.Now;
        var b = getBeats(context);
        for (var i = context.Metronome.Sent.Count; i < b; i++)
        {
            context.Metronome.Sent.Add(t);
        }
    }
    public void FullTick(WorldContext context)
    {
        fullTick(context);
    }
    public void full(WorldContext context)
    {
        var t = DateTime.Now;
        var b = getBeats(context);
        for (var i = 0; i < b; i++)
        {
            context.Metronome.Sent.Add(t);
        }
    }
    public void Full(WorldContext context)
    {
        full(context);
    }
    public void Send(WorldContext context, Command cmd)
    {
        cmd.Split("\n").ToList().ForEach(c =>
        {
            ConnService.DoSend(context, c);
            var t = DateTime.Now;
            context.Metronome.Sent.Add(t);
        });
    }
    public void append(WorldContext context, List<Command> rawcmds, bool grouped)
    {
        var cmds = new List<Command>();
        rawcmds.ForEach(c =>
        {
            cmds.AddRange(c.Split("\n"));
        });
        if (grouped)
        {
            context.Metronome.Queue.Add(cmds);
        }
        else
        {
            cmds.ForEach(c =>
            {
                context.Metronome.Queue.Add([c]);
            });
        }
    }
    public async Task play(WorldContext context)
    {
        if (!context.Connection.IsConnected())
        {
            return;
        }
        clean(context);
        var b = getBeats(context);
        while (context.Metronome.Queue.Count != 0 && context.Metronome.Sent.Count < b)
        {
            var cmds = context.Metronome.Queue[0];
            if (b - context.Metronome.Sent.Count < cmds.Count)
            {
                //避免cmds长于beats时永远不发送
                if (context.Metronome.Sent.Count() != 0)
                {
                    return;
                }
            }
            context.Metronome.Queue.RemoveAt(0);
            foreach (var cmd in cmds)
            {
                await ConnService.DoSend(context, cmd);
                var t = DateTime.Now;
                context.Metronome.Sent.Add(t);
            }
        }
    }
    public async Task Play(WorldContext context)
    {
        _ = play(context);
    }
    public void stopTick(WorldContext context)
    {
        if (context.Metronome.ticker != null)
        {
            context.Metronome.ticker.Dispose();
        }
        context.Metronome.ticker = null;
    }
    public void startTick(WorldContext context)
    {
        stopTick(context);
        var interval = context.Metronome.Interval;
        if (interval <= TimeSpan.Zero)
        {
            interval = Metronome.DefaultInterval;
        }
        context.Metronome.ticker = new PeriodicTimer(interval);
        Task.Run(async () => await nextTick(context));
    }
    public async Task nextTick(WorldContext context)
    {
        if (context.Metronome.ticker == null)
        {
            return;
        }
        if (await context.Metronome.ticker.WaitForNextTickAsync())
        {
            await play(context);
        }
    }
    public async Task Push(WorldContext context, List<Command> cmds, bool grouped)
    {
        append(context, cmds, grouped);
        _ = play(context);
    }
    public void InstallTo(WorldContext context)
    {
        SetInterval(context, Metronome.DefaultInterval);
        context.EventBus.CloseEvent += async (sender, args) =>
        {
            stopTick(context);
        };
    }
    public void Reset(WorldContext context)
    {
        context.Metronome.Sent.Clear();
        Task.Run(async () => await nextTick(context));
    }
}

