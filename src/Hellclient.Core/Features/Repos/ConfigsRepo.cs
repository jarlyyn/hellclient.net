using Hellclient.World.Configs;
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
        if (systemConfig == null)
        {
            throw new Exception("Failed to load system config");
        }
        if (systemConfig.MaxHistory <= 0)
        {
            systemConfig.MaxHistory = SystemConfig.DefaultMaxHistory;
        }
        if (systemConfig.MaxLines <= 0)
        {
            systemConfig.MaxLines = SystemConfig.DefaultMaxLines;
        }
        if (systemConfig.MaxRecent <= 0)
        {
            systemConfig.MaxRecent = SystemConfig.DefaultMaxRecent;
        }
        if (systemConfig.LinesPerScreen <= 0)
        {
            systemConfig.LinesPerScreen = SystemConfig.DefaultLinesPerScreen;
        }
        return systemConfig!;
    }
}