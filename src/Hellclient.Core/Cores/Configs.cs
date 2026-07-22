namespace Hellclient.Core.Cores;

using Hellclient.Core.Features.Repos;
using Hellclient.Core.Features.Services;

public class Configs
{
    public Configs()
    {
        Service = new ConfigsService(new ConfigsRepo());
    }
    public static Configs Instance { get; set; } = new Configs();
    public ConfigsService Service { get; set; }

    public void Load()
    {
        Service.LoadConfig();
    }
}