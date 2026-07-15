using Hellclient.World.Contexts;
using Hellclient.World.Models;

namespace Hellclient.World.Shared;

public partial class World
{
    public string ID { get => Context.ID; }
    public int GetMaxHistory() => Context.MaxHistory;
    public int GetMaxLines() => Context.MaxLines;
    public int GetMaxRecent() => Context.MaxRecent;
    public string GetHost()=>Context.Data.Host;
	public void SetHost(string host)=>Context.Data.Host=host;
	public string GetPort()=>Context.Data.Port;
	public void SetPort(string port)=>Context.Data.Port=port;
	public string GetProxy()=>Context.Data.Proxy;
	public void SetProxy(string proxy)=>Context.Data.Proxy=proxy;
	public string GetName()=>Context.Data.Name;
	public void SetName(string name)=>Context.Data.Name=name;
	public bool GetShowBroadcast()=>Context.Data.ShowBroadcast;
	public void SetShowBroadcast(bool showBroadcast)=>Context.Data.ShowBroadcast=true;
	public bool GetShowSubneg()=>Context.Data.ShowSubneg;
	public void SetShowSubneg(bool showSubneg)=>Context.Data.ShowSubneg=showSubneg;
	public bool GetModEnabled()=>Context.Data.ModEnabled;
	public void SetModEnabled(bool modEnabled)=>Context.Data.ModEnabled=modEnabled;
	public bool GetAutoSave()=>Context.Data.AutoSave;
	public void SetAutoSave(bool autoSave)=>Context.Data.AutoSave=autoSave;
	public bool GetIgnoreBatchCommand()=>Context.Data.IgnoreBatchCommand;
	public void SetIgnoreBatchCommand(bool ignoreBatchCommand)=>Context.Data.IgnoreBatchCommand=ignoreBatchCommand;
	public string GetCommandStackCharacter()=>Context.Data.CommandStackCharacter;
	public void SetCommandStackCharacter(string commandStackCharacter)=>Context.Data.CommandStackCharacter=commandStackCharacter;
	public string GetScriptPrefix()=>Context.Data.ScriptPrefix;
	public void SetScriptPrefix(string scriptPrefix)=>Context.Data.ScriptPrefix=scriptPrefix;


}