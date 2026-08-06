using Hellclient.World.Cores;
using Microsoft.ClearScript.V8;




namespace Hellclient.V8ScriptEngine.Features.States;

public class V8EngineContext
{
    public V8EngineContext(IWorld world)
    {
        World = world;
    }
    public Microsoft.ClearScript.V8.V8ScriptEngine Engine { get; set; } = new Microsoft.ClearScript.V8.V8ScriptEngine();
    public IWorld World { get; init; }
}