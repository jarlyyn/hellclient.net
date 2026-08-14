using Hellclient.V8Engine.Infras.Components;
using Hellclient.V8Engine.Types;
using Hellclient.World.Cores;
using Microsoft.ClearScript.V8;




namespace Hellclient.V8Engine.Features.States;

public class V8EngineContext
{
    public V8EngineContext(IWorld world)
    {
        World = world;
        JsAPI = new JsAPI(new ScriptAPI(world));
        
    }
    public Microsoft.ClearScript.V8.V8ScriptEngine Engine { get; set; } = new Microsoft.ClearScript.V8.V8ScriptEngine();
    public JsAPI JsAPI { get; init; }
    public IWorld World { get; init; }
    public V8ScriptEngine Runtime { get; set; } = new V8ScriptEngine();

    public ScriptEvents Events { get; set; } = new ScriptEvents();
}