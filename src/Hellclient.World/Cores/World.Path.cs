

using Hellclient.World.Types;

namespace Hellclient.World.Cores;


public partial class World
{
    public string GetScriptPath() => Service.PathService.GetScriptPath(Context);
    public string GetModPath() => Service.PathService.GetModPath(Context);
    public string GetCorePath() => Service.PathService.GetCorePath(Context);
    public string GetScriptModPath() => Service.PathService.GetScriptModPath(Context);
    public string GetLogsPath() => Service.PathService.GetLogsPath(Context);
    public string GetScriptHome() => Service.PathService.GetScriptHome(Context);

}