using Hellclient.World.States;

namespace Hellclient.World.Features.Services;

public interface ILogService
{
    //日志功能
    public void DoLog(WorldContext context, string message) { }

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
        File.AppendAllText(Path.Combine(context.Paths.LogsPath, $"{context.ID}.log"), message + "\n");
    }
    private void dolog(WorldContext context, string message)
    {
        Task.Run(() => ConvertService.DoPrintSystem(context, message));
        Console.Error.WriteLine(message);
        DoLog(context, message);
    }
    public void HandleConnError(WorldContext context, Exception err)
    {
        dolog(context, err.Message);
    }
    public void HandleConverterError(WorldContext context, Exception err)
    {
        dolog(context, err.Message);
    }
    public void HandleCmdError(WorldContext context, Exception err)
    {
        dolog(context, err.Message);
    }
    public void HandleTriggerError(WorldContext context, Exception err)
    {
        dolog(context, err.Message);
    }
    public void HandleScriptError(WorldContext context, Exception err)
    {
        dolog(context, err.Message);
    }
}