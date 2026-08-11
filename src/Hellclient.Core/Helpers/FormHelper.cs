using Hellclient.Core.Types;
using Hellclient.Core.Types.Forms;
using Hellclient.World.Cores;
using Hellclient.World.Types;
using Hellclient.World.Utils;
using Timer = Hellclient.World.Types.Timer;

namespace Hellclient.Core.Helpers;

public class FormHelper
{
    public static List<FieldError> ValidateCreateAliasForm(CreateAliasForm form)
    {
        var result = new List<FieldError>();
        if (form.SendTo < SendTo.SendToMin || form.SendTo > SendTo.SendToMax)
        {
            result.Add(new FieldError()
            {
                Field = "SendTo",
                Label = "发送到",
                Msg = "发送到无效",
            });
        }
        if (form.Match == "")
        {
            result.Add(new FieldError()
            {
                Field = "Match",
                Label = "别名",
                Msg = "别名不能为空",
            });
        }
        return result;
    }
    public static Alias CreateAliasFromForm(CreateAliasForm form)
    {
        var result = new Alias()
        {
            ID = form.ID,
            Name = form.Name,
            Enabled = form.Enabled,
            Match = form.Match,
            Send = form.Send,
            Script = form.Script,
            SendTo = form.SendTo,
            Sequence = form.Sequence,
            ExpandVariables = form.ExpandVariables,
            Temporary = form.Temporary,
            OneShot = form.OneShot,
            Regexp = form.Regexp,
            Group = form.Group,
            Variable = form.Variable,
            IgnoreCase = form.IgnoreCase,
            KeepEvaluating = form.KeepEvaluating,
            Menu = form.Menu,
            OmitFromLog = form.OmitFromLog,
            ReverseSpeedwalk = form.ReverseSpeedwalk,
            OmitFromOutput = form.OmitFromOutput
        };
        result.SetByUser(form.ByUser);
        return result;
    }
    public static List<FieldError> CreateAliasFailErrors = new List<FieldError>()
    {
        new ()
        {
            Field = "Name",
            Label = "名称",
            Msg = "添加失败",
        }
    };
    public static List<FieldError> ValidateCreateGameForm(CreateGameForm form)
    {
        var result = new List<FieldError>();
        if (form.ID.Count() < 2)
        {
            result.Add(new FieldError()
            {
                Field = "ID",
                Label = "ID",
                Msg = "名称至少需要2个字符",
            });
        }
        if (form.ID.Count() > 64)
        {
            result.Add(new FieldError()
            {
                Field = "ID",
                Label = "ID",
                Msg = "名称不能超过64个字符",
            });
        }
        if (IDRegexp.MatchString(form.ID) == false)
        {
            result.Add(new FieldError()
            {
                Field = "ID",
                Label = "ID",
                Msg = "名称只能包含数字，字母，- _ @ .()[]+",
            });
        }
        if (form.Host.Trim() == "")
        {
            result.Add(new FieldError()
            {
                Field = "Host",
                Label = "Host",
                Msg = "网址不能为空",
            });
        }
        if (form.Port.Trim() == "")
        {
            result.Add(new FieldError()
            {
                Field = "Port",
                Label = "Port",
                Msg = "端口不能为空",
            });
        }
        if (form.Charset.Trim() == "")
        {
            result.Add(new FieldError()
            {
                Field = "Charset",
                Label = "Charset",
                Msg = "字符编码不能为空",
            });
        }
        return result;
    }
    public static List<FieldError> CreateGameFailErrors = new List<FieldError>()
    {
        new ()
        {
            Field = "ID",
            Label = "ID",
            Msg = "名称已经存在",
        }
    };
    public static List<FieldError> ValidateCreateScriptForm(CreateScriptForm form)
    {
        var result = new List<FieldError>();
        if (form.ID.Count() < 2)
        {
            result.Add(new FieldError()
            {
                Field = "ID",
                Label = "ID",
                Msg = "名称至少需要2个字符",
            });
        }
        if (form.ID.Count() > 64)
        {
            result.Add(new FieldError()
            {
                Field = "ID",
                Label = "ID",
                Msg = "名称不能超过64个字符",
            });
        }
        if (IDRegexp.MatchString(form.ID) == false)
        {
            result.Add(new FieldError()
            {
                Field = "ID",
                Label = "ID",
                Msg = "名称只能包含数字，字母，- _ @ .()[]+",
            });
        }
        if (!ScriptEngineFactoryManager.HasScriptEngine(form.Type))
        {
            result.Add(new FieldError()
            {
                Field = "Type",
                Label = "类型",
                Msg = "类型无效",
            });
        }
        return result;
    }
    public static List<FieldError> CreateScriptFailErrors = new List<FieldError>()
    {
        new ()
        {
            Field = "ID",
            Label = "ID",
            Msg = "脚本已经存在",
        }
    };
    public static List<FieldError> ValidateCreateTimerForm(CreateTimerForm form)
    {
        var result = new List<FieldError>();
        if (form.SendTo < SendTo.SendToMin || form.SendTo > SendTo.SendToMax)
        {
            result.Add(new FieldError()
            {
                Field = "SendTo",
                Label = "发送到",
                Msg = "发送到无效",
            });
        }
        if (form.Hour == 0 && form.Minute == 0 && form.Second == 0 && form.AtTime == false)
        {
            result.Add(new FieldError()
            {
                Field = "Second",
                Label = "时间",
                Msg = "时间无效",
            });
        }
        if (result.Count == 0)
        {
            if (form.Name != "" && IDRegexp.MatchString(form.Name) == false)
            {
                result.Add(new FieldError()
                {
                    Field = "Name",
                    Label = "名称",
                    Msg = "名称不可用",
                });
            }
        }
        return result;
    }
    public static Timer CreateTimerFromForm(CreateTimerForm form)
    {
        var result = new Timer()
        {
            ID = form.Name,
            Enabled = form.Enabled,
            Hour = form.Hour,
            Minute = form.Minute,
            Second = form.Second,
            SendTo = form.SendTo,
            Send = form.Send,
            Script = form.Script,
            Group = form.Group,
            Variable = form.Variable,
            AtTime = form.AtTime,
            ActionWhenDisconnectd = form.ActionWhenDisconnectd,
            OneShot = form.OneShot,
            Temporary = form.Temporary,
            OmitFromOutput = form.OmitFromOutput,
            OmitFromLog = form.OmitFromLog
        };
        result.SetByUser(form.ByUser);
        return result;
    }
    public static List<FieldError> CreateTimerFailErrors = new List<FieldError>()
    {
        new ()
        {
            Field = "Name",
            Label = "Name",
            Msg = "名称已经存在",
        }
    };
    public static List<FieldError> ValidateCreateTriggerForm(CreateTriggerForm form)
    {
        var result = new List<FieldError>();
        if (form.SendTo < SendTo.SendToMin || form.SendTo > SendTo.SendToMax)
        {
            result.Add(new FieldError()
            {
                Field = "SendTo",
                Label = "发送到",
                Msg = "发送到无效",
            });
        }
        if (form.Match == "")
        {
            result.Add(new FieldError()
            {
                Field = "Match",
                Label = "匹配",
                Msg = "匹配不能为空",
            });
        }
        if (form.Name != "" && !IDRegexp.MatchString(form.Name))
        {
            result.Add(new FieldError()
            {
                Field = "Name",
                Label = "名称",
                Msg = "名称不可用",
            });
        }
        return result;
    }
    public static Trigger CreateTriggerFromForm(CreateTriggerForm form)
    {
        var result = new Trigger()
        {
            ID = form.ID,
            Name = form.Name,
            Enabled = form.Enabled,
            Match = form.Match,
            Send = form.Send,
            Script = form.Script,
            SendTo = form.SendTo,
            Sequence = form.Sequence,
            ExpandVariables = form.ExpandVariables,
            Temporary = form.Temporary,
            OneShot = form.OneShot,
            Regexp = form.Regexp,
            Group = form.Group,
            Variable = form.Variable,
            IgnoreCase = form.IgnoreCase,
            KeepEvaluating = form.KeepEvaluating,
            OmitFromLog = form.OmitFromLog,
            OmitFromOutput = form.OmitFromOutput,
            MultiLine = form.MultiLine,
            Repeat = form.Repeat,
            LinesToMatch = form.LinesToMatch,
            WildcardLowerCase = form.WildcardLowerCase
        };
        result.SetByUser(form.ByUser);
        return result;
    }
    public static List<FieldError> CreateTriggerFailErrors = new List<FieldError>()
    {
        new ()
        {
            Field = "Name",
            Label = "名称",
            Msg = "名称不可用",
        }
    };
}