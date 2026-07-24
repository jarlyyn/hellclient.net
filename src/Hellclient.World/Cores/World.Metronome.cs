

using Hellclient.World.Types;

namespace Hellclient.World.Cores;


public partial class World
{
    public int GetMetronomeBeats() => Service.MetronomeService.Beats(Context);
    public void SetMetronomeBeats(int beats) => Service.MetronomeService.SetBeats(Context, beats);
    public void DoResetMetronome() => Service.MetronomeService.Reset(Context);
    public int GetMetronomeSpace() => Service.MetronomeService.Space(Context);
    public List<string> GetMetronomeQueue() => Service.MetronomeService.Queue(Context);
    public bool DoDiscardMetronome(bool force) => Service.MetronomeService.Discard(Context, force);
    public void DoLockMetronomeQueue() => Service.MetronomeService.LockQueue(Context);
    public void DoFullMetronome() => Service.MetronomeService.Full(Context);
    public void DoFullTickMetronome() => Service.MetronomeService.FullTick(Context);
    public void SetMetronomeInterval(TimeSpan interval) => Service.MetronomeService.SetInterval(Context, interval);
    public TimeSpan GetMetronomeInterval() => Service.MetronomeService.Interval(Context);
    public void SetMetronomeTick(TimeSpan tick) => Service.MetronomeService.SetTick(Context, tick);
    public TimeSpan GetMetronomeTick() => Service.MetronomeService.Tick(Context);
    public void DoPushMetronome(List<Command> cmds, bool grouped) => Service.MetronomeService.Push(Context, cmds, grouped);
    public void DoMetronomeSend(Command cmds) => Service.MetronomeService.Send(Context, cmds);
    public void DoMetronomeLock() => Service.MetronomeService.LockQueue(Context);
}