using Hellclient.World.Types;
using Timer = Hellclient.World.Types.Timer;

namespace Hellclient.World.Cores;

public partial class World
{
    public string GetStatus() => Service.ScriptService.GetStatus(Context);
    public void SetStatus(string status) => Service.ScriptService.SetStatus(Context, status);
    public void DoRunScript(string script) => Service.ScriptService.Run(Context, script);

    public void DoSendHUDClickToScript(Click click) => Service.ScriptService.SendHUDClick(Context, click);
    public void DoSendBroadcastToScript(Broadcast broadcast) => Service.ScriptService.SendBroadcast(Context, broadcast);
    public bool HandleBuffer(byte[] buffer) => Service.ScriptService.HandleBuffer(Context, buffer);
    public bool HandleSubneg(byte[] buffer) => Service.ScriptService.HandleSubneg(Context, buffer);
    public void HandleFocus() => Service.ScriptService.HandleFocus(Context);
    public void HandleLoseFocus() => Service.ScriptService.HandleLoseFocus(Context);
    public void DoSendTimerToScript(Timer timer) => Service.ScriptService.SendTimer(Context, timer);

    public void DoSendCallbackToScript(Callback cb) => Service.ScriptService.SendCallback(Context, cb);
    public void DoSendKeyUpToScript(string key) => Service.ScriptService.KeyUp(Context, key);
    public void DoAssist() => Service.ScriptService.Assist(Context);
    public CreatorInfo GetScriptCaller() => Service.ScriptService.CreatorAndType(Context);
    public void DoSendResponseToScript(Message msg) => Service.ScriptService.SendResponse(Context, msg);
    public List<RequiredParam> GetRequiredParams() => Service.ScriptService.GetRequiredParams(Context);
    public ScriptData GetScriptData() => Service.ScriptService.ScriptData(Context);
    public PlainOptions GetPluginOptions() => Service.ScriptService.PluginOptions(Context);

    public void DoReloadPermissions() => Service.ScriptService.ReloadPermissions(Context);
    public void DoReloadScript() => Service.ScriptBridgeService.Reload(Context);
    public void DoSaveScript() => Service.ScriptBridgeService.Save(Context);
    public void DoUseScript(string scriptPath) => Service.ScriptBridgeService.UseScript(Context, scriptPath);

}