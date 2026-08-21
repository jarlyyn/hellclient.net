using Hellclient.V8Engine.Features.States;
using Hellclient.V8Engine.Infras.Components;

namespace Hellclient.V8Engine.Features.Services;

public partial class V8ScriptService
{
    private void initHTTP(V8EngineContext context)
    {
        using var m = new JsHttp(context.World, context.Runtime).Convert();
        ((IDictionary<string, object>)context.Runtime.Script)["HTTP"] = m;
    }
}