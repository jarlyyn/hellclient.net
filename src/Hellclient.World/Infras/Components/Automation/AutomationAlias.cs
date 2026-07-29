using Hellclient.World.Types;
using Hellclient.World.Utils;

namespace Hellclient.World.Components.Automation;

public class AutomationAlias
{
    public required Alias Data;
    public bool Deleted;
    public IMatcher? Matcher;
    public MatchResult? Match(string message)
    {
        if (Deleted || !Data.Enabled)
        {
            return null;
        }
        if (Matcher == null)
        {
            Matcher = MatcherBuilder.Build(Data.Match, Data.Regexp, Data.IgnoreCase);

        }
        return Matcher.Match(message);
    }
    public StringResult Option(string name)
    {
        lock (this)
        {
            switch (name)
            {
                case "echo_alias":
                    return new StringResult(MushString.ToStringBool(!Data.OmitFromOutput), true);
                case "enabled":
                    return new StringResult(MushString.ToStringBool(Data.Enabled), true);
                case "expand_variables":
                    return new StringResult(MushString.ToStringBool(Data.ExpandVariables), true);
                case "group":
                    return new StringResult(Data.Group, true);
                case "ignore_case":
                    return new StringResult(MushString.ToStringBool(Data.IgnoreCase), true);
                case "keep_evaluating":
                    return new StringResult(MushString.ToStringBool(Data.KeepEvaluating), true);
                case "match":
                    return new StringResult(Data.Match, true);
                case "menu":
                    return new StringResult(MushString.ToStringBool(Data.Menu), true);
                case "name":
                    return new StringResult(Data.Name, true);
                case "offset_hour":
                    return new StringResult("0", true);
                case "offset_minute":
                    return new StringResult("0", true);
                case "offset_second":
                    return new StringResult("0", true);
                case "omit_from_command_history":
                    return new StringResult(MushString.ToStringBool(Data.OmitFromCommandHistory), true);
                case "omit_from_log":
                    return new StringResult(MushString.ToStringBool(Data.OmitFromLog), true);
                case "omit_from_output":
                    return new StringResult(MushString.ToStringBool(Data.OmitFromOutput), true);
                case "one_shot":
                    return new StringResult(MushString.ToStringBool(Data.OneShot), true);
                case "regexp":
                    return new StringResult(MushString.ToStringBool(Data.Regexp), true);
                case "script":
                    return new StringResult(Data.Script, true);
                case "send":
                    return new StringResult(Data.Send, true);
                case "send_to":
                    return new StringResult(Data.SendTo.ToString(), true);
                case "sequence":
                    return new StringResult(Data.Sequence.ToString(), true);
                case "user":
                    return new StringResult("0", true);
                case "variable":
                    return new StringResult(Data.Variable, true);
            }

            return new StringResult(string.Empty, false);
        }
    }

    public StringResult Info(int infotype)
    {
        lock (this)
        {
            switch (infotype)
            {
                case 1:
                    return new StringResult(Data.Match, true);
                case 2:
                    return new StringResult(Data.Send, true);
                case 3:
                    return new StringResult(Data.Script, true);
                case 4:
                    return new StringResult(Data.Send, true);
                case 5:
                    return new StringResult(Data.Script, true);
                case 6:
                    return new StringResult(MushString.ToStringBool(Data.Enabled), true);
                case 7:
                    return new StringResult(MushString.ToStringBool(Data.Regexp), true);
                case 8:
                    return new StringResult(MushString.ToStringBool(Data.IgnoreCase), true);
                case 9:
                    return new StringResult(MushString.ToStringBool(Data.ExpandVariables), true);
                case 14:
                    return new StringResult(MushString.ToStringBool(Data.Temporary), true);
                case 16:
                    return new StringResult(Data.Group, true);
                case 17:
                    return new StringResult(Data.Variable, true);
                case 18:
                    return new StringResult(Data.SendTo.ToString(), true);
                case 19:
                    return new StringResult(MushString.ToStringBool(Data.KeepEvaluating), true);
                case 20:
                    return new StringResult(Data.Sequence.ToString(), true);
                case 22:
                    return new StringResult(MushString.ToStringBool(Data.OmitFromCommandHistory), true);
                case 23:
                    return new StringResult(0.ToString(), true);
                case 29:
                    return new StringResult(MushString.ToStringBool(Data.OneShot), true);
            }

            return new StringResult(string.Empty, false);
        }
    }

    public BoolResult SetOption(string name, string val)
    {
        lock (this)
        {
            switch (name)
            {
                case "echo_alias":
                    Data.OmitFromOutput = !MushString.FromStringBool(val);
                    return new BoolResult(true, true);
                case "enabled":
                    Data.Enabled = MushString.FromStringBool(val);
                    return new BoolResult(true, true);
                case "expand_variables":
                    Data.ExpandVariables = MushString.FromStringBool(val);
                    return new BoolResult(true, true);
                case "ignore_case":
                    Data.IgnoreCase = MushString.FromStringBool(val);
                    return new BoolResult(true, true);
                case "keep_evaluating":
                    Data.KeepEvaluating = MushString.FromStringBool(val);
                    return new BoolResult(true, true);
                case "match":
                    Data.Match = val;
                    return new BoolResult(true, true);
                case "offset_hour":
                    return new BoolResult(false, false);
                case "offset_minute":
                    return new BoolResult(false, false);
                case "offset_second":
                    return new BoolResult(false, false);
                case "menu":
                    Data.Menu = MushString.FromStringBool(val);
                    return new BoolResult(true, true);
                case "omit_from_command_history":
                    Data.OmitFromCommandHistory = MushString.FromStringBool(val);
                    return new BoolResult(true, true);
                case "omit_from_log":
                    Data.OmitFromLog = MushString.FromStringBool(val);
                    return new BoolResult(true, true);
                case "omit_from_output":
                    Data.OmitFromOutput = MushString.FromStringBool(val);
                    return new BoolResult(true, true);
                case "one_shot":
                    Data.OneShot = MushString.FromStringBool(val);
                    return new BoolResult(true, true);
                case "regexp":
                    Data.Regexp = MushString.FromStringBool(val);
                    return new BoolResult(true, true);
                case "script":
                    Data.Script = val;
                    return new BoolResult(true, true);
                case "send":
                    Data.Send = val;
                    return new BoolResult(true, true);
                case "send_to":
                    Data.SendTo = MushString.FromStringInt(val);
                    return new BoolResult(true, true);
                case "user":
                    return new BoolResult(false, false);
                case "sequence":
                    Data.Sequence = MushString.FromStringInt(val);
                    return new BoolResult(true, true);
                case "variable":
                    Data.Variable = val;
                    return new BoolResult(true, true);
            }

            return new BoolResult(false, false);
        }
    }
    public int CompareTo(AutomationAlias other)
    {
        if (Deleted != other.Deleted)
        {
            return Deleted ? 1 : -1;
        }
        if (Data.Sequence != other.Data.Sequence)
        {
            return Data.Sequence.CompareTo(other.Data.Sequence);
        }
        if (Data.Enabled != other.Data.Enabled)
        {
            return Data.Enabled ? -1 : 1;
        }
        return Data.ID.CompareTo(other.Data.ID);
    }
}
