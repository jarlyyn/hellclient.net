using Path = System.IO.Path;
using Hellclient.Script.Cores;
using Hellclient.World.Cores;
using Hellclient.World.Types;
using Hellclient.World.Configs;
using Hellclient.PythonEngine.Features.Services;
using Hellclient.PythonEngine.Features.States;
using Python.Runtime;
namespace Hellclient.PythonEngine.Cores;

public class PythonEngine : IScriptEngine
{
    public static volatile bool Inited = false;
    public static nint ThreadState { get; set; }

    public PythonEngine(IWorld world)
    {

        if (Inited == false)
        {
            var pythonhome = Environment.GetEnvironmentVariable("PYTHONHOME");
            if (pythonhome != null)
            {
                Python.Runtime.PythonEngine.PythonHome = pythonhome;
            }
            var pythonpath = Environment.GetEnvironmentVariable("PYTHONPATH");
            if (pythonpath != null)
            {
                Python.Runtime.PythonEngine.PythonPath = pythonpath;
            }
            Python.Runtime.PythonEngine.Initialize();
            ThreadState = Python.Runtime.PythonEngine.BeginAllowThreads();
            Inited = true;
            Console.WriteLine("Python Engine Path:");
            Console.WriteLine(Python.Runtime.PythonEngine.PythonPath);
        }
        using (Python.Runtime.Py.GIL())
        {
            this.Context = new PythonEngineContext(world);
            Service.InstallTo(Context);
        }

    }
    public IPythonScriptService Service { get; set; } = new PythonScriptService();
    private PythonEngineContext Context { get; init; }
    public void Open() => Service.Open(Context);
    public void Close() => Service.Close(Context);

    public void OnConnect() => Service.OnConnect(Context);
    public void OnDisconnect() => Service.OnDisconnect(Context);
    public void OnTrigger(Line line, Trigger trigger, MatchResult matchResult) => Service.OnTrigger(Context, line, trigger, matchResult);
    public void OnAlias(string message, Alias alias, MatchResult matchResult) => Service.OnAlias(Context, message, alias, matchResult);
    public void OnTimer(World.Types.Timer timer) => Service.OnTimer(Context, timer);
    public void OnCallback(Callback cb) => Service.OnCallback(Context, cb);
    public void OnBroadCast(Broadcast bc) => Service.OnBroadCast(Context, bc);
    public void OnHUDClick(Click c) => Service.OnHUDClick(Context, c);
    public void OnResponse(Message msg) => Service.OnResponse(Context, msg);
    public void OnAssist(string script) => Service.OnAssist(Context, script);
    public void OnFocus() => Service.OnFocus(Context);
    public void OnLoseFocus() => Service.OnLoseFocus(Context);
    public void OnKeyUp(string key) => Service.OnKeyUp(Context, key);
    public bool OnSubneg(byte code, byte[] data) => Service.OnSubneg(Context, code, data);
    public void Run(string script) => Service.Run(Context, script);
    public bool OnLine(string line, string ansi) => Service.OnLine(Context, line, ansi);
    public void OnAfterLine(string line, string ansi) => Service.OnAfterLine(Context, line, ansi);
    public bool OnSend(string message) => Service.OnSend(Context, message);
}

public class PythonEngineFactory : IScriptEngineFactory
{
    public static string Name => "python";
    public static void Install()
    {
        ScriptEngineFactoryManager.RegisterFactory(Name, new PythonEngineFactory());
    }
    public string Label()
    {
        return "Python(风险)";
    }
    public IScriptEngine CreateScriptEngine(IWorld world)
    {
        return new PythonEngine(world);
    }
    public void NewScript(string ID)
    {
        if (Directory.Exists(Path.Combine(Deployment.Instance.ScriptsPath, ID)))
        {
            throw new Exception($"Script {ID} already exists");
        }
        Directory.CreateDirectory(Path.Combine(Deployment.Instance.ScriptsPath, ID));
        var data = File.ReadAllText(Path.Combine(Deployment.Instance.SystemPath, "template", "script", "python.toml"));
        File.WriteAllText(Path.Combine(Deployment.Instance.ScriptsPath, ID, "script.toml"), data);
        var scriptdata = File.ReadAllText(Path.Combine(Deployment.Instance.SystemPath, "template", "script", "main.py"));
        File.WriteAllText(Path.Combine(Deployment.Instance.ScriptsPath, ID, "script", "main.py"), scriptdata);
    }
}