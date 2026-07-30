namespace Hellclient.World.Cores;


public partial class World
{
    public void DoLog(string message) => Service.LogService.DoLog(message);

    public void HandleConnReceive(byte[] msg) => Service.LogService.HandleConnReceive(msg);
    public void HandleConnError(Exception err) => Service.LogService.HandleConnError(err);
    public void HandleConverterError(Exception err) => Service.LogService.HandleConnError(err);
    public void HandleCmdError(Exception err) => Service.LogService.HandleConnError(err);
    public void HandleTriggerError(Exception err) => Service.LogService.HandleConnError(err);
    public void HandleScriptError(Exception err) => Service.LogService.HandleConnError(err);

}