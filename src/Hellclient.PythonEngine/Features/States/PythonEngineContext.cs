

using Hellclient.PythonEngine.Infras.Components;
using Hellclient.Script.Infras.Components.API;
using Hellclient.Script.Types;
using Hellclient.World.Cores;
using Python.Runtime;

namespace Hellclient.PythonEngine.Features.States;

public class PythonEngineContext
{
    public PythonEngineContext(IWorld world)
    {
        World = world;
        PyAPI = new PyAPI(new ScriptAPI(world), Scope);
    }
    public IWorld World { get; init; }
    public PyModule Scope { get; set; } = Py.CreateScope();
    public PyAPI PyAPI { get; init; }
    public ScriptEvents Events { get; set; } = new ScriptEvents();

}