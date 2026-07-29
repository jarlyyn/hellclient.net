using Hellclient.World.Components.Automation;
using Hellclient.World.States;
using Hellclient.World.Types;
using Timer = Hellclient.World.Types.Timer;

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
    public IInfoService InfoService { get; set; } = new InfoService();
    public IMetronomeService MetronomeService { get; set; } = new MetronomeService();
    public IConnService ConnService { get; set; } = new ConnService();
    public IQueueService QueueService { get; set; } = new QueueService();
    public IScriptService ScriptService { get; set; } = new ScriptService();

    public IConfigService ConfigService { get; set; } = new ConfigService();
    public void InstallTo(WorldContext context)
    {
        // Install automation service to the world context
    }
    public async Task OnLine(WorldContext context, Line? line)
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
            var r = v.Match(trictx, context.Automation.MultiLines);
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
    public bool MatchAlias(WorldContext context, string message)
    {
        bool matched = false;
        var queue = context.Automation.Aliases.Queue();
        foreach (var v in queue)
        {
            var r = v.Match(message);
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
            ScriptService.DoRunScript(context, script);
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
                ConnService.DoPrint(context, message);
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
                ScriptService.DoRunScript(context, message);
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
                ScriptService.DoRunScript(context, message);
                break;
        }
        return false;
    }
    public void AddTimer(Timer timer, bool replace)
    {

    }
    public bool RemoveTimer(string id)
    {
        return false;
    }
}