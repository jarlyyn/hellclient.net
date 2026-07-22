using Hellclient.World.States;

namespace Hellclient.World.Features.Services;

public interface IAutomationService
{
    public void InstallTo(WorldContext context);
    public void AddTimer(Timer timer, bool replace);
    public bool RemoveTimer(string id);
}
// 自动机服务
// 用于管理Mud中的触发器/计时器/别名等自动化任务工作
public class AutomationService : IAutomationService
{
    public void InstallTo(WorldContext context)
    {
        // Install automation service to the world context
    }
    public void AddTimer(Timer timer, bool replace)
    {

    }
    public bool RemoveTimer(string id)
    {
        return false;
    }
}