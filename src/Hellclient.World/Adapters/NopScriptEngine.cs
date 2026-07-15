namespace Hellclient.World.Adapters;

using Hellclient.World.Interfaces;
using Hellclient.World.Models;

//空脚本引擎实现
public class NopScriptEngine : IScriptEngine
{
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
    {}
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
    public void Run(string script){
        
    }
}