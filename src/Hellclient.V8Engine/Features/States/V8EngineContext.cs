using Hellclient.Script.Infras.Components.API;
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
        JsAPI = new JsAPI(new ScriptAPI(world), Runtime);
        JsMetronome = new JsMetronome(world, Runtime);

    }
    public PeriodicTimer Timer = new PeriodicTimer(TimeSpan.FromSeconds(10));
    public JsAPI JsAPI { get; init; }
    public JsMetronome JsMetronome { get; init; }
    public IWorld World { get; init; }
    public V8ScriptEngine Runtime { get; set; } = new V8ScriptEngine();

    public ScriptEvents Events { get; set; } = new ScriptEvents();
}