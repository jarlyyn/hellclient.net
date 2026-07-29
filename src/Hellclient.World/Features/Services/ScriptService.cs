using Hellclient.World.States;
using Hellclient.World.Types;

namespace Hellclient.World.Features.Services;

public interface IScriptService
{
    public void InstallTo(WorldContext context);
    public void DoRunScript(WorldContext context, string script);
    public void SendAlias(WorldContext context, string message, Alias alias, MatchResult matchResult);
    public void SendTrigger(WorldContext context, Line line, Trigger trigger, MatchResult matchResult);
}

public class ScriptService : IScriptService
{
    public void InstallTo(WorldContext context)
    {
        // Install script service to the world context
    }
    public void DoRunScript(WorldContext context, string script)
    {
        // TODO: Implement script execution logic
    }
    public void SendAlias(WorldContext context, string message, Alias alias, MatchResult matchResult)
    {
        // TODO: Implement trigger sending logic
    }
    public void SendTrigger(WorldContext context, Line line, Trigger trigger, MatchResult matchResult)
    {
        // TODO: Implement trigger sending logic
    }
}