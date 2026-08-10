using Hellclient.World.Types;

namespace Hellclient.Core.Infras.Adapters;


public class FileLogger : ILogger
{
    public required string WorldLogsPath { get; set; }
    public void Log(string message)
    {
        Console.Error.WriteLine(message);
    }
    public void WorldLog(string worldId, string message)
    {
        Console.Error.WriteLine(message);
        System.IO.File.AppendAllText(System.IO.Path.Combine(WorldLogsPath, $"{worldId}.log"), message + Environment.NewLine);
    }
}