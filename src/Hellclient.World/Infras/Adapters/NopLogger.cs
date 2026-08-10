using Hellclient.World.Types;

namespace Hellclient.World.Infras.Adapters;

public class NopLogger : ILogger
{
    public string LogsPath { get; set; } = string.Empty;
    public string WorldLogsPath { get; set; } = string.Empty;
    public void Log(string message)
    {
        Console.WriteLine(message);
    }
    public void WorldLog(string world, string message)
    {
        Console.WriteLine($"[{world}]:{message}");
    }
}