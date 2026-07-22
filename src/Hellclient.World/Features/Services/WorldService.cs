using Hellclient.World.States;

namespace Hellclient.World.Features.Services;

public class WorldService
{
    public static WorldService Instance { get; set; } = new WorldService();
    public void InstallTo(WorldContext context)
    {
        ConnService.InstallTo(context);
        AutomationService.InstallTo(context);
    }
    public IConnService ConnService { get; set; } = new ConnService();
    public IAutomationService AutomationService { get; set; } = new AutomationService();
}