using Hellclient.World.States;
using Hellclient.World.Types;
using Hellclient.World.Cores;
using Timer = Hellclient.World.Types.Timer;


namespace Hellclient.World.Cores;

public partial class World
{
    public string GetStatus() => string.Empty;
    public void SetStatus(string status) { }
    public void RequestPermissions(Authorization authorization) { }
    public ScriptData? GetScriptData() => null;

    public string GetScriptType() => string.Empty;
    public string GetScriptPath() => string.Empty;
    public string GetModPath() => string.Empty;
    public string GetCorePath() => string.Empty;
    public string GetScriptModPath() => string.Empty;
    public string GetLogsPath() => string.Empty;
    public string GetScriptHome() => string.Empty;
    public void DoLog(string message) { }
    public void RequestTrustDomains(Authorization authorization) { }
    public object GetPluginOptions() => new object();
    public void DoReloadPermissions() { }
    public void DoExecute(string message) { }
    public (byte[] Data, Exception? Error) DoEncode() => (Array.Empty<byte>(), null);
    public Exception? DoDecode(byte[] data) => null;
    public Exception? DoReloadScript() => null;
    public Exception? DoSaveScript() => null;
    public void DoUseScript(string scriptPath) { }
    public List<RequiredParam> GetRequiredParams() => [];
    public void DoRunScript(string script) { }

    public void DoSendHUDClickToScript(Click click) { }
    public void DoSendBroadcastToScript(Broadcast broadcast) { }
    public bool HandleBuffer(byte[] buffer) => false;
    public bool HandleSubneg(byte[] buffer) => false;
    public void HandleFocus() { }
    public void HandleLoseFocus() { }
    public void DoSendTimerToScript(Timer timer) { }

    public void DoSendCallbackToScript(Callback cb) { }
    public void DoSendKeyUpToScript(string key) { }
    public void DoAssist() { }
    public void DoMultiLinesFlush() { }
    public List<string> DoMultiLinesLast(int count) => new();
    public IMapper? GetMapper() => null;
    public void HandleConnReceive(byte[] msg) { }
    public void HandleConnError(Exception err) { }
    public void HandleConnPrompt(byte[] msg) { }
    public void HandleConverterError(Exception err) { }
    public void HandleCmdError(Exception err) { }
    public void HandleTriggerError(Exception err) { }
    public void HandleScriptError(Exception err) { }
    public (string, string) GetScriptCaller() => (string.Empty, string.Empty);

    public void DoSendResponseToScript(Message msg) { }

}