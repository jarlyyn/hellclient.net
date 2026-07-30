using Hellclient.World.States;

namespace Hellclient.World.Features.Services;

public interface IPathService
{
    public string GetScriptPath(WorldContext context);
    public string GetScriptHome(WorldContext context);
    public string GetModPath(WorldContext context);
    public string GetCorePath(WorldContext context);
    public string GetScriptModPath(WorldContext context);
    public string GetLogsPath(WorldContext context);

}

public class PathService : IPathService
{
    public string GetScriptHome(WorldContext context)
    {
        return "";
    }
    public string GetScriptPath(WorldContext context)
    {

        return "";
    }
    public string GetModPath(WorldContext context) => "";
    public string GetCorePath(WorldContext context) => "";
    public string GetScriptModPath(WorldContext context) => "";
    public string GetLogsPath(WorldContext context) => "";

}
