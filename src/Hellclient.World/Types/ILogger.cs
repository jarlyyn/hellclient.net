namespace Hellclient.World.Types;

public interface ILogger
{
    void Log(string message);
    void WorldLog(string world, string message);
}