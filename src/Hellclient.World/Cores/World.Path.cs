

using Hellclient.World.Types;

namespace Hellclient.World.Cores;


public partial class World
{
    public string GetScriptPath() => Service.PathService.GetScriptPath(Context);
    public string GetModPath() => Service.PathService.GetModPath(Context);
    public string GetLogsPath() => Service.PathService.GetLogsPath(Context);
    public string GetScriptHome() => Service.PathService.GetScriptHome(Context);
    public string GetSharedPath() => Service.PathService.GetSharedPath(Context);

}