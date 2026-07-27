using Hellclient.World.Types;

namespace Hellclient.World.Cores;

public partial class World
{
    public string GetHost() => Service.ConfigService.GetHost(Context);
	public void SetHost(string host) => Service.ConfigService.SetHost(Context, host);
	public string GetPort() => Service.ConfigService.GetPort(Context);
	public void SetPort(string port) => Service.ConfigService.SetPort(Context, port);
	public string GetProxy() => Service.ConfigService.GetProxy(Context);
	public void SetProxy(string proxy) => Service.ConfigService.SetProxy(Context, proxy);
	public string GetName() => Service.ConfigService.GetName(Context);
	public void SetName(string name) => Service.ConfigService.SetName(Context, name);
	public bool GetShowBroadcast() => Service.ConfigService.GetShowBroadcast(Context);
	public void SetShowBroadcast(bool showBroadcast) => Service.ConfigService.SetShowBroadcast(Context, showBroadcast);
	public bool GetShowSubneg() => Service.ConfigService.GetShowSubneg(Context);
	public void SetShowSubneg(bool showSubneg) => Service.ConfigService.SetShowSubneg(Context, showSubneg);
	public bool GetModEnabled() => Service.ConfigService.GetModEnabled(Context);
	public void SetModEnabled(bool modEnabled) => Service.ConfigService.SetModEnabled(Context, modEnabled);
	public bool GetAutoSave() => Service.ConfigService.GetAutoSave(Context);
	public void SetAutoSave(bool autoSave) => Service.ConfigService.SetAutoSave(Context, autoSave);
	public bool GetIgnoreBatchCommand() => Service.ConfigService.GetIgnoreBatchCommand(Context);
	public void SetIgnoreBatchCommand(bool ignoreBatchCommand) => Service.ConfigService.SetIgnoreBatchCommand(Context, ignoreBatchCommand);
	public string GetCommandStackCharacter() => Service.ConfigService.GetCommandStackCharacter(Context);
	public void SetCommandStackCharacter(string commandStackCharacter) => Service.ConfigService.SetCommandStackCharacter(Context, commandStackCharacter);
	public string GetScriptPrefix() => Service.ConfigService.GetScriptPrefix(Context);
	public void SetScriptPrefix(string scriptPrefix) => Service.ConfigService.SetScriptPrefix(Context, scriptPrefix);
	public void SetCharset(string charset) => Service.ConfigService.SetCharset(Context, charset);
    public string GetCharset() => Service.ConfigService.GetCharset(Context);
    public void SetQueueDelay(int queueDelay) => Service.ConfigService.SetQueueDelay(Context, queueDelay);

    public int GetQueueDelay() => Service.ConfigService.GetQueueDelay(Context);
    public string GetParam(string key) => Service.ConfigService.GetParam(Context, key);
    public Dictionary<string, string> GetParams() => Service.ConfigService.GetParams(Context);
    public void SetParam(string key, string value) => Service.ConfigService.SetParam(Context, key, value);
    public void DeleteParam(string key) => Service.ConfigService.DeleteParam(Context, key);
    public string GetParamComment(string key) => Service.ConfigService.GetParamComment(Context, key);
    public Dictionary<string, string> GetParamComments() => Service.ConfigService.GetParamComments(Context);
    public void SetParamComment(string key, string comment) => Service.ConfigService.SetParamComment(Context, key, comment);
    public long GetReadyAt() => Service.ConfigService.GetReadyAt(Context);
    public int GetPosition() => Service.ConfigService.GetPosition(Context);
    public void SetPosition(int position) => Service.ConfigService.SetPosition(Context, position);
    public WorldData? GetWorldData() => Service.ConfigService.GetWorldData(Context);
    public string GetScriptID() => Service.ConfigService.GetScriptID(Context);
    public void SetScriptID(string scriptID) => Service.ConfigService.SetScriptID(Context, scriptID);
    public void SetTrusted(Trusted trusted) => Service.ConfigService.SetTrusted(Context, trusted);
    public Trusted? GetTrusted() => Service.ConfigService.GetTrusted(Context);
    public void SetPermissions(List<string> permissions) => Service.ConfigService.SetPermissions(Context, permissions);
    public List<string> GetPermissions() => Service.ConfigService.GetPermissions(Context);

}