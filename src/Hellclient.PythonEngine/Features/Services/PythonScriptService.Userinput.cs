using System.Dynamic;
using System.Text;
using Hellclient.PythonEngine.Features.States;
using Hellclient.PythonEngine.Infras.Components.PyUserinput;
using Hellclient.World.Types;

namespace Hellclient.PythonEngine.Features.Services;



public partial class PythonScriptService
{
    public void initUserinput(PythonEngineContext context)
    {
        var m = new PyUserinput(context.World).Convert();
        context.Scope.Set("Userinput", m);
    }
}