using Hellclient.V8Engine.Features.States;
using Hellclient.World.Infras.Adapters;
using Microsoft.ClearScript.V8;

namespace Hellclient.V8Engine.Features.Services;

public partial class V8ScriptService
{
    private object? HandleSnapshot(V8EngineContext context, params object[] values)
    {
        try
        {
            var p = Path.Join(context.World.GetLogsPath(), context.World.ID + "." + SimpleID.Instance.GenerateID() + ".heapsnapshot");
            using var fs = new FileStream(p, FileMode.Create, FileAccess.Write);
            context.Runtime.WriteRuntimeHeapSnapshot(fs);
            context.JsAPI.Note("镜像文件写入" + p);
        }
        catch (Exception ex)
        {
            handleError(context, ex);
        }
        return null;
    }
    private void initSnapshot(V8EngineContext context)
    {
        context.Runtime.AddHostObject("snapshot", (params object[] values) => HandleSnapshot(context, values));
    }
}