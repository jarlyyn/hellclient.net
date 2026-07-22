using Hellclient.Core.Configs;
using Hellclient.Core.Utils;
namespace Hellclient.Core.Configs;


public class Deployment
{
    public static Deployment Instance { get; set; } = CommonDeploy(PathUtil.GetRootPath());
    public static Deployment CommonDeploy(string rootPath)
    {
        var ctx= new Deployment
        {
            RootPath = rootPath,
            ConfigPath = System.IO.Path.Combine(rootPath, "config"),
            PersistDataPath = System.IO.Path.Combine(rootPath, "appdata", "persistdata"),
            GamePath = System.IO.Path.Combine(rootPath, "appdata", "game")
        };
        Directory.CreateDirectory(ctx.ConfigPath);
        Directory.CreateDirectory(ctx.PersistDataPath);
        Directory.CreateDirectory(ctx.GamePath);
        return ctx;
    }
    public string RootPath { get; init; } = string.Empty;
    public string ConfigPath { get; init; } = string.Empty;
    public string PersistDataPath { get; init; } = string.Empty;
    public string GamePath { get; init; } = string.Empty;

}