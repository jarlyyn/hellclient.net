namespace Hellclient.Core.Features.Services;
using Hellclient.Core.Features.Repos;
using Hellclient.World.Configs;

public class IConfigsService
{
    
}
public class ConfigsService:IConfigsService
{
    public IConfigsRepo Repo { get; init; }
    public ConfigsService(IConfigsRepo repo)
    {
        Repo = repo;
    }
    public void LoadConfig()
    {
        var systemConfig = Repo.LoadSystemConfig();
        AppConfig.System = systemConfig;
    }
}