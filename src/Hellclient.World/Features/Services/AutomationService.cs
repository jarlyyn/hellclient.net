using Hellclient.World.Components.Automation;
using Hellclient.World.States;
using Hellclient.World.Types;
using Timer = Hellclient.World.Types.Timer;

namespace Hellclient.World.Features.Services;

public interface IAutomationService
{
    public void InstallTo(WorldContext context);
    public MatchResult? GetTriggerWildcard(WorldContext context, string name);
    public void DoStopEvaluatingTriggers(WorldContext context);
    public bool AddTimer(WorldContext context, Timer timer, bool replace);
    public void AddTimers(WorldContext context, List<Timer> ts);
    public bool RemoveTimer(WorldContext context, string id);
    public bool RemoveTimerByName(WorldContext context, string name);
    public int DeleteTemporaryTimers(WorldContext context);
    public int DeleteTimerGroup(WorldContext context, string group, bool byUser);
    public bool EnableTimerByName(WorldContext context, string name, bool enabled);
    public int EnableTimerGroup(WorldContext context, string group, bool enabled);
    public List<string> ListTimerNames(WorldContext context, bool byUser);
    public bool HasNamedTimer(WorldContext context, string name);
    public bool ResetNamedTimer(WorldContext context, string name);
    public void ResetTimers(WorldContext context);
    public FoundStringResult GetTimerOption(WorldContext context, string name, string option);
    public FoundStringResult GetTimerInfo(WorldContext context, string name, int infotype);
    public FoundBoolResult SetTimerOption(WorldContext context, string name, string option, string value);
    public List<Timer> GetTimersByType(WorldContext context, bool byuser);
    public Timer? GetTimer(WorldContext context, string id);
    public void DoDeleteTimerByType(WorldContext context, bool byuser);
    public int DoUpdateTimer(WorldContext context, Timer ti);
    public bool DoDeleteAlias(WorldContext context, string id);
    public bool DoDeleteAliasByName(WorldContext context, string name);
    public int DoDeleteTemporaryAliases(WorldContext context);
    public int DoDeleteAliasGroup(WorldContext context, string group, bool byUser);
    public bool DoEnableAliasByName(WorldContext context, string name, bool enabled);
    public int DoEnableAliasGroup(WorldContext context, string group, bool enabled);
    public Alias? GetAlias(WorldContext context, string id);
    public List<Alias> GetAliasesByType(WorldContext context, bool byUser);
    public void DoDeleteAliasByType(WorldContext context, bool byUser);
    public void AddAliases(WorldContext context, List<Alias> aliases);
    public FoundStringResult GetAliasOption(WorldContext context, string name, string option);
    public FoundStringResult GetAliasInfo(WorldContext context, string name, int infotype);
    public FoundBoolResult SetAliasOption(WorldContext context, string name, string option, string value);
    public bool HasNamedAlias(WorldContext context, string name);
    public List<string> DoListAliasNames(WorldContext context, bool byUser);
    public bool AddAlias(WorldContext context, Alias alias, bool byUser);
    public int DoUpdateAlias(WorldContext context, Alias alias);
    public bool DoDeleteTrigger(WorldContext context, string id);
    public bool DoDeleteTriggerByName(WorldContext context, string name);
    public int DoDeleteTemporaryTriggers(WorldContext context);
    public int DoDeleteTriggerGroup(WorldContext context, string group, bool byUser);
    public bool DoEnableTriggerByName(WorldContext context, string name, bool enabled);
    public int DoEnableTriggerGroup(WorldContext context, string group, bool enabled);
    public Trigger? GetTrigger(WorldContext context, string name);
    public List<Trigger> GetTriggersByType(WorldContext context, bool byUser);
    public void DoDeleteTriggerByType(WorldContext context, bool byUser);
    public void AddTriggers(WorldContext context, List<Trigger> triggers);
    public FoundStringResult GetTriggerOption(WorldContext context, string name, string option);
    public FoundStringResult GetTriggerInfo(WorldContext context, string name, int infotype);
    public FoundBoolResult SetTriggerOption(WorldContext context, string name, string option, string value);
    public bool HasNamedTrigger(WorldContext context, string name);
    public List<string> DoListTriggerNames(WorldContext context, bool byUser);
    public bool AddTrigger(WorldContext context, Trigger trigger, bool byUser);
    public int DoUpdateTrigger(WorldContext context, Trigger trigger);
    public void DoExecute(WorldContext context, string cmd);
    public void DoMultiLinesFlush(WorldContext context);
}
// 自动机服务
// 用于管理Mud中的触发器/计时器/别名等自动化任务工作
public class AutomationService : IAutomationService
{
    public IInfoService InfoService { get; set; } = new InfoService();
    public ILogService LogService { get; set; } = new LogService();
    public IMetronomeService MetronomeService { get; set; } = new MetronomeService();
    public IConvertService ConvertService { get; set; } = new ConvertService();
    public IQueueService QueueService { get; set; } = new QueueService();
    public IScriptService ScriptService { get; set; } = new ScriptService();

    public IConfigService ConfigService { get; set; } = new ConfigService();
    public void InstallTo(WorldContext context)
    {
        context.EventBus.LineEvent += (_, line) => OnLine(context, line);
        context.EventBus.CloseEvent += (_, _) => OnClose(context);
        context.Automation.Timers.OnFire += (_, timer) => OnTimer(context, timer);
        // Install automation service to the world context
    }
    private void OnClose(WorldContext context)
    {
        context.Automation.Timers.Flush();
    }
    public void OnTimer(WorldContext context, Timer timer)
    {
        context.Lock.Wait();
        try
        {
            ScriptService.SendTimer(context, timer);
        }
        finally
        {
            context.Lock.Release();
        }
    }
    public void OnLine(WorldContext context, Line? line)
    {
        if (line is null || line.Type != Line.LineTypeReal)
        {
            return;
        }
        var text = line.ToPlainText();
        context.Automation.ReadyForLine();
        context.Automation.MultiLinesAppend(text);
        var queue = context.Automation.Triggers.Queue();
        var trictx = new TriggerContext(text, context.Config.Data.Params);
        for (int i = 0; i < queue.Count; i++)
        {
            var v = queue[i];
            MatchResult? r;
            try
            {
                r = v.Match(trictx, context.Automation.MultiLines);
            }
            catch (Exception ex)
            {
                LogService.HandleTriggerError(context, ex);
                continue;
            }
            if (r is null)
            {
                continue;
            }
            var rawtrigger = context.Automation.Triggers.All.TryGetValue(v.Data.ID, out var t) ? t : null;
            if (rawtrigger is not null)
            {
                rawtrigger.Wildcards = r;
            }
            string send = "";
            Trigger data;
            data = v.Data;
            if (data.Script != "")
            {
                line.Triggers.Add(data.Script);
            }
            else
            {
                line.Triggers.Add($"#{data.ID}");
            }
            if (v.Data.Send != "")
            {
                var rl = r.ReplaceList(v.Data.Name);
                if (v.Data.ExpandVariables)
                {
                    rl.AddRange(Replacer.BuildParamsReplacer(trictx.Params));
                }
                send = Replacer.Replace(v.Data.Send, rl);
            }
            if (data.OneShot)
            {
                context.Automation.Triggers.RemoveTrigger(data.ID);
            }
            if (data.OneShot)
            {
                InfoService.OmitOutput(context);
            }
            if (data.OmitFromLog)
            {
                line.OmitFromLog = true;
            }
            if (send != "")
            {
                trySendTo(context, data.SendTo, send, data.Variable, data.OmitFromLog, data.OmitFromOutput);
            }
            if (data.Script != "")
            {
                ScriptService.SendTrigger(context, line, data, r);
            }
            if (!data.KeepEvaluating || context.Automation.EvaluatingTriggersStop())
            {
                return;
            }
        }
    }
    public MatchResult? GetTriggerWildcard(WorldContext context, string name)
    {
        return context.Automation.GetTriggerWildcard(name);
    }
    public void DoStopEvaluatingTriggers(WorldContext context)
    {
        context.Automation.DoStopEvaluatingTriggers();
    }
    public bool MatchAlias(WorldContext context, string message)
    {
        bool matched = false;
        var queue = context.Automation.Aliases.Queue();
        foreach (var v in queue)
        {
            MatchResult? r;
            try
            {
                r = v.Match(message);
            }
            catch (Exception ex)
            {
                LogService.HandleTriggerError(context, ex);
                continue;
            }
            if (r is null)
            {
                continue;
            }
            matched = true;
            var send = "";
            var data = v.Data;
            if (data.Send != "")
            {
                var rl = r.ReplaceList(data.Name);
                if (v.Data.ExpandVariables)
                {
                    rl.AddRange(Replacer.BuildParamsReplacer(context.Config.Data.Params));
                }
                send = Replacer.Replace(data.Send, rl);
            }
            if (send != "")
            {
                trySendTo(context, data.SendTo, send, data.Variable, data.OmitFromLog, data.OmitFromOutput);
            }
            if (data.Script != "")
            {
                ScriptService.SendAlias(context, message, v.Data, r);
            }
            if (data.OneShot)
            {
                context.Automation.Aliases.RemoveAlias(data.ID);
            }
            if (!data.KeepEvaluating)
            {
                return true;
            }
        }
        return matched;
    }
    public void DoExecute(WorldContext context, string cmd)
    {
        if (cmd == "")
        {
            return;
        }
        var replacers = new List<ReplacePair>()
        {
            new("\\\\","\\"),
        };
        var sep = ConfigService.GetCommandStackCharacter(context);
        if (sep != "")
        {
            replacers.Add(new($"\\{sep}", sep));
            replacers.Add(new(sep, "\n"));
        }
        var m = Replacer.Replace(cmd, replacers);
        var cmds = m.Split("\n");
        foreach (var c in cmds)
        {
            executecmd(context, c);
        }
    }
    private void executecmd(WorldContext context, string cmd)
    {
        var p = ConfigService.GetScriptPrefix(context);
        if (p != "" && cmd.StartsWith(p))
        {
            var script = cmd[p.Length..];
            ScriptService.Run(context, script);
            return;
        }
        if (!MatchAlias(context, cmd))
        {
            var c = Command.Create(cmd);
            c.History = true;
            MetronomeService.Send(context, c);
        }
    }
    private bool trySendTo(WorldContext context, int target, string message, string variable, bool omit_from_log, bool omit_from_output)
    {
        if (message == "")
        {
            return false;
        }
        switch (target)
        {
            case SendTo.SendtoWorld:
                var cmd = Command.Create(message);
                if (omit_from_log)
                {
                    cmd.Echo = false;
                }
                if (omit_from_output)
                {
                    cmd.Log = false;
                }
                MetronomeService.Send(context, cmd);
                break;
            case SendTo.SendtoCommand:
            case SendTo.SendtoOutput:
                ConvertService.DoPrint(context, message);
                break;
            case SendTo.SendtoStatus:
                //  ToDO
                break;
            case SendTo.SendtoNotepad:
            case SendTo.SendtoNotepadAppend:
            case SendTo.SendtoLogfile:
            case SendTo.SendtoNotepadReplace:
            case SendTo.SendtoCommandqueue:
                var qcmd = Command.Create(message);
                if (omit_from_log)
                {
                    qcmd.Echo = false;
                }
                if (omit_from_output)
                {
                    qcmd.Log = false;
                }
                QueueService.Append(context, qcmd);
                break;
            case SendTo.SendtoVariable:
                context.Config.Data.Params[variable] = message;
                break;
            case SendTo.SendtoExecute:
                DoExecute(context, message);
                break;

            case SendTo.SendtoSpeedwalk:
                DoExecute(context, message);
                break;
            case SendTo.SendtoScript:
                ScriptService.Run(context, message);
                break;
            case SendTo.SendtoImmediate:
                var icmd = Command.Create(message);
                if (omit_from_log)
                {
                    icmd.Echo = false;
                }
                if (omit_from_output)
                {
                    icmd.Log = false;
                }
                MetronomeService.Send(context, icmd);
                break;
            case SendTo.SendtoScriptAfterOmit:
                ScriptService.Run(context, message);
                break;
        }
        return false;
    }
    public bool AddTimer(WorldContext context, Timer timer, bool replace)
    {
        return context.Automation.Timers.AddTimer(timer, replace);
    }
    public void AddTimers(WorldContext context, List<Timer> ts)
    {
        context.Automation.Timers.AddTimers(ts);
    }

    public bool RemoveTimer(WorldContext context, string id)
    {
        return context.Automation.Timers.RemoveTimer(id);
    }
    public bool RemoveTimerByName(WorldContext context, string name)
    {
        return context.Automation.Timers.RemoveTimerByName(name);
    }
    public int DeleteTemporaryTimers(WorldContext context)
    {
        return context.Automation.Timers.DeleteTemporaryTimers();
    }
    public int DeleteTimerGroup(WorldContext context, string group, bool byUser)
    {
        return context.Automation.Timers.DeleteTimerGroup(group, byUser);
    }
    public bool EnableTimerByName(WorldContext context, string name, bool enabled)
    {
        return context.Automation.Timers.EnableTimerByName(name, enabled);
    }
    public int EnableTimerGroup(WorldContext context, string group, bool enabled)
    {
        return context.Automation.Timers.EnableTimerGroup(group, enabled);
    }
    public List<string> ListTimerNames(WorldContext context, bool byUser)
    {
        return context.Automation.Timers.ListTimerNames(byUser);
    }
    public bool HasNamedTimer(WorldContext context, string name)
    {
        return context.Automation.Timers.HasNamedTimer(name);
    }
    public bool ResetNamedTimer(WorldContext context, string name)
    {
        return context.Automation.Timers.ResetNamedTimer(name);
    }
    public void ResetTimers(WorldContext context)
    {
        context.Automation.Timers.ResetTimers();
    }
    public FoundStringResult GetTimerOption(WorldContext context, string name, string option)
    {
        return context.Automation.Timers.GetTimerOption(name, option);
    }
    public FoundStringResult GetTimerInfo(WorldContext context, string name, int infotype)
    {
        return context.Automation.Timers.GetTimerInfo(name, infotype);
    }
    public FoundBoolResult SetTimerOption(WorldContext context, string name, string option, string value)
    {
        return context.Automation.Timers.SetTimerOption(name, option, value);
    }
    public List<Timer> GetTimersByType(WorldContext context, bool byuser)
    {
        return context.Automation.Timers.GetTimersByType(byuser);
    }
    public Timer? GetTimer(WorldContext context, string id)
    {
        return context.Automation.Timers.GetTimer(id);
    }
    public void DoDeleteTimerByType(WorldContext context, bool byuser)
    {
        context.Automation.Timers.DoDeleteTimerByType(byuser);
    }
    public int DoUpdateTimer(WorldContext context, Timer ti)
    {
        return context.Automation.Timers.DoUpdateTimer(ti);
    }

    public bool DoDeleteAlias(WorldContext context, string id)
    {
        return context.Automation.Aliases.RemoveAlias(id);
    }
    public bool DoDeleteAliasByName(WorldContext context, string name)
    {
        return context.Automation.Aliases.DoDeleteAliasByName(name);
    }
    public int DoDeleteTemporaryAliases(WorldContext context)
    {
        return context.Automation.Aliases.DoDeleteTemporaryAliases();
    }
    public int DoDeleteAliasGroup(WorldContext context, string group, bool byUser)
    {
        return context.Automation.Aliases.DoDeleteAliasGroup(group, byUser);
    }
    public bool DoEnableAliasByName(WorldContext context, string name, bool enabled)
    {
        return context.Automation.Aliases.DoEnableAliasByName(name, enabled);
    }
    public int DoEnableAliasGroup(WorldContext context, string group, bool enabled)
    {
        return context.Automation.Aliases.DoEnableAliasGroup(group, enabled);
    }
    public Alias? GetAlias(WorldContext context, string id)
    {
        return context.Automation.Aliases.GetAlias(id);
    }
    public List<Alias> GetAliasesByType(WorldContext context, bool byUser)
    {
        return context.Automation.Aliases.GetAliasesByType(byUser);
    }
    public void DoDeleteAliasByType(WorldContext context, bool byUser)
    {
        context.Automation.Aliases.DoDeleteAliasByType(byUser);
    }
    public void AddAliases(WorldContext context, List<Alias> aliases)
    {
        context.Automation.Aliases.AddAliases(aliases);
    }
    public FoundStringResult GetAliasOption(WorldContext context, string name, string option)
    {
        return context.Automation.Aliases.GetAliasOption(name, option);
    }
    public FoundStringResult GetAliasInfo(WorldContext context, string name, int infotype)
    {
        return context.Automation.Aliases.GetAliasInfo(name, infotype);
    }
    public FoundBoolResult SetAliasOption(WorldContext context, string name, string option, string value)
    {
        return context.Automation.Aliases.SetAliasOption(name, option, value);
    }
    public bool HasNamedAlias(WorldContext context, string name)
    {
        return context.Automation.Aliases.HasNamedAlias(name);
    }
    public List<string> DoListAliasNames(WorldContext context, bool byUser)
    {
        return context.Automation.Aliases.DoListAliasNames(byUser);
    }
    public bool AddAlias(WorldContext context, Alias alias, bool byUser)
    {
        return context.Automation.Aliases.AddAlias(alias, byUser);
    }
    public int DoUpdateAlias(WorldContext context, Alias alias)
    {
        return context.Automation.Aliases.DoUpdateAlias(alias);
    }

    public bool DoDeleteTrigger(WorldContext context, string id)
    {
        return context.Automation.Triggers.RemoveTrigger(id);
    }
    public bool DoDeleteTriggerByName(WorldContext context, string name)
    {
        return context.Automation.Triggers.DoDeleteTriggerByName(name);
    }
    public int DoDeleteTemporaryTriggers(WorldContext context)
    {
        return context.Automation.Triggers.DoDeleteTemporaryTriggers();
    }
    public int DoDeleteTriggerGroup(WorldContext context, string group, bool byUser)
    {
        return context.Automation.Triggers.DoDeleteTriggerGroup(group, byUser);
    }
    public bool DoEnableTriggerByName(WorldContext context, string name, bool enabled)
    {
        return context.Automation.Triggers.DoEnableTriggerByName(name, enabled);
    }
    public int DoEnableTriggerGroup(WorldContext context, string group, bool enabled)
    {
        return context.Automation.Triggers.DoEnableTriggerGroup(group, enabled);
    }
    public Trigger? GetTrigger(WorldContext context, string id)
    {
        return context.Automation.Triggers.GetTrigger(id);
    }
    public List<Trigger> GetTriggersByType(WorldContext context, bool byuser)
    {
        return context.Automation.Triggers.GetTriggersByType(byuser);
    }
    public void DoDeleteTriggerByType(WorldContext context, bool byuser)
    {
        context.Automation.Triggers.DoDeleteTriggerByType(byuser);
    }
    public void AddTriggers(WorldContext context, List<Trigger> triggers)
    {
        context.Automation.Triggers.AddTriggers(triggers);
    }
    public FoundStringResult GetTriggerOption(WorldContext context, string name, string option)
    {
        return context.Automation.Triggers.GetTriggerOption(name, option);
    }
    public FoundStringResult GetTriggerInfo(WorldContext context, string name, int infotype)
    {
        return context.Automation.Triggers.GetTriggerInfo(name, infotype);
    }
    public FoundBoolResult SetTriggerOption(WorldContext context, string name, string option, string value)
    {
        return context.Automation.Triggers.SetTriggerOption(name, option, value);
    }
    public bool HasNamedTrigger(WorldContext context, string name)
    {
        return context.Automation.Triggers.HasNamedTrigger(name);
    }
    public List<string> DoListTriggerNames(WorldContext context, bool byUser)
    {
        return context.Automation.Triggers.DoListTriggerNames(byUser);
    }
    public bool AddTrigger(WorldContext context, Trigger trigger, bool byUser)
    {
        return context.Automation.Triggers.AddTrigger(trigger, byUser);
    }
    public int DoUpdateTrigger(WorldContext context, Trigger trigger)
    {
        return context.Automation.Triggers.DoUpdateTrigger(trigger);
    }
    public void DoMultiLinesFlush(WorldContext context)
    {
        context.Automation.MultiLines.Flush();
    }
    public List<string> DoMultiLinesLast(WorldContext context, int count)
    {
        return context.Automation.MultiLines.GetRecentItems(count);
    }
}