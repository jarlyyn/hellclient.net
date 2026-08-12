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
        var result = Alias.Create();
        result.Name = form.Name;
        result.Enabled = form.Enabled;
        result.Match = form.Match;
        result.Send = form.Send;
        result.Script = form.Script;
        result.SendTo = form.SendTo;
        result.Sequence = form.Sequence;
        result.ExpandVariables = form.ExpandVariables;
        result.Temporary = form.Temporary;
        result.OneShot = form.OneShot;
        result.Regexp = form.Regexp;
        result.Group = form.Group;
        result.Variable = form.Variable;
        result.IgnoreCase = form.IgnoreCase;
        result.KeepEvaluating = form.KeepEvaluating;
        result.Menu = form.Menu;
        result.OmitFromLog = form.OmitFromLog;
        result.ReverseSpeedwalk = form.ReverseSpeedwalk;
        result.OmitFromOutput = form.OmitFromOutput;
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
        var result = Timer.Create();
        result.Name = form.Name;
        result.Enabled = form.Enabled;
        result.Hour = form.Hour;
        result.Minute = form.Minute;
        result.Second = form.Second;
        result.SendTo = form.SendTo;
        result.Send = form.Send;
        result.Script = form.Script;
        result.Group = form.Group;
        result.Variable = form.Variable;
        result.AtTime = form.AtTime;
        result.ActionWhenDisconnectd = form.ActionWhenDisconnectd;
        result.OneShot = form.OneShot;
        result.Temporary = form.Temporary;
        result.OmitFromOutput = form.OmitFromOutput;
        result.OmitFromLog = form.OmitFromLog;
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
        var result = Trigger.Create();
        result.Name = form.Name;
        result.Enabled = form.Enabled;
        result.Match = form.Match;
        result.Send = form.Send;
        result.Script = form.Script;
        result.SendTo = form.SendTo;
        result.Sequence = form.Sequence;
        result.ExpandVariables = form.ExpandVariables;
        result.Temporary = form.Temporary;
        result.OneShot = form.OneShot;
        result.Regexp = form.Regexp;
        result.Group = form.Group;
        result.Variable = form.Variable;
        result.IgnoreCase = form.IgnoreCase;
        result.KeepEvaluating = form.KeepEvaluating;
        result.OmitFromLog = form.OmitFromLog;
        result.OmitFromOutput = form.OmitFromOutput;
        result.MultiLine = form.MultiLine;
        result.Repeat = form.Repeat;
        result.LinesToMatch = form.LinesToMatch;
        result.WildcardLowerCase = form.WildcardLowerCase;
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
    public static List<FieldError> ValidateUpdateAliasForm(UpdateAliasForm form)
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
    public static void UpdateAliasFromForm(Alias alias, UpdateAliasForm form)
    {
        alias.ID = form.ID;
        alias.Name = form.Name;
        alias.Enabled = form.Enabled;
        alias.Match = form.Match;
        alias.Send = form.Send;
        alias.Script = form.Script;
        alias.SendTo = form.SendTo;
        alias.Sequence = form.Sequence;
        alias.ExpandVariables = form.ExpandVariables;
        alias.Temporary = form.Temporary;
        alias.OneShot = form.OneShot;
        alias.Regexp = form.Regexp;
        alias.Group = form.Group;
        alias.Variable = form.Variable;
        alias.IgnoreCase = form.IgnoreCase;
        alias.KeepEvaluating = form.KeepEvaluating;
        alias.Menu = form.Menu;
        alias.OmitFromLog = form.OmitFromLog;
        alias.ReverseSpeedwalk = form.ReverseSpeedwalk;
        alias.OmitFromOutput = form.OmitFromOutput;
    }
    public static List<FieldError> UpdateAliasDuplicateErrors = new List<FieldError>()
    {
        new ()
        {
            Field = "Name",
            Label = "名称",
            Msg = "名称重复",
        }
    };
    public static List<FieldError> UpdateAliasNotFoundErrors = new List<FieldError>()
    {
        new ()
        {
            Field = "ID",
            Label = "ID",
            Msg = "未找到",
        }
    };
    public static List<FieldError> ValidateUpdateTimerForm(UpdateTimerForm form)
    {
        var result = new List<FieldError>();
        if (form.ID == "")
        {
            result.Add(new FieldError()
            {
                Field = "ID",
                Label = "ID",
                Msg = "无效的ID",
            });
        }
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
    public static void UpdateTimerFromForm(Timer timer, UpdateTimerForm form)
    {
        timer.ID = form.ID;
        timer.Hour = form.Hour;
        timer.Minute = form.Minute;
        timer.Second = form.Second;
        timer.Name = form.Name;
        timer.SendTo = form.SendTo;
        timer.Send = form.Send;
        timer.Script = form.Script;
        timer.Group = form.Group;
        timer.Variable = form.Variable;
        timer.AtTime = form.AtTime;
        timer.Enabled = form.Enabled;
        timer.ActionWhenDisconnectd = form.ActionWhenDisconnectd;
        timer.OneShot = form.OneShot;
        timer.Temporary = form.Temporary;
        timer.OmitFromOutput = form.OmitFromOutput;
        timer.OmitFromLog = form.OmitFromLog;
    }
    public static List<FieldError> UpdateTimerNotFoundErrors = new List<FieldError>()
    {
        new ()
        {
            Field = "ID",
            Label = "ID",
            Msg = "未找到",
        }
    };
    public static List<FieldError> UpdateTimerDuplicateErrors = new List<FieldError>()
    {
        new ()
        {
            Field = "Name",
            Label = "名称",
            Msg = "名称重复",
        }
    };
    public static List<FieldError> ValidateUpdateTriggerForm(UpdateTriggerForm form)
    {
        var result = new List<FieldError>();
        if (form.ID == "")
        {
            result.Add(new FieldError()
            {
                Field = "ID",
                Label = "ID",
                Msg = "无效的ID",
            });
        }
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
    public static void UpdateTriggerFromForm(Trigger trigger, UpdateTriggerForm form)
    {
        trigger.ID = form.ID;
        trigger.Name = form.Name;
        trigger.Enabled = form.Enabled;
        trigger.Match = form.Match;
        trigger.Send = form.Send;
        trigger.Script = form.Script;
        trigger.SendTo = form.SendTo;
        trigger.Sequence = form.Sequence;
        trigger.ExpandVariables = form.ExpandVariables;
        trigger.Temporary = form.Temporary;
        trigger.OneShot = form.OneShot;
        trigger.Regexp = form.Regexp;
        trigger.Group = form.Group;
        trigger.Variable = form.Variable;
        trigger.IgnoreCase = form.IgnoreCase;
        trigger.KeepEvaluating = form.KeepEvaluating;
        trigger.OmitFromLog = form.OmitFromLog;
        trigger.OmitFromOutput = form.OmitFromOutput;
        trigger.MultiLine = form.MultiLine;
        trigger.Repeat = form.Repeat;
        trigger.LinesToMatch = form.LinesToMatch;
        trigger.WildcardLowerCase = form.WildcardLowerCase;
    }
    public static List<FieldError> UpdateTriggerNotFoundErrors = new List<FieldError>()
    {
        new ()
        {
            Field = "ID",
            Label = "ID",
            Msg = "未找到",
        }
    };
    public static List<FieldError> UpdateTriggerDuplicateErrors = new List<FieldError>()
    {
        new ()
        {
            Field = "Name",
            Label = "名称",
            Msg = "名称重复",
        }
    };
    public static List<FieldError> ValidateUpdateGameForm(UpdateGameForm form)
    {
        var result = new List<FieldError>();
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
    public static void UpdateGameFromForm(IWorld world, UpdateGameForm form)
    {
        world.SetName(form.Name);
        world.SetHost(form.Host);
        world.SetPort(form.Port);
        world.SetCharset(form.Charset);
        world.SetScriptPrefix(form.ScriptPrefix);
        world.SetCommandStackCharacter(form.CommandStackCharacter);
        world.SetProxy(form.Proxy);
        world.SetShowBroadcast(form.ShowBroadcast);
        world.SetShowSubneg(form.ShowSubneg);
        world.SetModEnabled(form.ModEnabled);
        world.SetAutoSave(form.AutoSave);
        world.SetIgnoreBatchCommand(form.IgnoreBatchCommand);
    }
    public static List<FieldError> ValidateUpdateScriptForm(UpdateScriptForm form)
    {
        var result = new List<FieldError>();
        return result;
    }
    public static void UpdateScriptFromForm(ScriptData script, UpdateScriptForm form)
    {
        script.Type = form.Type;
        script.Intro = form.Intro;
        script.Desc = form.Desc;
        script.OnOpen = form.OnOpen;
        script.OnClose = form.OnClose;
        script.OnConnect = form.OnConnect;
        script.OnDisconnect = form.OnDisconnect;
        script.OnBroadcast = form.OnBroadcast;
        script.OnResponse = form.OnResponse;
        script.OnHUDClick = form.OnHUDClick;
        script.OnAssist = form.OnAssist;
        script.OnBuffer = form.OnBuffer;
        script.OnKeyUp = form.OnKeyUp;
        script.OnBufferMin = form.OnBufferMin;
        script.OnBufferMax = form.OnBufferMax;
        script.OnSubneg = form.OnSubneg;
        script.OnFocus = form.OnFocus;
        script.OnLoseFocus = form.OnLoseFocus;
        script.Channel = form.Channel;
    }
    public static List<FieldError>ValidateUpdatePasswordForm(UpdatePasswordForm form)
    {
        var result = new List<FieldError>();
        if (form.Username.Trim() == "")
        {
            result.Add(new FieldError()
            {
                Field = "Username",
                Label = "用户名",
                Msg = "用户名为空",
            });
        }
        if (form.Password.Trim() == "")
        {
            result.Add(new FieldError()
            {
                Field = "Password",
                Label = "密码",
                Msg = "密码为空",
            });
        }
        if (form.Password != form.RepeatPassword)
        {
            result.Add(new FieldError()
            {
                Field = "RepeatPassword",
                Label = "重复密码",
                Msg = "密码不匹配",
            });
        }
        return result;
    }
}