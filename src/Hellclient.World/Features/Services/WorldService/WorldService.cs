using Hellclient.World.States;
using Hellclient.World.Features.Services;
namespace Hellclient.World.Features.WorldServices;

public class WorldService : IWorldService
{
    public void InstallTo(WorldContext context)
    {
        ConnService.InstallTo(context);
        MetronomeService.InstallTo(context);
        InfoService.InstallTo(context);
        QueueService.InstallTo(context);
        // HudService.InstallTo(context);
        ConfigService.InstallTo(context);
        ScriptService.InstallTo(context);
        AutomationService.InstallTo(context);
        ScriptBridgeService.InstallTo(context);
    }
    public void RemoveFrom(WorldContext context)
    {
        context.Dispose();
    }
    public IConnService ConnService { get; set; } = new ConnService();
    public IAutomationService AutomationService { get; set; } = new AutomationService();

    public IMetronomeService MetronomeService { get; set; } = new MetronomeService();

    public IInfoService InfoService { get; set; } = new InfoService();

    public IQueueService QueueService { get; set; } = new QueueService();
    public IHUDService HudService { get; set; } = new HUDService();
    public IConfigService ConfigService { get; set; } = new ConfigService();
    public IScriptService ScriptService { get; set; } = new ScriptService();

    public IPathService PathService { get; set; } = new PathService();
    public IScriptBridgeService ScriptBridgeService { get; set; } = new ScriptBridgeService();

    public ILogService LogService { get; set; } = new LogService();
    public ILoaderService LoaderService { get; set; } = new LoaderService();
}