using Hellclient.World.Utils;
using Hellclient.Application;
using Hellclient.Core.WebApp;
using Hellclient.World.Configs;
using Hellclient.WebUI;
#if !nov8
using Hellclient.V8Engine.Cores;
#endif
#if !nopy
using Hellclient.PythonEngine.Cores;
#endif
using Hellclient.Helpers;


Application.Instance.Init();
Application.Instance.Config();
CharsetUtil.InstallEncodingProvider();
#if !nov8
V8ScriptEngineFactory.Install();
#endif
#if !nopy
PythonEngineFactory.Install();
#endif
WebUI.Instance.Init();
Console.WriteLine($"Hellclient version {AppVersion.Version.FullVersionCode()} (API {AppVersion.APIVersion.FullVersionCode()})");
Console.WriteLine($"Listening http on {AppConfig.System.Addr}");
await WebApp.Instance.Start(ConfigHelper.ConvertListenUrl(AppConfig.System.Addr));
