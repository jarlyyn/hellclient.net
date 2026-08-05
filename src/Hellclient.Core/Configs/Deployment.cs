using Hellclient.Core.Configs;
using Hellclient.Core.Utils;
namespace Hellclient.Core.Configs;


public class Deployment
{
    public static Deployment Instance { get; set; } = CommonDeploy(PathUtil.GetRootPath());
    public static Deployment CommonDeploy(string rootPath)
    {
        var ctx = new Deployment
        {
            RootPath = rootPath,
            ConfigPath = System.IO.Path.Combine(rootPath, "config"),
            PersistDataPath = System.IO.Path.Combine(rootPath, "appdata", "persistdata"),
            GamePath = System.IO.Path.Combine(rootPath, "appdata", "game"),
            WorldsPath = System.IO.Path.Combine(rootPath, "appdata", "game", "worlds"),
            ModsPath = System.IO.Path.Combine(rootPath, "appdata", "game", "mods"),
            ScriptsPath = System.IO.Path.Combine(rootPath, "appdata", "game", "scripts"),
            LogsPath = System.IO.Path.Combine(rootPath, "appdata", "logs"),
            ResourcesPath = System.IO.Path.Combine(rootPath, "resources")
        };
        Directory.CreateDirectory(ctx.ConfigPath);
        Directory.CreateDirectory(ctx.PersistDataPath);
        Directory.CreateDirectory(ctx.GamePath);
        Directory.CreateDirectory(ctx.WorldsPath);
        Directory.CreateDirectory(ctx.ModsPath);
        Directory.CreateDirectory(ctx.ScriptsPath);
        Directory.CreateDirectory(ctx.LogsPath);
        return ctx;
    }
    public string RootPath { get; init; } = string.Empty;
    public string ConfigPath { get; init; } = string.Empty;
    public string PersistDataPath { get; init; } = string.Empty;
    public string GamePath { get; init; } = string.Empty;
    public string WorldsPath { get; init; } = string.Empty;
    public string ModsPath { get; init; } = string.Empty;
    public string ScriptsPath { get; init; } = string.Empty;
    public string LogsPath { get; init; } = string.Empty;
    public string ResourcesPath { get; init; } = string.Empty;

}