using Hellclient.World.States;

namespace Hellclient.World.Features.Services;

public interface IPathService
{
    public string GetScriptPath(WorldContext context);
    public string GetScriptHome(WorldContext context);
    public string GetModPath(WorldContext context);
    public string GetLogsPath(WorldContext context);
    public string GetSharedPath(WorldContext context);
}

public class PathService : IPathService
{
    public string GetScriptHome(WorldContext context)
    {
        var sid = context.Config.Data.ScriptID;
        if (sid == "")
        {
            return "";
        }
        return Path.Combine(context.Paths.WorldsPath, context.ID, sid);
    }
    public string GetScriptPath(WorldContext context) => context.Paths.ScriptPath;
    public string GetModPath(WorldContext context) => context.Paths.ModPath;
    public string GetLogsPath(WorldContext context) => context.Paths.LogsPath;
    public string GetSharedPath(WorldContext context) => context.Paths.SharedPath;

}
