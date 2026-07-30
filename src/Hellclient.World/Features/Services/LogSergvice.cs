namespace Hellclient.World.Features.Services;

public interface ILogService
{
    public void DoLog(string message) { }

    public void HandleConnReceive(byte[] msg) { }
    public void HandleConnError(Exception err) { }
    public void HandleConverterError(Exception err) { }
    public void HandleCmdError(Exception err) { }
    public void HandleTriggerError(Exception err) { }
    public void HandleScriptError(Exception err) { }
}

public class LogService : ILogService
{
    public void DoLog(string message) { }

    public void HandleConnReceive(byte[] msg) { }
    public void HandleConnError(Exception err) { }
    public void HandleConverterError(Exception err) { }
    public void HandleCmdError(Exception err) { }
    public void HandleTriggerError(Exception err) { }
    public void HandleScriptError(Exception err) { }
}