using Hellclient.World.Types;
using Hellclient.World.Utils;

namespace Hellclient.World.Components.Automation;


public class AutomationTrigger
{
    public bool Deleted;
    public Trigger Data { get; set; }=new Trigger();
    public IMatcher? Matcher { get; set; }
    private string RawMatch { get; set; } = "";
    public  MatchResult? Wildcards { get; set; }
    public StringResult Option(string name)
    {
        return name switch
        {
            "clipboard_arg" => new StringResult("0", true),
            "colour_change_type" => new StringResult(Data.ColourChangeType.ToString(), true),
            "custom_colour" => new StringResult(Data.Colour.ToString(), true),
            "enabled" => new StringResult(MushString.ToStringBool(Data.Enabled), true),
            "expand_variables" => new StringResult(MushString.ToStringBool(Data.ExpandVariables), true),
            "group" => new StringResult(Data.Group, true),
            "ignore_case" => new StringResult(MushString.ToStringBool(Data.IgnoreCase), true),
            "inverse" => new StringResult(MushString.ToStringBool(Data.Inverse), true),
            "italic" => new StringResult(MushString.ToStringBool(Data.Italic), true),
            "keep_evaluating" => new StringResult(MushString.ToStringBool(Data.KeepEvaluating), true),
            "lines_to_match" => new StringResult(Data.LinesToMatch.ToString(), true),
            "lowercase_wildcard" => new StringResult(MushString.ToStringBool(Data.WildcardLowerCase), true),
            "match" => new StringResult(Data.Match, true),
            "match_style" => new StringResult("0", true),
            "multi_line" => new StringResult(MushString.ToStringBool(Data.MultiLine), true),
            "name" => new StringResult(Data.Name, true),
            "new_style" => new StringResult("0", true),
            "omit_from_log" => new StringResult(MushString.ToStringBool(Data.OmitFromLog), true),
            "omit_from_output" => new StringResult(MushString.ToStringBool(Data.OmitFromOutput), true),
            "one_shot" => new StringResult(MushString.ToStringBool(Data.OneShot), true),
            "other_back_colour" => new StringResult("0", true),
            "other_text_colour" => new StringResult("0", true),
            "regexp" => new StringResult(MushString.ToStringBool(Data.Regexp), true),
            "repeat" => new StringResult(MushString.ToStringBool(Data.Repeat), true),
            "script" => new StringResult(Data.Script, true),
            "send" => new StringResult(Data.Send, true),
            "send_to" => new StringResult(Data.SendTo.ToString(), true),
            "sequence" => new StringResult(Data.Sequence.ToString(), true),
            "user" => new StringResult("0", true),
            "variable" => new StringResult(Data.Variable, true),
            _ => new StringResult(string.Empty, false),
        };
    }

    public StringResult Info(int infotype)
    {
        return infotype switch
        {
            1 => new StringResult(Data.Match, true),
            2 => new StringResult(Data.Send, true),
            3 => new StringResult(Data.SoundFileName, true),
            4 => new StringResult(Data.Script, true),
            5 => new StringResult(MushString.ToStringBool(Data.OmitFromLog), true),
            6 => new StringResult(MushString.ToStringBool(Data.OmitFromOutput), true),
            7 => new StringResult(MushString.ToStringBool(Data.KeepEvaluating), true),
            8 => new StringResult(MushString.ToStringBool(Data.Enabled), true),
            9 => new StringResult(MushString.ToStringBool(Data.Regexp), true),
            10 => new StringResult(MushString.ToStringBool(Data.IgnoreCase), true),
            11 => new StringResult(MushString.ToStringBool(Data.Repeat), true),
            13 => new StringResult(MushString.ToStringBool(Data.ExpandVariables), true),
            15 => new StringResult(Data.SendTo.ToString(), true),
            16 => new StringResult(Data.Sequence.ToString(), true),
            23 => new StringResult(MushString.ToStringBool(Data.Temporary), true),
            25 => new StringResult(MushString.ToStringBool(Data.WildcardLowerCase), true),
            26 => new StringResult(Data.Group, true),
            27 => new StringResult(Data.Variable, true),
            28 => new StringResult("0", true),
            31 => new StringResult(Wildcards == null ? "0" : Wildcards.List.Count.ToString(), true),
            36 => new StringResult(MushString.ToStringBool(Data.OneShot), true),
            _ => new StringResult(string.Empty, false),
        };
    }

    public BoolResult SetOption(string name, string val)
    {
        return name switch
        {
            "clipboard_arg" => new BoolResult(true, true),
            "colour_change_type" => new BoolResult(true, true),
            "custom_colour" => new BoolResult(true, true),
            "enabled" => SetBool(() => Data.Enabled = MushString.FromStringBool(val)),
            "expand_variables" => SetBool(() => Data.ExpandVariables = MushString.FromStringBool(val)),
            "group" => SetString(() => Data.Group = val),
            "ignore_case" => SetBool(() => Data.IgnoreCase = MushString.FromStringBool(val)),
            "inverse" => SetBool(() => Data.Inverse = MushString.FromStringBool(val)),
            "italic" => SetBool(() => Data.Italic = MushString.FromStringBool(val)),
            "lines_to_match" => SetInt(() => Data.LinesToMatch = MushString.FromStringInt(val)),
            "lowercase_wildcard" => SetBool(() => Data.WildcardLowerCase = MushString.FromStringBool(val)),
            "match" => SetString(() => Data.Match = val),
            "match_style" => new BoolResult(true, true),
            "multi_line" => SetBool(() => Data.MultiLine = MushString.FromStringBool(val)),
            "name" => SetString(() => Data.Name = val),
            "new_style" => new BoolResult(true, true),
            "omit_from_log" => SetBool(() => Data.OmitFromLog = MushString.FromStringBool(val)),
            "omit_from_output" => SetBool(() => Data.OmitFromOutput = MushString.FromStringBool(val)),
            "one_shot" => SetBool(() => Data.OneShot = MushString.FromStringBool(val)),
            "other_back_colour" => new BoolResult(true, true),
            "other_text_colour" => new BoolResult(true, true),
            "send" => SetString(() => Data.Send = val),
            "regexp" => SetBool(() => Data.Regexp = MushString.FromStringBool(val)),
            "repeat" => SetBool(() => Data.Repeat = MushString.FromStringBool(val)),
            "script" => SetString(() => Data.Script = val),
            "send_to" => SetInt(() => Data.SendTo = MushString.FromStringInt(val)),
            "sequence" => SetInt(() => Data.Sequence = MushString.FromStringInt(val)),
            "sound" => new BoolResult(true, true),
            "sound_if_inactive" => new BoolResult(true, true),
            "user" => new BoolResult(false, false),
            "variable" => SetString(() => Data.Variable = val),
            _ => new BoolResult(false, false),
        };
    }

    private static BoolResult SetBool(Action action)
    {
        action();
        return new BoolResult(true, true);
    }

    private static BoolResult SetInt(Action action)
    {
        action();
        return new BoolResult(true, true);
    }

    private static BoolResult SetString(Action action)
    {
        action();
        return new BoolResult(true, true);
    }
    public void BuildMatcher(string match)
    {
        Matcher = MatcherBuilder.Build(match, Data.Regexp, Data.IgnoreCase);
    }
    public MatchResult? Match(TriggerContext context, Ring<string> lines)
    {
        if (Deleted || !Data.Enabled)
        {
            return null;
        }
        if (Data.ExpandVariables)
        {
            if (context.Expanded is null)
            {
                context.Expanded = Replacer.BuildParamsTriggerReplacer(context.Params);
            }
            var match = Replacer.Replace(Data.Match, context.Expanded);
            if (RawMatch == "" || RawMatch != match)
            {
                BuildMatcher(match);
                RawMatch = match;
            }
        }
        else
        {
            if (Matcher is null)
            {
                BuildMatcher(Data.Match);
            }
        }
        string line;
        if (Data.MultiLine && Data.Regexp)
        {
            line=string.Join("\n", lines.GetRecentItems(Data.LinesToMatch));
        }
        else
        {
            line=context.Line;
        }
        return Matcher!.Match(line);
    }
    public AutomationTrigger Clone()
    {
        return new AutomationTrigger()
        {
            Data = this.Data,
            Matcher = this.Matcher,
            RawMatch = this.RawMatch,
            Deleted = this.Deleted,
        };
    }

}
