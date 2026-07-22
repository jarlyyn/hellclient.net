using Hellclient.Core.Configs;
using Hellclient.World.Infras.Components;
using Hellclient.World.Types;
using Tomlyn;

namespace Hellclient.Core.Features.Repos;

public interface IConfigsRepo
{
    public SystemConfig LoadSystemConfig();
}
public class ConfigsRepo : IConfigsRepo
{
    public SystemConfig LoadSystemConfig()
    {
        var content=System.IO.File.ReadAllText(System.IO.Path.Combine(Deployment.Instance.ConfigPath,"system.toml"));
        var systemConfig = TomlSerializer.Deserialize<SystemConfig>(content,TomlContext.Default.SystemConfig);
        return systemConfig!;
    }
}