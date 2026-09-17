using Hellclient.World.Cores;
using Hellclient.World.Types;
using Python.Runtime;

namespace Hellclient.PythonEngine.Infras.Components;

public class PyMetronome(IWorld world)
{
    private readonly IWorld _world = world;

    public PyObject? GetBeats(params PyObject[] args)
    {
        using var _ = Py.GIL();        
        return _world.GetMetronomeBeats().ToPython();
    }

    public PyObject? SetBeats(params PyObject[] args)
    {
        using var _ = Py.GIL();
        _world.SetMetronomeBeats(PyAPI.GetIntArg(args, 0));
        return null;
    }

    public PyObject? Reset(params PyObject[] args)
    {
        using var _ = Py.GIL();
        _world.DoResetMetronome();
        return null;
    }

    public PyObject? GetSpace(params PyObject[] args)
    {
        using var _ = Py.GIL();
        return _world.GetMetronomeSpace().ToPython();
    }

    public PyObject? GetQueue(params PyObject[] args)
    {
        using var _ = Py.GIL();
        return _world.GetMetronomeQueue().ToPython();
    }

    public PyObject? Discard(params PyObject[] args)
    {
        using var _ = Py.GIL();
        _world.DoDiscardMetronome(PyAPI.GetBoolArg(args, 0));
        return null;
    }

    public PyObject? LockQueue(params PyObject[] args)
    {
        using var _ = Py.GIL();
        _world.DoLockMetronomeQueue();
        return null;
    }
    public PyObject? full(params PyObject[] args)
    {
        using var _ = Py.GIL();
        _world.DoFullMetronome();
        return null;
    }


    public PyObject? FullTick(params PyObject[] args)
    {
        using var _ = Py.GIL();
        _world.DoFullTickMetronome();
        return null;
    }

    public PyObject? GetInterval(params PyObject[] args)
    {
        using var _ = Py.GIL();
        return _world.GetMetronomeInterval().Milliseconds.ToPython();
    }

    public PyObject? SetInterval(params PyObject[] args)
    {
        using var _ = Py.GIL();
        _world.SetMetronomeInterval(TimeSpan.FromMilliseconds(PyAPI.GetIntArg(args, 0)));
        return null;
    }

    public PyObject? GetTick(params PyObject[] args)
    {
        using var _ = Py.GIL();
        return _world.GetMetronomeTick().Milliseconds.ToPython();
    }

    public PyObject? SetTick(params PyObject[] args)
    {
        using var _ = Py.GIL();
        _world.SetMetronomeTick(TimeSpan.FromMilliseconds(PyAPI.GetIntArg(args, 0)));
        return null;
    }
    public PyObject? Push(params PyObject[] args)
    {
        using var _ = Py.GIL();
        var pushArgs = PyAPI.GetStringArrayArg(args, 0);
        var grouped = PyAPI.GetBoolArg(args, 1);
        var echo = PyAPI.GetBoolArg(args, 2);
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