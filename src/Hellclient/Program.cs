using Hellclient.World.Utils;
using Hellclient.Application;
using Hellclient.Core.WebApp;
using Hellclient.World.Configs;
using Hellclient.WebUI;
using Hellclient.V8ScriptEngine.Cores;


Application.Instance.Init();
Application.Instance.Config();
CharsetUtil.InstallEncodingProvider();
V8ScriptEngineFactory.Install();
WebUI.Instance.Init();
Console.WriteLine($"Starting web server at http://{AppConfig.System.Addr}");
await WebApp.Instance.Start($"http://{AppConfig.System.Addr}");
