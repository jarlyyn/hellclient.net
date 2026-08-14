using System.Dynamic;
using Hellclient.V8Engine.Features.States;
using Hellclient.V8Engine.Infras.Components;
using Hellclient.World.Types;

namespace Hellclient.V8Engine.Features.Services;

public partial class V8ScriptService
{
    public void initMetronome(V8EngineContext context)
    {
        var met=context.JsMetronome;
        var m = new ExpandoObject() as IDictionary<string, object>;
        m["getbeats"]=met.GetBeats;
        m["setbeats"]=met.SetBeats;
        m["reset"]=met.Reset;
        m["getspace"]=met.GetSpace;
        m["getqueue"]=met.GetQueue;
        m["discard"]=met.Discard;
        m["lockqueue"]=met.LockQueue;
        m["full"]=met.full;
        m["fulltick"]=met.FullTick;
        m["getinterval"]=met.GetInterval;
        m["setinterval"]=met.SetInterval;
        m["gettick"]=met.GetTick;
        m["settick"]=met.SetTick;
        m["push"]=met.Push;
        m["GetBeats"]=met.GetBeats;
        m["SetBeats"]=met.SetBeats;
        m["Reset"]=met.Reset;
        m["GetSpace"]=met.GetSpace;
        m["GetQueue"]=met.GetQueue;
        m["Discard"]=met.Discard;
        m["LockQueue"]=met.LockQueue;
        m["Full"]=met.full;
        m["FullTick"]=met.FullTick;
        m["GetInterval"]=met.GetInterval;
        m["SetInterval"]=met.SetInterval;
        m["GetTick"]=met.GetTick;
        m["SetTick"]=met.SetTick;
        m["Push"]=met.Push;
        context.Runtime.AddHostObject("Metronome", m);

    }
}