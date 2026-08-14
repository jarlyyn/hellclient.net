using Hellclient.V8Engine.Features.States;

namespace Hellclient.V8Engine.Features.Services;

public partial class V8ScriptService
{
    private object? HandleEval(V8EngineContext context, params object[] values)
    {
        try
        {
            if (values.Length == 1 && values[0] is string data)
            {
                return context.Runtime.Evaluate(data);
            }
            else if (values.Length == 2 && values[0] is string data2 && values[1] is string name)
            {
                return context.Runtime.Evaluate(name, data2);
            }
        }
        catch (Exception ex)
        {
            handleError(context, ex);
        }
        return null;
    }
    private void initEval(V8EngineContext context)
    {
        context.Runtime.AddHostObject("eval", (params object[] values) => HandleEval(context, values));
    }
}