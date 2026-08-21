using System.Dynamic;
using Hellclient.V8Engine.Features.States;
using Hellclient.V8Engine.Infras.Components;
using Hellclient.World.Types;

namespace Hellclient.V8Engine.Features.Services;

public partial class V8ScriptService
{
    public void initMetronome(V8EngineContext context)
    {
        using var m = new JsMetronome(context.World, context.Runtime).Convert();
        ((IDictionary<string, object>)context.Runtime.Script)["Metronome"] = m;
    }
}