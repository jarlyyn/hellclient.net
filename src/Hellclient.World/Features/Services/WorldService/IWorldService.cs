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

}
