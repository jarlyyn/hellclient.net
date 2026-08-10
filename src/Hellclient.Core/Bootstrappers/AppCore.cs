using Hellclient.Core.Configs;
using Hellclient.Core.Cores;
using Hellclient.Core.Features.States;
using Hellclient.Core.Infras.Adapters;
using Hellclient.Core.Infras.Components;

namespace Hellclient.Core.Bootstrappers;

public class AppCore
{
    public static AppCore Instance { get; set; } = BuildDefault();
    public required Prophet Prophet { get; set; }
    public static AppCore BuildDefault()
    {
        var logger = new FileLogger()
        {
            WorldLogsPath = System.IO.Path.Combine(Deployment.Instance.LogsPath, "logs")
        };
        var tctx = new TitanContext()
        {
            Deployment = Deployment.Instance,
            ScriptPath = Deployment.Instance.ScriptsPath,
            WorldsPath = Deployment.Instance.WorldsPath,
            Logger = logger,
            WorldFactory=new WorldFactory(logger),
        };
        var pctx = new ProphetContext()
        {
            Deployment = Deployment.Instance,
            TitanContext = tctx,
        };        
        var prophet = new Prophet()
        {
            Context = pctx,
        };
        prophet.Init();
        var app = new AppCore()
        {
            Prophet = prophet
        };
        return app;
    }
}