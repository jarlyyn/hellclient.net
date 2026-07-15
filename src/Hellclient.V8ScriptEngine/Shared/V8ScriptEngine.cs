using Hellclient.V8ScriptEngine.Context;
using Hellclient.World.Interfaces;
using Hellclient.World.Models;
using Hellclient.World.Shared;
using Timer = Hellclient.World.Models.Timer;

namespace Hellclient.V8ScriptEngine;

public class V8ScriptEngine : IScriptEngine
{
    public V8ScriptEngine(IWorld world)
    {
        this.Context = new V8EngineContext(world);
    }
    private V8EngineContext Context { get; init; }
    public void Open()
    {

    }
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
    public void Run(string script)
    {

    }
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
}