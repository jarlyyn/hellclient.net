using Hellclient.World.States;
using Hellclient.World.Features.Services;
namespace Hellclient.World.Features.WorldServices;

public class WorldService : IWorldService
{
    public void InstallTo(WorldContext context)
    {
        ConnService.InstallTo(context);
        AutomationService.InstallTo(context);
        MetronomeService.InstallTo(context);
    }
    public IConnService ConnService { get; set; } = new ConnService();
    public IAutomationService AutomationService { get; set; } = new AutomationService();

    public IMetronomeService MetronomeService { get; set; } = new MetronomeService();
}