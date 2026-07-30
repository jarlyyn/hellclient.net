using Hellclient.World.Types;

using Timer = Hellclient.World.Types.Timer;

namespace Hellclient.World.Cores;

public partial class World
{
    public bool DoDeleteTimer(string id) => Service.AutomationService.RemoveTimer(Context, id);
    public bool DoDeleteTimerByName(string name) => Service.AutomationService.RemoveTimerByName(Context, name);
    public int DoDeleteTemporaryTimers() => Service.AutomationService.DeleteTemporaryTimers(Context);
    public int DoDeleteTimerGroup(string group, bool byUser) => Service.AutomationService.DeleteTimerGroup(Context, group, byUser);
    public bool DoEnableTimerByName(string name, bool enabled) => Service.AutomationService.EnableTimerByName(Context, name, enabled);
    public int DoEnableTimerGroup(string group, bool enabled) => Service.AutomationService.EnableTimerGroup(Context, group, enabled);
    public bool DoResetNamedTimer(string name) => Service.AutomationService.ResetNamedTimer(Context, name);
    public Timer? GetTimer(string name) => Service.AutomationService.GetTimer(Context, name);
    public List<Timer> GetTimersByType(bool byUser) => Service.AutomationService.GetTimersByType(Context, byUser);
    public void DoDeleteTimerByType(bool byUser) => Service.AutomationService.DoDeleteTimerByType(Context, byUser);
    public void AddTimers(List<Timer> ts) => Service.AutomationService.AddTimers(Context, ts);
    public void DoResetTimers() => Service.AutomationService.ResetTimers(Context);
    public FoundStringResult GetTimerOption(string name, string option) => Service.AutomationService.GetTimerOption(Context, name, option);
    public FoundStringResult GetTimerInfo(string name, int infotype) => Service.AutomationService.GetTimerInfo(Context, name, infotype);
    public FoundBoolResult SetTimerOption(string name, string option, string value) => Service.AutomationService.SetTimerOption(Context, name, option, value);
    public bool HasNamedTimer(string name) => Service.AutomationService.HasNamedTimer(Context, name);
    public List<string> DoListTimerNames(bool byUser) => Service.AutomationService.ListTimerNames(Context, byUser);
    public bool AddTimer(Timer timer, bool byUser) => Service.AutomationService.AddTimer(Context, timer, byUser);
    public int DoUpdateTimer(Timer timer) => Service.AutomationService.DoUpdateTimer(Context, timer);
    public bool DoDeleteAlias(string id) => Service.AutomationService.DoDeleteAlias(Context, id);
    public bool DoDeleteAliasByName(string name) => Service.AutomationService.DoDeleteAliasByName(Context, name);
    public int DoDeleteTemporaryAliases() => Service.AutomationService.DoDeleteTemporaryAliases(Context);
    public int DoDeleteAliasGroup(string group, bool byUser) => Service.AutomationService.DoDeleteAliasGroup(Context, group, byUser);
    public bool DoEnableAliasByName(string name, bool enabled) => Service.AutomationService.DoEnableAliasByName(Context, name, enabled);
    public int DoEnableAliasGroup(string group, bool enabled) => Service.AutomationService.DoEnableAliasGroup(Context, group, enabled);
    public Alias? GetAlias(string name) => Service.AutomationService.GetAlias(Context, name);
    public List<Alias> GetAliasesByType(bool byUser) => Service.AutomationService.GetAliasesByType(Context, byUser);
    public void DoDeleteAliasByType(bool byUser) => Service.AutomationService.DoDeleteAliasByType(Context, byUser);
    public void AddAliases(List<Alias> aliases) => Service.AutomationService.AddAliases(Context, aliases);
    public FoundStringResult GetAliasOption(string name, string option) => Service.AutomationService.GetAliasOption(Context, name, option);
    public FoundStringResult GetAliasInfo(string name, int infotype) => Service.AutomationService.GetAliasInfo(Context, name, infotype);
    public FoundBoolResult SetAliasOption(string name, string option, string value) => Service.AutomationService.SetAliasOption(Context, name, option, value);
    public bool HasNamedAlias(string name) => Service.AutomationService.HasNamedAlias(Context, name);
    public List<string> DoListAliasNames(bool byUser) => Service.AutomationService.DoListAliasNames(Context, byUser);
    public bool AddAlias(Alias alias, bool byUser) => Service.AutomationService.AddAlias(Context, alias, byUser);
    public int DoUpdateAlias(Alias alias) => Service.AutomationService.DoUpdateAlias(Context, alias);

    public bool DoDeleteTrigger(string id) => Service.AutomationService.DoDeleteTrigger(Context, id);
    public bool DoDeleteTriggerByName(string name) => Service.AutomationService.DoDeleteTriggerByName(Context, name);
    public int DoDeleteTemporaryTriggers() => Service.AutomationService.DoDeleteTemporaryTriggers(Context);
    public int DoDeleteTriggerGroup(string group, bool byUser) => Service.AutomationService.DoDeleteTriggerGroup(Context, group, byUser);
    public bool DoEnableTriggerByName(string name, bool enabled) => Service.AutomationService.DoEnableTriggerByName(Context, name, enabled);
    public int DoEnableTriggerGroup(string group, bool enabled) => Service.AutomationService.DoEnableTriggerGroup(Context, group, enabled);
    public Trigger? GetTrigger(string name) => Service.AutomationService.GetTrigger(Context, name);
    public List<Trigger> GetTriggersByType(bool byUser) => Service.AutomationService.GetTriggersByType(Context, byUser);
    public void DoDeleteTriggerByType(bool byUser) => Service.AutomationService.DoDeleteTriggerByType(Context, byUser);
    public void AddTriggers(List<Trigger> triggers) => Service.AutomationService.AddTriggers(Context, triggers);
    public FoundStringResult GetTriggerOption(string name, string option) => Service.AutomationService.GetTriggerOption(Context, name, option);
    public FoundStringResult GetTriggerInfo(string name, int infotype) => Service.AutomationService.GetTriggerInfo(Context, name, infotype);
    public FoundBoolResult SetTriggerOption(string name, string option, string value) => Service.AutomationService.SetTriggerOption(Context, name, option, value);
    public bool HasNamedTrigger(string name) => Service.AutomationService.HasNamedTrigger(Context, name);
    public List<string> DoListTriggerNames(bool byUser) => Service.AutomationService.DoListTriggerNames(Context, byUser);
    public bool AddTrigger(Trigger trigger, bool byUser) => Service.AutomationService.AddTrigger(Context, trigger, byUser);
    public int DoUpdateTrigger(Trigger trigger) => Service.AutomationService.DoUpdateTrigger(Context, trigger);
    public MatchResult? DoGetTriggerWildcard(string name) => Service.AutomationService.GetTriggerWildcard(Context, name);
    public void DoStopEvaluatingTriggers() => Service.AutomationService.DoStopEvaluatingTriggers(Context);
    public void DoExecute(string message) => Service.AutomationService.DoExecute(Context, message);

    public void DoMultiLinesFlush() => Service.AutomationService.DoMultiLinesFlush(Context);
    public List<string> DoMultiLinesLast(int count) => new();

}