using Hellclient.World.States;

namespace Hellclient.World.Features.Services;

public interface IPathService
{
    public string GetScriptPath(WorldContext context);
    public string GetScriptHome(WorldContext context);
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
}
