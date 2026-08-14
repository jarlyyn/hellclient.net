namespace Hellclient.World.Cores;


public partial class World
{
    public void DoLog(string message) => Service.LogService.DoLog(Context, message);

    public void HandleConnReceive(byte[] msg) => Service.LogService.HandleConnReceive(Context, msg);
    public void HandleConnError(Exception err) => Service.LogService.HandleConnError(Context, err);
    public void HandleConverterError(Exception err) => Service.LogService.HandleConverterError(Context, err);
    public void HandleCmdError(Exception err) => Service.LogService.HandleCmdError(Context, err);
    public void HandleTriggerError(Exception err) => Service.LogService.HandleTriggerError(Context, err);
    public void HandleScriptError(Exception err) => Service.LogService.HandleScriptError(Context, err);

}