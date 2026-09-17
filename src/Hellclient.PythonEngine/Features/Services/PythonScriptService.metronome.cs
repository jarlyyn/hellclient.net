using System.Dynamic;
using Hellclient.PythonEngine.Features.States;
using Hellclient.PythonEngine.Infras.Components;
using Hellclient.World.Types;
using Python.Runtime;

namespace Hellclient.PythonEngine.Features.Services;

public partial class PythonScriptService
{
    public void initMetronome(PythonEngineContext context)
    {
        var m = new PyMetronome(context.World).ToPython();
        context.Scope.Set("Metronome", m);
    }
}