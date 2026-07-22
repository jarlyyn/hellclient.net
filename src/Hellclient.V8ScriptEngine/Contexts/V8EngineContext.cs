using Hellclient.World.Cores;

namespace Hellclient.V8ScriptEngine.Context;

public class V8EngineContext
{
    public V8EngineContext(IWorld world)
    {
        World = world;
    }
    public IWorld World { get; init; }
}   