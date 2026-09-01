using Hellclient.V8Engine.Features.States;
using Hellclient.World.Types;
using Hellclient.World.Cores;
using Hellclient.Script.Cores;
using Timer = Hellclient.World.Types.Timer;
using Path = System.IO.Path;
using Hellclient.World.Configs;
using Hellclient.V8Engine.Features.Services;
using Microsoft.ClearScript.V8;
namespace Hellclient.V8Engine.Cores;

public class V8JsScriptEngine : IScriptEngine
{
    public V8JsScriptEngine(IWorld world)
    {
        this.Context = new V8EngineContext(world);
        Service.InstallTo(Context);
    }
    public IV8ScriptService Service { get; set; } = new V8ScriptService();
    private V8EngineContext Context { get; init; }
    public void Open() => Service.Open(Context);
    public void Close() => Service.Close(Context);

    public void OnConnect() => Service.OnConnect(Context);
    public void OnDisconnect() => Service.OnDisconnect(Context);
    public void OnTrigger(Line line, Trigger trigger, MatchResult matchResult) => Service.OnTrigger(Context, line, trigger, matchResult);
    public void OnAlias(string message, Alias alias, MatchResult matchResult) => Service.OnAlias(Context, message, alias, matchResult);
    public void OnTimer(Timer timer) => Service.OnTimer(Context, timer);
    public void OnCallback(Callback cb) => Service.OnCallback(Context, cb);
    public void OnBroadCast(Broadcast bc) => Service.OnBroadCast(Context, bc);
    public void OnHUDClick(Click c) => Service.OnHUDClick(Context, c);
    public void OnResponse(Message msg) => Service.OnResponse(Context, msg);
    public void OnAssist(string script) => Service.OnAssist(Context, script);
    public bool OnBuffer(byte[] data) => Service.OnBuffer(Context, data);
    public void OnFocus() => Service.OnFocus(Context);
    public void OnLoseFocus() => Service.OnLoseFocus(Context);
    public void OnKeyUp(string key) => Service.OnKeyUp(Context, key);
    public bool OnSubneg(byte code, byte[] data) => Service.OnSubneg(Context, code, data);
    public void Run(string script) => Service.Run(Context, script);
    public bool OnLine(string line) => Service.OnLine(Context, line);
    public void OnAfterLine(string line) => Service.OnAfterLine(Context, line);
    public bool OnSend(string message) => Service.OnSend(Context, message);
}

public class V8ScriptEngineFactory : IScriptEngineFactory
{
    public static string Name => "v8";
    public static void Install()
    {
        //在低配置(1核2g)的机器上，会造成死锁。
        // V8Settings.GlobalFlags |= V8GlobalFlags.DisableJITCompilation;
        V8Settings.GlobalFlags |= V8GlobalFlags.DisableBackgroundWork;
        ScriptEngineFactoryManager.RegisterFactory(Name, new V8ScriptEngineFactory());
    }
    public string Label()
    {
        return "V8 Javascript";
    }
    public IScriptEngine CreateScriptEngine(IWorld world)
    {
        return new V8JsScriptEngine(world);
    }
    public void NewScript(string ID)
    {
        if (Directory.Exists(Path.Combine(Deployment.Instance.ScriptsPath, ID)))
        {
            throw new Exception($"Script {ID} already exists");
        }
        Directory.CreateDirectory(Path.Combine(Deployment.Instance.ScriptsPath, ID));
        var data = File.ReadAllText(Path.Combine(Deployment.Instance.SystemPath, "template", "script", "v8.toml"));
        File.WriteAllText(Path.Combine(Deployment.Instance.ScriptsPath, ID, "script.toml"), data);
        var scriptdata = File.ReadAllText(Path.Combine(Deployment.Instance.SystemPath, "template", "script", "v8.js"));
        File.WriteAllText(Path.Combine(Deployment.Instance.ScriptsPath, ID, "script", "script.js"), scriptdata);
    }
}