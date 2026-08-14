using Hellclient.World.Cores;
using Hellclient.World.Types;

namespace Hellclient.V8Engine.Infras.Components;

public class JsMetronome(IWorld world)
{
    private readonly IWorld _world = world;

    public object? GetBeats(params object[] args)
    {
        return _world.GetMetronomeBeats();
    }

    public object? SetBeats(params object[] args)
    {
        _world.SetMetronomeBeats(JsAPI.GetIntArg(args, 0));
        return null;
    }

    public object? Reset(params object[] args)
    {
        _world.DoResetMetronome();
        return null;
    }

    public object? GetSpace(params object[] args)
    {
        return _world.GetMetronomeSpace();
    }

    public object? GetQueue(params object[] args)
    {
        return _world.GetMetronomeQueue();
    }

    public object? Discard(params object[] args)
    {
        _world.DoDiscardMetronome(JsAPI.GetBoolArg(args, 0));
        return null;
    }

    public object? LockQueue(params object[] args)
    {
        _world.DoLockMetronomeQueue();
        return null;
    }
    public object? full(params object[] args)
    {
        _world.DoFullMetronome();
        return null;
    }


    public object? FullTick(params object[] args)
    {
        _world.DoFullTickMetronome();
        return null;
    }

    public object? GetInterval(params object[] args)
    {
        return _world.GetMetronomeInterval().Milliseconds;
    }

    public object? SetInterval(params object[] args)
    {
        _world.SetMetronomeInterval(TimeSpan.FromMilliseconds(JsAPI.GetIntArg(args, 0)));
        return null;
    }

    public object? GetTick(params object[] args)
    {
        return _world.GetMetronomeTick().Milliseconds;
    }

    public object? SetTick(params object[] args)
    {
        _world.SetMetronomeTick(TimeSpan.FromMilliseconds(JsAPI.GetIntArg(args, 0)));
        return null;
    }
    public object? Push(params object[] args)
    {
        var pushArgs = JsAPI.GetStringArrayArg(args, 0);
        var grouped = JsAPI.GetBoolArg(args, 1);
        var echo = JsAPI.GetBoolArg(args, 2);
        var cmds = new List<Command>();
        foreach (var arg in pushArgs)
        {
            var c = new Command();
            c.Message = arg;
            c.Echo = echo;
            cmds.Add(c);
        }
        _world.DoPushMetronome(cmds, grouped);
        return null;
    }
}