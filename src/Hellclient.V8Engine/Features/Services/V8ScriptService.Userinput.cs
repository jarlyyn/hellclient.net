using System.Dynamic;
using System.Text;
using Hellclient.V8Engine.Features.States;
using Hellclient.V8Engine.Infras.Components.JsUserinput;
using Hellclient.World.Types;

namespace Hellclient.V8Engine.Features.Services;



public partial class V8ScriptService
{
    public void initUserinput(V8EngineContext context)
    {
        using var m = new JsUserinput(context.World, context.Runtime).Convert();
        ((IDictionary<string, object>)context.Runtime.Script)["Userinput"] = m!;
    }
}