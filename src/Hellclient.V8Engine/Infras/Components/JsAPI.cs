using System.IO.Pipelines;
using System.Text;
using System.Text.Unicode;
using Hellclient.V8Engine.Cores;
using Hellclient.Script.Infras.Components;
using Hellclient.World.Cores;
using Hellclient.World.Utils;
using Microsoft.ClearScript.V8;
using Microsoft.ClearScript.JavaScript;

namespace Hellclient.V8Engine.Infras.Components;

public class JsAPI(ScriptAPI api, V8ScriptEngine runtime)
{
    private V8ScriptEngine _runtime { get; init; } = runtime;
    private ScriptAPI _api { get; init; } = api;
    public void Print(params object[] args)
    {

        var msg = new List<string>();
        foreach (var v in args)
        {
            if (v != null)
            {
                msg.Add(v.ToString() ?? "");
                continue;
            }
        }
        _api.Note(string.Join(" ", msg));
    }
    public static object? GetArg(object[] args, int idx)
    {
        if (idx < 0 || idx >= args.Length)
        {
            return null;
        }
        return args[idx];
    }
    public static bool HasArg(object[] args, int idx)
    {
        if (idx < 0 || idx >= args.Length)
        {
            return false;
        }
        return args[idx] is not null;
    }
    public static string ConvertString(object? arg)
    {
        return arg?.ToString() ?? "";
    }
    public static string GetStringArg(object[] args, int idx)
    {
        return ConvertString(GetArg(args, idx));
    }
    public static int ConvertInt(object? arg)
    {
        switch (arg)
        {
            case int i:
                return i;
            case double d:
                return (int)d;
            case string s when int.TryParse(s, out var i):
                return i;
            default:
                return 0;
        }
    }
    public static int GetIntArg(object[] args, int idx)
    {
        return ConvertInt(GetArg(args, idx));
    }
    public static List<string> ConvertStringArray(object? arg)
    {
        switch (arg)
        {
            case List<string> list:
                return list;
            case string s:
                return new List<string> { s };
            case object[] arr:
                return arr.Select(a => a?.ToString() ?? "").ToList();
            case IList<object> arr:
                return arr.Select(a => a?.ToString() ?? "").ToList();
            default:
                return new List<string>();
        }
    }
    public static List<string> GetStringArrayArg(object[] args, int idx)
    {
        return ConvertStringArray(GetArg(args, idx));
    }
    public static bool ConvertBool(object? arg)
    {
        switch (arg)
        {
            case bool b:
                return b;
            case string s:
                return s != "";
            case int i:
                return i != 0;
            case double d:
                return d != 0.0;
            default:
                return false;
        }
    }
    public static bool GetBoolArg(object[] args, int idx)
    {
        return ConvertBool(GetArg(args, idx));
    }
    public static double ConvertDouble(object? arg)
    {
        switch (arg)
        {
            case double d:
                return d;
            case int i:
                return (double)i;
            case string s when double.TryParse(s, out var d):
                return d;
            default:
                return 0.0;
        }
    }
    public static double GetDoubleArg(object[] args, int idx)
    {
        return ConvertDouble(GetArg(args, idx));
    }
    // public static Dictionary<string, string> ConvertStringDictionary(object? arg)
    // {
    //     var result = new Dictionary<string, string>();
    //     switch (arg)
    //     {
    //         case IJavaScriptObject dict:
    //             foreach (var key in dict.PropertyNames)
    //             {
    //                 var value = dict.GetProperty(key);
    //                 result[key] = ConvertString(value);
    //             }
    //             break;
    //     }
    //     return result;
    // }
    public object ToJsArray(List<string> list)
    {
        var result = _runtime.Script.Array();
        foreach (var v in list)
        {
            result.push(v);
        }
        return result;
    }
    public object? Request(params object[] args)
    {

        var msgtype = GetStringArg(args, 0);
        var msg = GetStringArg(args, 1);
        var id = _api.Request(msgtype, msg);
        return id;
    }
    public object? Note(params object[] args)
    {

        var info = GetStringArg(args, 0);
        _api.Note(info);
        return null;
    }
    public object? PrintSystem(params object[] args)
    {
        var info = GetStringArg(args, 0);
        _api.PrintSystem(info);
        return null;
    }
    public object? SendImmediate(params object[] args)
    {

        var info = GetStringArg(args, 0);
        return _api.SendImmediate(info);


    }
    public object? Send(params object[] args)
    {

        var info = GetStringArg(args, 0);
        var res = _api.Send(info);
        return res;
    }
    public object? Execute(params object[] args)
    {

        var info = GetStringArg(args, 0);
        return _api.Execute(info);
    }
    public object? SendNoEcho(params object[] args)
    {

        var info = GetStringArg(args, 0);
        return _api.SendNoEcho(info);
    }
    public object? GetVariable(params object[] args)
    {

        var val = _api.GetVariable(GetStringArg(args, 0));
        return val;
    }
    public object? DeleteVariable(params object[] args)
    {

        var name = GetStringArg(args, 0);


        return _api.DeleteVariable(name);
    }
    public object? SetVariable(params object[] args)
    {
        var name = GetStringArg(args, 0);
        var value = GetStringArg(args, 1);
        return _api.SetVariable(name, value);
    }
    public object? GetVariableList(params object[] args)
    {

        var list = _api.GetVariableList().ToList();


        var result = new string[list.Count];


        for (int k = 0; k < list.Count; k++)
        {
            result[k] = list[k].Value;
        }
        return result;
    }
    public object? GetVariableComment(params object[] args)
    {
        var val = _api.GetVariableComment(GetStringArg(args, 0));
        return val;
    }
    public object? SetVariableComment(params object[] args)
    {
        var name = GetStringArg(args, 0);
        var value = GetStringArg(args, 1);
        return _api.SetVariableComment(name, value);
    }
    public object? Version(params object[] args)
    {

        return _api.Version();
    }
    public object? Hash(params object[] args)
    {
        var name = GetStringArg(args, 0);
        return _api.Hash(name);
    }
    public object? Base64Encode(params object[] args)
    {
        var src = GetStringArg(args, 0);
        var ok = GetBoolArg(args, 1);
        return _api.Base64Encode(src, ok);
    }
    public object? Base64Decode(params object[] args)
    {
        var src = GetStringArg(args, 0);
        var result = _api.Base64Decode(src);
        if (result == null)
        {
            return null;
        }
        return result;
    }
    public object? Connect(params object[] args)
    {
        return _api.Connect();
    }
    public object? IsConnected(params object[] args)
    {
        return _api.IsConnected();
    }
    public object? Disconnect(params object[] args)
    {
        return _api.Disconnect();
    }

    public object? GetWorldById(params object[] args)
    {
        return null;
    }

    public object? GetWorld(params object[] args)
    {

        return null;
    }

    public object? GetWorldID(params object[] args)
    {

        return _api.GetWorldID();


    }
    public object? GetWorldIdList(params object[] args)
    {

        return _runtime.Script.Array();
    }
    public object? GetWorldList(params object[] args)
    {

        return _runtime.Script.Array();
    }
    public object? WorldName(params object[] args)
    {
        return _api.WorldName();
    }
    public object? WorldAddress(params object[] args)
    {
        return _api.WorldAddress();
    }
    public object? WorldPort(params object[] args)
    {
        return _api.WorldPort();
    }
    public object? WorldProxy(params object[] args)
    {
        return _api.WorldProxy();
    }

    public object? Trim(params object[] args)
    {
        var src = GetStringArg(args, 0);
        return _api.Trim(src);
    }
    public object? GetUniqueNumber(params object[] args)
    {

        return _api.GetUniqueNumber();
    }
    public object? GetUniqueID(params object[] args)
    {

        return _api.GetUniqueID();
    }
    public object? CreateGUID(params object[] args)
    {

        return _api.CreateGUID();
    }
    public object? FlashIcon(params object[] args)
    {
        _api.FlashIcon();
        return null;
    }
    public object? SetStatus(params object[] args)
    {
        var text = GetStringArg(args, 0);
        _api.SetStatus(text);
        return null;
    }
    public object? DeleteCommandHistory(params object[] args)
    {
        _api.DeleteCommandHistory();
        return null;
    }
    public object? DiscardQueue(params object[] args)
    {
        return _api.DiscardQueue(GetBoolArg(args, 0));
    }
    public object? LockQueue(params object[] args)
    {
        _api.LockQueue();
        return null;
    }
    public object? GetQueue(params object[] args)
    {
        var cmds = _api.GetQueue();
        return cmds;
    }
    public object? Queue(params object[] args)
    {

        return _api.Queue(GetStringArg(args, 0), GetBoolArg(args, 1));
    }
    public object? DoAfter(params object[] args)
    {
        var seconds = GetDoubleArg(args, 0);
        var send = GetStringArg(args, 1);
        return _api.DoAfter(seconds, send);
    }
    public object? DoAfterNote(params object[] args)
    {
        var seconds = GetDoubleArg(args, 0);
        var send = GetStringArg(args, 1);
        return _api.DoAfterNote(seconds, send);
    }
    public object? DoAfterSpeedWalk(params object[] args)
    {
        var seconds = GetDoubleArg(args, 0);
        var send = GetStringArg(args, 1);
        return _api.DoAfterSpeedWalk(seconds, send);
    }
    public object? DoAfterSpecial(params object[] args)
    {
        var seconds = GetDoubleArg(args, 0);
        var send = GetStringArg(args, 1);
        var sendto = GetIntArg(args, 2);
        return _api.DoAfterSpecial(seconds, send, sendto);
    }

    public object? DeleteGroup(params object[] args)
    {
        var name = GetStringArg(args, 0);
        return _api.DeleteGroup(name);
    }

    public object? AddTimer(params object[] args)
    {
        var name = GetStringArg(args, 0);
        var hour = GetIntArg(args, 1);
        var min = GetIntArg(args, 2);
        var seconds = GetDoubleArg(args, 3);
        var send = GetStringArg(args, 4);
        var flags = GetIntArg(args, 5);
        var script = GetStringArg(args, 6);
        return _api.AddTimer(name, hour, min, seconds, send, flags, script);
    }
    public object? DeleteTimer(params object[] args)
    {
        var name = GetStringArg(args, 0);
        return _api.DeleteTimer(name);
    }
    public object? DeleteTemporaryTimers(params object[] args)
    {
        return _api.DeleteTemporaryTimers();
    }
    public object? DeleteTimerGroup(params object[] args)
    {

        var name = GetStringArg(args, 0);
        return _api.DeleteTimerGroup(name);
    }

    public object? EnableTimer(params object[] args)
    {

        var name = GetStringArg(args, 0);
        var enabled = GetBoolArg(args, 1);
        return _api.EnableTimer(name, enabled);
    }
    public object? EnableTimerGroup(params object[] args)
    {

        var group = GetStringArg(args, 0);
        var enabled = GetBoolArg(args, 1);
        return _api.EnableTimerGroup(group, enabled);
    }

    public object? GetTimerList(params object[] args)
    {

        var list = _api.GetTimerList();


        var result = _runtime.Script.Array();
        foreach (var v in list)
        {
            result.push(v);
        }
        return result;
    }
    public object? IsTimer(params object[] args)
    {
        var name = GetStringArg(args, 0);
        return _api.IsTimer(name);
    }

    public object? ResetTimer(params object[] args)
    {
        var name = GetStringArg(args, 0);
        return _api.ResetTimer(name);
    }

    public object? ResetTimers(params object[] args)
    {
        _api.ResetTimers();
        return null;
    }

    public object? GetTimerOption(params object[] args)
    {
        var name = GetStringArg(args, 0);
        var option = GetStringArg(args, 1);
        var (result, code) = _api.GetTimerOption(name, option);
        if (code != ScriptAPI.EOK)
        {
            return null;
        }
        else
        {
            switch (option)
            {
                case "active_closed":
                case "at_time":
                case "enabled":
                case "omit_from_log":
                case "omit_from_output":
                case "one_shot":
                    return result == MushString.StringYes;
                case "group":
                case "name":
                case "script":
                case "send":
                case "variable":
                    return result;
                case "hour":
                case "minute":
                case "offset_hour":
                case "offset_minute":
                case "offset_second":
                case "send_to":
                case "user":
                    return int.TryParse(result, out var i) ? i : 0;
                case "second":
                    return Double.TryParse(result, out var d) ? d : 0.0;
            }
        }
        return null;
    }
    public object? SetTimerOption(params object[] args)
    {

        var name = GetStringArg(args, 0);


        var option = GetStringArg(args, 1);


        string value = "";


        switch (option)
        {
            case "active_closed":
            case "at_time":
            case "enabled":
            case "omit_from_log":
            case "omit_from_output":
            case "one_shot":
                if (GetBoolArg(args, 2))
                {
                    value = MushString.StringYes;
                }
                else
                {
                    value = "";
                }
                break;
            case "group":
            case "name":
            case "script":
            case "send":
            case "variable":
                value = GetStringArg(args, 2);
                break;
            case "hour":
            case "minute":
            case "offset_hour":
            case "offset_minute":
            case "offset_second":
            case "second":
            case "send_to":
            case "user":
                value = GetStringArg(args, 2);
                break;

        }
        return _api.SetTimerOption(name, option, value);
    }

    public object? AddAlias(params object[] args)
    {
        var name = GetStringArg(args, 0);
        var match = GetStringArg(args, 1);
        var send = GetStringArg(args, 2);
        var flags = GetIntArg(args, 3);
        var script = GetStringArg(args, 4);
        return _api.AddAlias(name, match, send, flags, script);
    }
    public object? DeleteAlias(params object[] args)
    {
        var name = GetStringArg(args, 0);
        return _api.DeleteAlias(name);
    }
    public object? DeleteTemporaryAliases(params object[] args)
    {

        return _api.DeleteTemporaryAliases();
    }
    public object? DeleteAliasGroup(params object[] args)
    {

        var name = GetStringArg(args, 0);


        return _api.DeleteAliasGroup(name);
    }

    public object? EnableAlias(params object[] args)
    {
        var name = GetStringArg(args, 0);
        var enabled = GetBoolArg(args, 1);
        return _api.EnableAlias(name, enabled);
    }
    public object? EnableAliasGroup(params object[] args)
    {
        var group = GetStringArg(args, 0);
        var enabled = GetBoolArg(args, 1);
        return _api.EnableAliasGroup(group, enabled);
    }

    public object? GetAliasList(params object[] args)
    {

        var list = _api.GetAliasList();
        var result = _runtime.Script.Array();
        foreach (var v in list)
        {
            result.push(v);
        }
        return result;
    }
    public object? IsAlias(params object[] args)
    {
        var name = GetStringArg(args, 0);
        return _api.IsAlias(name);
    }

    public object? GetAliasOption(params object[] args)
    {
        var name = GetStringArg(args, 0);
        var option = GetStringArg(args, 1);
        var (result, code) = _api.GetAliasOption(name, option);
        if (code != ScriptAPI.EOK)
        {
            return null;
        }
        else
        {
            switch (option)
            {
                case "echo_alias":
                case "enabled":
                case "expand_variables":
                case "ignore_case":
                case "keep_evaluating":
                case "menu":
                case "omit_from_command_history":
                case "regexp":
                case "omit_from_log":
                case "omit_from_output":
                case "one_shot":
                    return result == MushString.StringYes;
                case "group":
                case "name":
                case "match":
                case "script":
                case "send":
                case "variable":
                    return result;
                case "send_to":
                case "user":
                case "sequence":
                    var ri = int.TryParse(result, out var i) ? i : 0;
                    return ri;
            }
            return null;
        }
    }
    public object? SetAliasOption(params object[] args)
    {
        var name = GetStringArg(args, 0);
        var option = GetStringArg(args, 1);
        string value = "";
        switch (option)
        {
            case "echo_alias":
            case "enabled":
            case "expand_variables":
            case "ignore_case":
            case "keep_evaluating":
            case "menu":
            case "omit_from_command_history":
            case "omit_from_log":
            case "omit_from_output":
            case "one_shot":
            case "regexp":
                if (GetStringArg(args, 2) == MushString.StringYes)
                {
                    value = MushString.StringYes;
                }
                else
                {
                    value = "";
                }
                break;
            case "group":
            case "name":
            case "match":
            case "script":
            case "send":
            case "variable":
                value = GetStringArg(args, 2);
                break;
            case "send_to":
            case "user":
            case "sequence":
                value = GetStringArg(args, 2);
                break;
        }
        return _api.SetAliasOption(name, option, value);
    }

    public object? AddTrigger(params object[] args)
    {
        var name = GetStringArg(args, 0);
        var match = GetStringArg(args, 1);
        var send = GetStringArg(args, 2);
        var flags = GetIntArg(args, 3);
        var color = GetIntArg(args, 4);
        var wildcard = GetIntArg(args, 5);
        var sound = GetStringArg(args, 6);
        var script = GetStringArg(args, 7);
        return _api.AddTrigger(name, match, send, flags, color, wildcard, sound, script);
    }
    public object? AddTriggerEx(params object[] args)
    {
        var name = GetStringArg(args, 0);
        var match = GetStringArg(args, 1);
        var send = GetStringArg(args, 2);
        var flags = GetIntArg(args, 3);
        var color = GetIntArg(args, 4);
        var wildcard = GetIntArg(args, 5);
        var sound = GetStringArg(args, 6);
        var script = GetStringArg(args, 7);
        var sendto = GetIntArg(args, 8);
        var sequence = GetIntArg(args, 9);
        return _api.AddTriggerEx(name, match, send, flags, color, wildcard, sound, script, sendto, sequence);
    }
    public object? DeleteTrigger(params object[] args)
    {
        var name = GetStringArg(args, 0);
        return _api.DeleteTrigger(name);
    }
    public object? DeleteTemporaryTriggers(params object[] args)
    {
        return _api.DeleteTemporaryTriggers();
    }
    public object? DeleteTriggerGroup(params object[] args)
    {
        var name = GetStringArg(args, 0);
        return _api.DeleteTriggerGroup(name);
    }

    public object? EnableTrigger(params object[] args)
    {

        var name = GetStringArg(args, 0);
        var enabled = GetBoolArg(args, 1);
        return _api.EnableTrigger(name, enabled);
    }
    public object? EnableTriggerGroup(params object[] args)
    {

        var name = GetStringArg(args, 0);
        var enabled = GetBoolArg(args, 1);
        return _api.EnableTriggerGroup(name, enabled);
    }
    public object? GetTriggerList(params object[] args)
    {
        var list = _api.GetTriggerList();
        var result = _runtime.Script.Array();
        foreach (var v in list)
        {
            result.push(v);
        }
        return result;
    }
    public object? IsTrigger(params object[] args)
    {
        var name = GetStringArg(args, 0);
        return _api.IsTrigger(name);
    }

    public object? GetTriggerOption(params object[] args)
    {
        var name = GetStringArg(args, 0);
        var option = GetStringArg(args, 1);
        var (result, code) = _api.GetTriggerOption(name, option);
        if (code != ScriptAPI.EOK)
        {
            return null;
        }
        else
        {
            switch (option)
            {
                case "echo_trigger":
                case "enabled":
                case "expand_variables":
                case "ignore_case":
                case "keep_evaluating":
                case "menu":
                case "omit_from_command_history":
                case "regexp":
                case "omit_from_log":
                case "omit_from_output":
                case "one_shot":
                    return result == MushString.StringYes;
                case "group":
                case "name":
                case "match":
                case "script":
                case "send":
                case "variable":
                    return result;
                case "send_to":
                case "user":
                case "sequence":
                    var ri = int.TryParse(result, out var i) ? i : 0;
                    return ri;
            }
        }
        return null;
    }
    public object? SetTriggerOption(params object[] args)
    {
        var name = GetStringArg(args, 0);
        var option = GetStringArg(args, 1);
        string value = "";
        switch (option)
        {
            case "echo_trigger":
            case "multi_line":
            case "enabled":
            case "expand_variables":
            case "ignore_case":
            case "keep_evaluating":
            case "menu":
            case "omit_from_command_history":
            case "omit_from_log":
            case "omit_from_output":
            case "one_shot":
            case "regexp":

                if (GetStringArg(args, 2) == MushString.StringYes)
                {
                    value = MushString.StringYes;
                }
                else
                {
                    value = "";
                }
                break;
            case "group":
            case "name":
            case "match":
            case "script":
            case "send":
            case "variable":
                value = GetStringArg(args, 2);
                break;
            case "lines_to_match":
            case "send_to":
            case "user":
            case "sequence":
                value = GetStringArg(args, 2);
                break;
        }
        return _api.SetTriggerOption(name, option, value);
    }

    public object? StopEvaluatingTriggers(params object[] args)
    {
        _api.StopEvaluatingTriggers();
        return null;
    }
    public object? GetTriggerWildcard(params object[] args)
    {
        var result = _api.GetTriggerWildcard(GetStringArg(args, 0), GetStringArg(args, 1));
        if (result == null)
        {
            return null;
        }
        return result;
    }

    public object? ColourNameToRGB(params object[] args)
    {
        var v = _api.ColourNameToRGB(GetStringArg(args, 0));
        return v;
    }
    public object? SetSpeedWalkDelay(params object[] args)
    {
        _api.SetSpeedWalkDelay(GetIntArg(args, 0));
        return null;
    }
    public object? GetSpeedWalkDelay(params object[] args)
    {
        return _api.SpeedWalkDelay();
    }

    public object? NewGetModInfoAPI(params object[] args)
    {
        var mod = _api.GetModInfo();
        return mod;
    }
    public object? NewHasFileAPI(params object[] args)
    {
        return _api.HasFile(GetStringArg(args, 0));
    }
    public object? NewReadFileAPI(params object[] args)
    {
        return _api.ReadFile(GetStringArg(args, 0));
    }
    public object? NewHasModFileAPI(params object[] args)
    {

        return _api.HasModFile(GetStringArg(args, 0));
    }
    public object? NewMakeHomeFolderAPI(params object[] args)
    {

        return _api.MakeHomeFolder(GetStringArg(args, 0));
    }
    public object? NewHasHomeFileAPI(params object[] args)
    {
        return _api.HasHomeFile(GetStringArg(args, 0));
    }
    public object? NewWriteHomeFileAPI(params object[] args)
    {

        _api.WriteHomeFile(GetStringArg(args, 0), Encoding.UTF8.GetBytes(GetStringArg(args, 1)));

        return null;
    }
    public object? NewReadHomeFileAPI(params object[] args)
    {
        return _api.ReadHomeFile(GetStringArg(args, 0));
    }
    public object? NewReadHomeLinesAPI(params object[] args)
    {

        var lines = _api.ReadHomeLines(GetStringArg(args, 0));
        return ToJsArray(lines);
    }

    public object? NewMakeSharedFolderAPI(params object[] args)
    {

        return _api.MakeSharedFolder(GetStringArg(args, 0));
    }
    public object? NewHasSharedFileAPI(params object[] args)
    {
        return _api.HasSharedFile(GetStringArg(args, 0));
    }
    public object? NewWriteSharedFileAPI(params object[] args)
    {

        _api.WriteSharedFile(GetStringArg(args, 0), Encoding.UTF8.GetBytes(GetStringArg(args, 1)));

        return null;
    }
    public object? NewReadSharedFileAPI(params object[] args)
    {
        return _api.ReadSharedFile(GetStringArg(args, 0));
    }
    public object? NewReadSharedLinesAPI(params object[] args)
    {

        var lines = _api.ReadSharedLines(GetStringArg(args, 0));
        return ToJsArray(lines);
    }

    public object? NewReadModFileAPI(params object[] args)
    {
        return _api.ReadModFile(GetStringArg(args, 0));
    }

    public object? NewReadLinesAPI(params object[] args)
    {
        var lines = _api.ReadLines(GetStringArg(args, 0));
        return ToJsArray(lines);
    }
    public object? NewReadModLinesAPI(params object[] args)
    {
        var lines = _api.ReadModLines(GetStringArg(args, 0));
        return ToJsArray(lines);
    }
    public object? SplitNfunc(params object[] args)
    {
        var text = GetStringArg(args, 0);
        var sep = GetStringArg(args, 1);
        var n = GetIntArg(args, 2);
        var s = _api.SplitN(text, sep, n);
        return ToJsArray(s);
    }

    public object? UTF8Len(params object[] args)
    {
        var text = GetStringArg(args, 0);
        return _api.UTF8Len(text);
    }
    public object? UTF8Index(params object[] args)
    {
        var text = GetStringArg(args, 0);
        var sub = GetStringArg(args, 1);
        return _api.UTF8Index(text, sub);
    }
    public object? ToUTF8(params object[] args)
    {
        var code = GetStringArg(args, 0);
        var text = GetStringArg(args, 1);
        var result = _api.ToUTF8(code, Encoding.UTF8.GetBytes(text));
        return result;
    }
    public object? FromUTF8(params object[] args)
    {
        var code = GetStringArg(args, 0);
        var text = GetStringArg(args, 1);
        var result = _api.FromUTF8(code, text);
        return result;
    }
    public object? UTF8Sub(params object[] args)
    {
        var text = GetStringArg(args, 0);
        var start = GetIntArg(args, 1);
        var end = GetIntArg(args, 2);
        return _api.UTF8Sub(text, start, end);
    }
    public object? Info(params object[] args)
    {
        var text = GetStringArg(args, 0);
        _api.Info(text);
        return null;
    }
    public object? InfoClear(params object[] args)
    {
        _api.InfoClear();
        return null;
    }

    public object? GetAlphaOption(params object[] args)
    {
        return _api.GetAlphaOption(GetStringArg(args, 0));
    }

    public object? SetAlphaOption(params object[] args)
    {

        return _api.SetAlphaOption(GetStringArg(args, 0), GetStringArg(args, 1));
    }
    public object? WriteLog(params object[] args)
    {
        return _api.WriteLog(GetStringArg(args, 0));
    }

    public object? CloseLog(params object[] args)
    {
        return _api.CloseLog();
    }
    public object? OpenLog(params object[] args)
    {
        return _api.OpenLog();
    }
    public object? FlushLog(params object[] args)
    {
        return _api.FlushLog();
    }

    public object? GetLinesInBufferCount(params object[] args)
    {
        return _api.GetLinesInBufferCount();
    }
    public object? DeleteOutput(params object[] args)
    {
        _api.DeleteOutput();
        return null;
    }
    public object? DeleteLines(params object[] args)
    {
        _api.DeleteLines(GetIntArg(args, 0));
        return null;
    }
    public object? GetLineCount(params object[] args)
    {
        return _api.GetLineCount();
    }
    public object? GetRecentLines(params object[] args)
    {

        return _api.GetRecentLines(GetIntArg(args, 0));
    }
    public object? GetLineInfo(params object[] args)
    {

        var (val, ok) = _api.GetLineInfo(GetIntArg(args, 0), GetIntArg(args, 1));
        if (!ok)
        {
            return null;
        }
        switch (GetIntArg(args, 1))
        {
            case 1:
                return val;
            case 2:
                return val;
            case 3:
                return MushString.FromStringInt(val);
            case 4:
                return MushString.FromStringBool(val);
            case 5:
                return MushString.FromStringBool(val);
            case 6:
                return MushString.FromStringBool(val);
            case 7:
                return MushString.FromStringBool(val);
            case 8:
                return MushString.FromStringBool(val);
            case 9:
                return MushString.FromStringInt(val);
            case 11:
                return MushString.FromStringInt(val);
        }
        return null;
    }
    public object? BoldColour(params object[] args)
    {
        return _api.BoldColour(GetIntArg(args, 0));
    }
    public object? NormalColour(params object[] args)
    {
        return _api.NormalColour(GetIntArg(args, 0));
    }

    public object? GetStyleInfo(params object[] args)
    {

        var (val, ok) = _api.GetStyleInfo(GetIntArg(args, 0), GetIntArg(args, 1), GetIntArg(args, 2));
        if (!ok)
        {
            return null;
        }
        switch (GetIntArg(args, 2))
        {
            case 1:
                return val;
            case 2:
                return MushString.FromStringInt(val);
            case 3:
                return MushString.FromStringInt(val);
            case 8:
                return MushString.FromStringBool(val);
            case 9:
                return MushString.FromStringBool(val);
            case 10:
                return MushString.FromStringBool(val);
            case 11:
                return MushString.FromStringBool(val);
            case 14:
                return MushString.FromStringInt(val);
            case 15:
                return MushString.FromStringInt(val);
        }
        return null;
    }

    public object? GetInfo(params object[] args)
    {
        return _api.GetInfo(GetIntArg(args, 0));
    }
    public object? GetTimerInfo(params object[] args)
    {

        var (v, ok) = _api.GetTimerInfo(GetStringArg(args, 0), GetIntArg(args, 1));


        if (ok != ScriptAPI.EOK)
        {
            return null;
        }
        switch (GetIntArg(args, 1))
        {
            case 1:
                return MushString.FromStringInt(v);
            case 2:
                return MushString.FromStringInt(v);
            case 3:
                return MushString.FromStringInt(v);
            case 4:
                return v;
            case 5:
                return v;
            case 6:
                return MushString.FromStringBool(v);
            case 7:
                return MushString.FromStringBool(v);
            case 8:
                return MushString.FromStringBool(v);
            case 14:
                return MushString.FromStringBool(v);
            case 19:
                return v;
            case 20:
                return MushString.FromStringInt(v);
            case 21:
                return MushString.FromStringInt(v);
            case 22:
                return v;
            case 23:
                return MushString.FromStringBool(v);
            case 24:
                return MushString.FromStringBool(v);

        }
        return null;
    }
    public object? GetTriggerInfo(params object[] args)
    {

        var (v, ok) = _api.GetTriggerInfo(GetStringArg(args, 0), GetIntArg(args, 1));
        if (ok != ScriptAPI.EOK)
        {
            return null;
        }
        switch (GetIntArg(args, 1))
        {
            case 1:
                return v;
            case 2:
                return v;
            case 3:
                return v;
            case 4:
                return v;
            case 5:
                return MushString.FromStringBool(v);
            case 6:
                return MushString.FromStringBool(v);
            case 7:
                return MushString.FromStringBool(v);
            case 8:
                return MushString.FromStringBool(v);
            case 9:
                return MushString.FromStringBool(v);
            case 10:
                return MushString.FromStringBool(v);
            case 11:
                return MushString.FromStringBool(v);
            case 13:
                return MushString.FromStringBool(v);
            case 15:
                return MushString.FromStringInt(v);
            case 16:
                return MushString.FromStringInt(v);
            case 23:
                return MushString.FromStringBool(v);
            case 25:
                return MushString.FromStringBool(v);
            case 26:
                return v;
            case 27:
                return v;
            case 28:
                return MushString.FromStringInt(v);
            case 31:
                return MushString.FromStringInt(v);
            case 36:
                return MushString.FromStringBool(v);
        }
        return null;
    }

    public object? GetAliasInfo(params object[] args)
    {

        var (v, ok) = _api.GetAliasInfo(GetStringArg(args, 0), GetIntArg(args, 1));


        if (ok != ScriptAPI.EOK)
        {
            return null;
        }
        switch (GetIntArg(args, 1))
        {
            case 1:
                return v;
            case 2:
                return v;
            case 3:
                return v;
            case 4:
                return v;
            case 5:
                return v;
            case 6:
                return MushString.FromStringBool(v);
            case 7:
                return MushString.FromStringBool(v);
            case 8:
                return MushString.FromStringBool(v);
            case 9:
                return MushString.FromStringBool(v);
            case 14:
                return MushString.FromStringBool(v);
            case 16:
                return v;
            case 17:
                return v;
            case 18:
                return MushString.FromStringInt(v);
            case 19:
                return MushString.FromStringBool(v);
            case 20:
                return MushString.FromStringInt(v);
            case 22:
                return MushString.FromStringBool(v);
            case 23:
                return MushString.FromStringInt(v);
            case 29:
                return MushString.FromStringBool(v);
        }
        return null;
    }

    public object? Broadcast(params object[] args)
    {

        _api.Broadcast(GetStringArg(args, 0), GetBoolArg(args, 1));
        return null;
    }
    public object? Notify(params object[] args)
    {

        string? link;


        if (!HasArg(args, 2))
        {
            link = null;
        }
        else
        {
            var data = GetStringArg(args, 2);
            link = data;
        }
        _api.Notify(GetStringArg(args, 0), GetStringArg(args, 1), link ?? "");
        return null;
    }
    public object? GetGlobalOption(params object[] args)
    {
        var result = _api.GetGlobalOption(GetStringArg(args, 0));
        switch (GetStringArg(args, 0))
        {
            default:
                switch (result)
                {
                    case "0":
                        return 0;
                    case "1":
                        return 1;
                    default:
                        return result;
                }
        }
    }

    public object? CheckPermissions(params object[] args)
    {
        var items = GetStringArrayArg(args, 0);
        return _api.CheckPermissions(items);
    }
    public object? RequestPermissions(params object[] args)
    {

        var items = GetStringArrayArg(args, 0);
        var reason = "";
        if (HasArg(args, 1))
        {
            reason = GetStringArg(args, 1);
        }
        var script = "";
        if (HasArg(args, 2))
        {
            script = GetStringArg(args, 2);
        }
        _api.RequestPermissions(items, reason, script);
        return null;
    }
    public object? CheckTrustedDomains(params object[] args)
    {
        var items = GetStringArrayArg(args, 0);
        return _api.CheckTrustedDomains(items);
    }

    public object? RequestTrustDomains(params object[] args)
    {

        var items = GetStringArrayArg(args, 0);
        var reason = "";
        if (HasArg(args, 1))
        {
            reason = GetStringArg(args, 1);
        }
        var script = "";

        if (HasArg(args, 2))
        {
            script = GetStringArg(args, 2);
        }
        _api.RequestTrustDomains(items, reason, script);
        return null;
    }
    public object? Encrypt(params object[] args)
    {
        var data = GetStringArg(args, 0);
        var key = GetStringArg(args, 1);
        var result = _api.Encrypt(data, key);
        if (result == null)
        {
            return null;
        }
        return result;
    }
    public object? Decrypt(params object[] args)
    {
        var data = GetStringArg(args, 0);
        var key = GetStringArg(args, 1);
        var result = _api.Decrypt(data, key);
        if (result == null)
        {
            return null;
        }
        return result;
    }

    public object? DumpOutput(params object[] args)
    {
        var length = GetIntArg(args, 0);
        var offset = GetIntArg(args, 1);
        return _api.DumpOutput(length, offset);
    }

    public object? ConcatOutput(params object[] args)
    {
        var output1 = GetStringArg(args, 0);
        var output2 = GetStringArg(args, 1);
        return _api.ConcatOutput(output1, output2);
    }
    public object? SliceOutput(params object[] args)
    {
        var output = GetStringArg(args, 0);
        var start = GetIntArg(args, 1);
        var end = GetIntArg(args, 2);
        return _api.SliceOutput(output, start, end);
    }
    public object? OutputToText(params object[] args)
    {
        var output = GetStringArg(args, 0);
        return _api.OutputToText(output);
    }
    public object? FormatOutput(params object[] args)
    {
        var output = GetStringArg(args, 0);
        return _api.FormatOutput(output);
    }
    public object? PrintOutput(params object[] args)
    {
        var output = GetStringArg(args, 0);
        return _api.PrintOutput(output);
    }
    public object? Simulate(params object[] args)
    {
        var text = GetStringArg(args, 0);
        _api.Simulate(text);
        return null;
    }
    public object? SimulateOutput(params object[] args)
    {
        var output = GetStringArg(args, 0);
        _api.SimulateOutput(output);
        return null;
    }

    public object? DumpTriggers(params object[] args)
    {
        var byUser = GetBoolArg(args, 0);
        return _api.DumpTriggers(byUser);
    }
    public object? RestoreTriggers(params object[] args)
    {
        var data = GetStringArg(args, 0);
        var byUser = GetBoolArg(args, 1);
        _api.RestoreTriggers(data, byUser);
        return null;
    }
    public object? DumpTimers(params object[] args)
    {

        var byUser = GetBoolArg(args, 0);
        return _api.DumpTimers(byUser);
    }
    public object? RestoreTimers(params object[] args)
    {

        var data = GetStringArg(args, 0);
        var byUser = GetBoolArg(args, 1);
        _api.RestoreTimers(data, byUser);
        return null;
    }
    public object? DumpAliases(params object[] args)
    {

        var byUser = GetBoolArg(args, 0);
        return _api.DumpAliases(byUser);
    }
    public object? RestoreAliases(params object[] args)
    {
        var data = GetStringArg(args, 0);
        var byUser = GetBoolArg(args, 1);
        _api.RestoreAliases(data, byUser);
        return null;
    }
    public object? SetHUDSize(params object[] args)
    {
        var size = GetIntArg(args, 0);
        _api.SetHUDSize(size);

        return null;
    }
    public object? GetHUDContent(params object[] args)
    {
        var content = _api.GetHUDContent();
        return content;
    }
    public object? GetHUDSize(params object[] args)
    {

        var size = _api.GetHUDSize();
        return size;
    }
    public object? UpdateHUD(params object[] args)
    {
        var start = GetIntArg(args, 0);
        var content = GetStringArg(args, 1);
        var result = _api.UpdateHUD(start, content);
        return result;
    }
    public object? NewLine(params object[] args)
    {

        return _api.NewLine();
    }
    public object? NewWord(params object[] args)
    {

        var text = GetStringArg(args, 0);
        return _api.NewWord(text);
    }

    public object? SetPriority(params object[] args)
    {
        var value = GetIntArg(args, 0);
        _api.SetPriority(value);
        return null;
    }
    public object? GetPriority(params object[] args)
    {
        return _api.GetPriority();
    }
    public object? SetSummary(params object[] args)
    {

        var content = GetStringArg(args, 0);
        _api.SetSummary(content);
        return null;
    }
    public object? GetSummary(params object[] args)
    {

        return _api.GetSummary();
    }
    public object? Save(params object[] args)
    {

        return _api.Save();
    }
    public object? Milliseconds(params object[] args)
    {

        return _api.Milliseconds();
    }

    public object? OmitOutput(params object[] args)
    {
        _api.OmitOutput();
        return null;
    }
}