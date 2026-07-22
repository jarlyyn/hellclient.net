namespace Hellclient.World.Types;
using Hellclient.World.Types;
public interface IScriptEngine
{
    public void Open();
    public void Close();
    public void OnConnect();
    public void OnDisconnect();
    public void OnTrigger(Line line, Trigger trigger, MatchResult matchResult);
    public void OnAlias(string message, Alias alias, MatchResult matchResult);
    public void OnTimer(Timer timer);
    public void OnCallback(Callback cb);
    public void OnBroadCast(Broadcast bc );
    public void OnHUDClick(Click c );
    public void OnResponse(Message msg);
    public void OnAssist(string script );
    public bool OnBuffer(byte[] data);
    public void OnFocus();
    public void OnLoseFocus();
    public void OnKeyUp(string key);
    public bool OnSubneg(byte code, byte[] data);
    public void Run(string script);
}