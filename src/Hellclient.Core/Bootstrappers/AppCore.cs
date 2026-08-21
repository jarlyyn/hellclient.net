using Hellclient.World.Configs;
using Hellclient.Core.Cores;
using Hellclient.Core.Features.States;
using Hellclient.Core.Infras.Adapters;
using Hellclient.Core.Infras.Components;
using Hellclient.Core.Features.Repos;
using Hellclient.Core.Features.Services;

namespace Hellclient.Core.Bootstrappers;

public class AppCore
{
    public static AppCore Instance { get; set; } = BuildDefault();
    public required Prophet Prophet { get; set; }
    public required Messenger Messenger { get; set; }
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
            WorldFactory = new WorldFactory(logger),
        };
        var pctx = new ProphetContext()
        {
            Deployment = Deployment.Instance,
            TitanContext = tctx,
        };
        var userPasswordRepo = new UserPasswordRepo(System.IO.Path.Combine(Deployment.Instance.PersistDataPath, "userpassword.persist"));
        var prophet = new Prophet()
        {
            Context = pctx,
            ProphetService = new ProphetService(userPasswordRepo)
        };
        prophet.Init();
        var mctx = new MessengerContext()
        {
            TitanContext = tctx
        };
        var messenger = new Messenger()
        {
            Context = mctx,
            MessengerService = new MessengerService()
        };
        messenger.init();
        var app = new AppCore()
        {
            Prophet = prophet,
            Messenger = messenger
        };
        return app;
    }
}