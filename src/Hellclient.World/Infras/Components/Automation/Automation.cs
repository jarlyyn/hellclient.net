using Hellclient.World.Types;

namespace Hellclient.World.Components.Automation;

public class Automation
{
    public static int MaxMultiLines = 200;
    public Timers Timers { get; set; } = new Timers();
    public Triggers Triggers { get; set; } = new Triggers();
    public Aliases Aliases { get; set; } = new Aliases();
    public Ring<string> MultiLines { get; set; } = new(MaxMultiLines);
    private bool evaluatingTriggersStop { get; set; } = false;

    public void Dispose()
    {
        Timers.Flush();
    }
    public void DoStopEvaluatingTriggers()
    {
        evaluatingTriggersStop = true;
    }

    public bool EvaluatingTriggersStop()
    {
        return evaluatingTriggersStop;
    }
    public void ReadyForLine()
    {
        evaluatingTriggersStop = false;
    }
    public void MultiLinesAppend(string line)
    {
        MultiLines.Add(line);
    }
    public MatchResult? GetTriggerWildcard(string name)
    {
        var trigger = Triggers.Named.TryGetValue(name, out var t) ? t : null;
        if (trigger == null)
        {
            return null;
        }
        return trigger.Wildcards;
    }
}