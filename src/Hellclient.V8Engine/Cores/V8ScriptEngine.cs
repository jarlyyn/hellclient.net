using Hellclient.V8Engine.Features.States;
using Hellclient.World.Types;
using Hellclient.World.Cores;
using Timer = Hellclient.World.Types.Timer;
using Path = System.IO.Path;
using Hellclient.World.Configs;
using Hellclient.V8Engine.Features.Services;
namespace Hellclient.V8Engine.Cores;

public class V8ScriptEngine : IScriptEngine
{
    public V8ScriptEngine(IWorld world)
    {
        this.Context = new V8EngineContext(world);
        Service.InstallTo(Context);
    }
    public IV8ScriptService Service { get; set; } = new V8ScriptService();
    private V8EngineContext Context { get; init; }
    public void Open()=>Service.Open(Context);
    public void Close()
    {

    }
    public void OnConnect()
    {

    }
    public void OnDisconnect()
    {

    }
    public void OnTrigger(Line line, Trigger trigger, MatchResult matchResult)
    {

    }
    public void OnAlias(string message, Alias alias, MatchResult matchResult)
    {

    }
    public void OnTimer(Timer timer)
    {

    }
    public void OnCallback(Callback cb)
    {

    }
    public void OnBroadCast(Broadcast bc)
    {

    }
    public void OnHUDClick(Click c)
    {

    }
    public void OnResponse(Message msg)
    {

    }
    public void OnAssist(string script)
    { }
    public bool OnBuffer(byte[] data)
    {
        return false;
    }
    public void OnFocus()
    {

    }
    public void OnLoseFocus()
    {

    }
    public void OnKeyUp(string key)
    {

    }
    public bool OnSubneg(byte code, byte[] data)
    {
        return false;
    }
    public void Run(string script)=>Service.Run(Context,script);
}

public class V8ScriptEngineFactory : IScriptEngineFactory
{
    public static string Name => "v8";
    public static void Install()
    {
        ScriptEngineFactoryManager.RegisterFactory(Name, new V8ScriptEngineFactory());
    }
    public IScriptEngine CreateScriptEngine(IWorld world)
    {
        return new V8ScriptEngine(world);
    }
    public void NewScript(string ID)
    {
        if( Directory.Exists(Path.Combine(Deployment.Instance.ScriptsPath, ID)))
        {
            throw new Exception($"Script {ID} already exists");
        }
        Directory.CreateDirectory(Path.Combine(Deployment.Instance.ScriptsPath, ID));
        var data=File.ReadAllText(Path.Combine(Deployment.Instance.SystemPath, "template", "script", "v8.toml"));
        File.WriteAllText(Path.Combine(Deployment.Instance.ScriptsPath, ID, "script.toml"), data);
        var scriptdata=File.ReadAllText(Path.Combine(Deployment.Instance.SystemPath, "template", "script", "v8.js"));
        File.WriteAllText(Path.Combine(Deployment.Instance.ScriptsPath, ID, "script","script.js"), scriptdata);
    }
}