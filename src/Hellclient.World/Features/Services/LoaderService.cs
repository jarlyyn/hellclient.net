using Hellclient.World.Infras.Components;
using Hellclient.World.States;
using Hellclient.World.Types;
using Tomlyn;

namespace Hellclient.World.Features.Services;

public interface ILoaderService
{
    //加载/保存的功能
    public void Decode(WorldContext context, string data);
    public string Encode(WorldContext context);
}

public class LoaderService : ILoaderService
{
    public IAutomationService AutomationService { get; set; } = new AutomationService();
    public void Decode(WorldContext context, string data)
    {
        var cd = TomlSerializer.Deserialize<WorldData>(data, TomlContext.Default.WorldData)!;

        cd.Timers.ForEach(t => t.SetByUser(true));
        cd.Aliases.ForEach(a => a.SetByUser(true));
        cd.Triggers.ForEach(m => m.SetByUser(true));
        context.Config.Data = cd;
        AutomationService.AddTimers(context, cd.Timers);
        AutomationService.AddAliases(context, cd.Aliases);
        AutomationService.AddTriggers(context, cd.Triggers);
    }
    public string Encode(WorldContext context)
    {
        var data = context.Config.Data;
        var timers = AutomationService.GetTimersByType(context, true);
        timers.Sort((a, b) => a.CompareTo(b));
        data.Timers = timers;
        var alias = AutomationService.GetAliasesByType(context, true);
        alias.Sort((a, b) => a.CompareTo(b));
        data.Aliases = alias;
        var triggers = AutomationService.GetTriggersByType(context, true);
        triggers.Sort((a, b) => a.CompareTo(b));
        data.Triggers = triggers;
        var content = TomlSerializer.Serialize<WorldData>(data, TomlContext.Default.WorldData);
        return content;
    }
}