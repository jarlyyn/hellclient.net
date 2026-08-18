using Hellclient.World.States;

namespace Hellclient.World.Features.Services;

public interface ILogService
{
    public void DoLog(WorldContext context, string message) { }

    public void HandleConnReceive(WorldContext context, byte[] msg) { }
    public void HandleConnError(WorldContext context, Exception err) { }
    public void HandleConverterError(WorldContext context, Exception err) { }
    public void HandleCmdError(WorldContext context, Exception err) { }
    public void HandleTriggerError(WorldContext context, Exception err) { }
    public void HandleScriptError(WorldContext context, Exception err) { }
}

public class LogService : ILogService
{
    public IConvertService ConvertService { get; set; } = new ConvertService();
    public void DoLog(WorldContext context, string message)
    {

    }

    public void HandleConnReceive(WorldContext context, byte[] msg) { }
    public void HandleConnError(WorldContext context, Exception err) { }
    public void HandleConverterError(WorldContext context, Exception err) { }
    public void HandleCmdError(WorldContext context, Exception err) { }
    public void HandleTriggerError(WorldContext context, Exception err) { }
    public void HandleScriptError(WorldContext context, Exception err)
    {
        Task.Run(async () => ConvertService.DoPrintSystem(context, err.Message));
    }
}