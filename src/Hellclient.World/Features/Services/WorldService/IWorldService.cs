using Hellclient.World.Features.Services;
using Hellclient.World.States;

namespace Hellclient.World.Features.WorldServices;

public interface IWorldService
{
    public void InstallTo(WorldContext context);
    public IConnService ConnService { get; set; }
    public IAutomationService AutomationService { get; set; }
    public IMetronomeService MetronomeService { get; set; }
    public IInfoService InfoService { get; set; }
    public IQueueService QueueService { get; set; }
    public IHUDService HudService { get; set; }
    public IConfigService ConfigService { get; set; }
    public IScriptService ScriptService { get; set; }

    public IPathService PathService { get; set; }
    public IScriptBridgeService ScriptBridgeService { get; set; }
    public ILogService LogService { get; set; }
    public ILoaderService LoaderService { get; set; }

}
