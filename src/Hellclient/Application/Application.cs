namespace Hellclient.Application;

using Hellclient.Core.Cores;
public class Application
{
    public static Application Instance { get; set; } = new Application();
    public void Init()
    {

    }
    public void Config()
    {
        Configs.Instance.Load();
    }
}