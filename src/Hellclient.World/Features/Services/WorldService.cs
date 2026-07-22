using Hellclient.World.States;

namespace Hellclient.World.Features.Services;

public interface IWorldService
{
    public void InstallTo(WorldContext context);
    public IConnService ConnService { get; set; }
    public IAutomationService AutomationService { get; set; }
}
public class WorldService : IWorldService
{
    public void InstallTo(WorldContext context)
    {
        ConnService.InstallTo(context);
        AutomationService.InstallTo(context);
    }
    public IConnService ConnService { get; set; } = new ConnService();
    public IAutomationService AutomationService { get; set; } = new AutomationService();
}