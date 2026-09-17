using System.Globalization;
using System.Text;
using Hellclient.Script.Infras.Components.API;
using Hellclient.World.Utils;
using Python.Runtime;

namespace Hellclient.PythonEngine.Infras.Components;

public class PyAPI(ScriptAPI api, PyModule scope)
{
    private PyModule _scope { get; init; } = scope;
    private ScriptAPI _api { get; init; } = api;
    public void Print(params PyObject[] args)
    {
        using var _ = Py.GIL();
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
    public static PyObject? GetArg(PyObject?[] args, int idx)
    {
        if (idx < 0 || idx >= args.Length)
        {
            return null;
        }
        return args[idx];
    }
    public static bool HasArg(PyObject?[] args, int idx)
    {
        if (idx < 0 || idx >= args.Length)
        {
            return false;
        }
        return args[idx] is not null;
    }
    public static string ConvertString(PyObject? arg)
    {
        return arg?.ToString() ?? "";
    }
    public static string GetStringArg(PyObject?[] args, int idx)
    {
        return ConvertString(GetArg(args, idx));
    }
    public static int ConvertInt(PyObject? arg)
    {
        return arg?.ToInt32(CultureInfo.InvariantCulture) ?? 0;
    }
    public static int GetIntArg(PyObject?[] args, int idx)
    {
        return ConvertInt(GetArg(args, idx));
    }
    public static List<string> ConvertStringArray(PyObject? arg)
    {
        var result = new List<string>();
        if (arg != null && !arg.IsNone() && PyList.IsListType(arg))
        {
            var pl = PyList.AsList(arg);
            if (pl != null && !pl.IsNone())
            {
                foreach (PyObject item in pl)
                {
                    result.Add(item?.ToString() ?? "");
                }
            }
        }
        return result;
    }
    public static List<string> StringArrayFromObject(PyObject? obj)
    {
        var result = new List<string>();
        LoadArray(obj).ForEach(item => result.Add(item?.ToString() ?? ""));
        return result;
    }
    public static List<PyObject> LoadArray(PyObject? obj)
    {
        var result = new List<PyObject>();
        if (obj != null && !obj.IsNone() && PyList.IsListType(obj))
        {
            var pl = PyList.AsList(obj);
            if (pl != null && !pl.IsNone())
            {
                foreach (PyObject item in pl)
                {
                    result.Add(item);
                }
            }
        }
        return result;
    }
    public static List<string> GetStringArrayArg(PyObject?[] args, int idx)
    {
        return ConvertStringArray(GetArg(args, idx));
    }
    public static bool ConvertBool(PyObject? arg)
    {
        return arg?.IsTrue() ?? false;
    }
    public static bool GetBoolArg(PyObject[] args, int idx)
    {
        return ConvertBool(GetArg(args, idx));
    }
    public static double ConvertDouble(PyObject? arg)
    {
        return arg?.ToDouble(CultureInfo.InvariantCulture) ?? double.NaN;
    }
    public static double GetDoubleArg(PyObject[] args, int idx)
    {
        return ConvertDouble(GetArg(args, idx));
    }
    public static Dictionary<string, string> ConvertStringDictionary(object? arg)
    {
        var result = new Dictionary<string, string>();
        if (arg != null && arg is PyDict pd)
        {
            foreach (var key in pd.Keys())
            {
                var value = pd[key];
                result[key?.ToString() ?? ""] = ConvertString(value);
            }
        }
        return result;
    }
    public PyObject ToPyArray(List<string> list)
    {
        var result = new PyList();
        foreach (var v in list)
        {
            result.Append(v.ToPython());
        }
        return result;
    }
    public PyObject? Request(params PyObject[] args)
    {
        using var _ = Py.GIL();
        var msgtype = GetStringArg(args, 0);
        var msg = GetStringArg(args, 1);
        var id = _api.Request(msgtype, msg);
        return id.ToPython();
    }
    public PyObject? Note(params PyObject[] args)
    {
        using var _ = Py.GIL();
        var info = GetStringArg(args, 0);
        _api.Note(info);
        return null;
    }
    public PyObject? PrintSystem(params PyObject[] args)
    {
        using var _ = Py.GIL();
        var info = GetStringArg(args, 0);
        _api.PrintSystem(info);
        return null;
    }
    public PyObject? SendImmediate(params PyObject[] args)
    {
        using var _ = Py.GIL();
        var info = GetStringArg(args, 0);
        return _api.SendImmediate(info).ToPython();
    }
    public PyObject? Send(params PyObject[] args)
    {
        using var _ = Py.GIL();
        var info = GetStringArg(args, 0);
        var res = _api.Send(info);
        return res.ToPython();
    }
    public PyObject? Execute(params PyObject[] args)
    {
        using var _ = Py.GIL();
        var info = GetStringArg(args, 0);
        return _api.Execute(info).ToPython();
    }
    public PyObject? SendNoEcho(params PyObject[] args)
    {
        using var _ = Py.GIL();
        var info = GetStringArg(args, 0);
        return _api.SendNoEcho(info).ToPython();
    }
    public PyObject? GetVariable(params PyObject[] args)
    {
        using var _ = Py.GIL();
        var val = _api.GetVariable(GetStringArg(args, 0));
        return val.ToPython();
    }
    public PyObject? DeleteVariable(params PyObject[] args)
    {
        using var _ = Py.GIL();
        var name = GetStringArg(args, 0);
        return _api.DeleteVariable(name).ToPython();
    }
    public PyObject? SetVariable(params PyObject[] args)
    {
        using var _ = Py.GIL();
        var name = GetStringArg(args, 0);
        var value = GetStringArg(args, 1);
        return _api.SetVariable(name, value).ToPython();
    }
    public PyObject? GetVariableList(params PyObject[] args)
    {
        using var _ = Py.GIL();
        var list = _api.GetVariableList().ToList();
        var result = new string[list.Count];
        for (int k = 0; k < list.Count; k++)
        {
            result[k] = list[k].Value;
        }
        return result.ToPython();
    }
    public PyObject? GetVariableComment(params PyObject[] args)
    {
        using var _ = Py.GIL();
        var val = _api.GetVariableComment(GetStringArg(args, 0));
        return val.ToPython();
    }
    public PyObject? SetVariableComment(params PyObject[] args)
    {
        using var _ = Py.GIL();
        var name = GetStringArg(args, 0);
        var value = GetStringArg(args, 1);
        return _api.SetVariableComment(name, value).ToPython();
    }
    public PyObject? Version(params PyObject[] args)
    {
        using var _ = Py.GIL();
        return _api.Version().ToPython();
    }
    public PyObject? Hash(params PyObject[] args)
    {
        using var _ = Py.GIL();
        var name = GetStringArg(args, 0);
        return _api.Hash(name).ToPython();
    }
    public PyObject? Base64Encode(params PyObject[] args)
    {
        using var _ = Py.GIL();
        var src = GetStringArg(args, 0);
        var ok = GetBoolArg(args, 1);
        return _api.Base64Encode(src, ok).ToPython();
    }
    public PyObject? Base64Decode(params PyObject[] args)
    {
        using var _ = Py.GIL();
        var src = GetStringArg(args, 0);
        var result = _api.Base64Decode(src);
        if (result == null)
        {
            return null;
        }
        return result.ToPython();
    }
    public PyObject? Connect(params PyObject[] args)
    {
        using var _ = Py.GIL();
        return _api.Connect().ToPython();
    }
    public PyObject? IsConnected(params PyObject[] args)
    {
        using var _ = Py.GIL();
        return _api.IsConnected().ToPython();
    }
    public PyObject? Disconnect(params PyObject[] args)
    {
        using var _ = Py.GIL();
        return _api.Disconnect().ToPython();
    }

    public PyObject? GetWorldById(params PyObject[] args)
    {
        using var _ = Py.GIL();
        return null;
    }

    public PyObject? GetWorld(params PyObject[] args)
    {
        using var _ = Py.GIL();
        return null;
    }

    public PyObject? GetWorldID(params PyObject[] args)
    {
        using var _ = Py.GIL();
        return _api.GetWorldID().ToPython();
    }
    public PyObject? GetWorldIdList(params PyObject[] args)
    {
        using var _ = Py.GIL();
        return new PyList();
    }
    public PyObject? GetWorldList(params PyObject[] args)
    {
        using var _ = Py.GIL();
        return new PyList();
    }
    public PyObject? WorldName(params PyObject[] args)
    {
        using var _ = Py.GIL();
        return _api.WorldName().ToPython();
    }
    public PyObject? WorldAddress(params PyObject[] args)
    {
        using var _ = Py.GIL();
        return _api.WorldAddress().ToPython();
    }
    public PyObject? WorldPort(params PyObject[] args)
    {
        using var _ = Py.GIL();
        return _api.WorldPort().ToPython();
    }
    public PyObject? WorldProxy(params PyObject[] args)
    {
        using var _ = Py.GIL();
        return _api.WorldProxy().ToPython();
    }

    public PyObject? Trim(params PyObject[] args)
    {
        using var _ = Py.GIL();
        var src = GetStringArg(args, 0);
        return _api.Trim(src).ToPython();
    }
    public PyObject? GetUniqueNumber(params PyObject[] args)
    {
        using var _ = Py.GIL();
        return _api.GetUniqueNumber().ToPython();
    }
    public PyObject? GetUniqueID(params PyObject[] args)
    {
        using var _ = Py.GIL();
        return _api.GetUniqueID().ToPython();
    }
    public PyObject? CreateGUID(params PyObject[] args)
    {
        using var _ = Py.GIL();
        return _api.CreateGUID().ToPython();
    }
    public PyObject? FlashIcon(params PyObject[] args)
    {
        using var _ = Py.GIL();
        _api.FlashIcon();
        return null;
    }
    public PyObject? SetStatus(params PyObject[] args)
    {
        using var _ = Py.GIL();
        var text = GetStringArg(args, 0);
        _api.SetStatus(text);
        return null;
    }
    public PyObject? DeleteCommandHistory(params PyObject[] args)
    {
        using var _ = Py.GIL();
        _api.DeleteCommandHistory();
        return null;
    }
    public PyObject? DiscardQueue(params PyObject[] args)
    {
        using var _ = Py.GIL();
        return _api.DiscardQueue(GetBoolArg(args, 0)).ToPython();
    }
    public PyObject? LockQueue(params PyObject[] args)
    {
        using var _ = Py.GIL();
        _api.LockQueue();
        return null;
    }
    public PyObject? GetQueue(params PyObject[] args)
    {
        using var _ = Py.GIL();
        var cmds = _api.GetQueue();
        return cmds.ToPython();
    }
    public PyObject? Queue(params PyObject[] args)
    {
        using var _ = Py.GIL();
        return _api.Queue(GetStringArg(args, 0), GetBoolArg(args, 1)).ToPython();
    }
    public PyObject? DoAfter(params PyObject[] args)
    {
        using var _ = Py.GIL();
        var seconds = GetDoubleArg(args, 0);
        var send = GetStringArg(args, 1);
        return _api.DoAfter(seconds, send).ToPython();
    }
    public PyObject? DoAfterNote(params PyObject[] args)
    {
        using var _ = Py.GIL();
        var seconds = GetDoubleArg(args, 0);
        var send = GetStringArg(args, 1);
        return _api.DoAfterNote(seconds, send).ToPython();
    }
    public PyObject? DoAfterSpeedWalk(params PyObject[] args)
    {
        using var _ = Py.GIL();
        var seconds = GetDoubleArg(args, 0);
        var send = GetStringArg(args, 1);
        return _api.DoAfterSpeedWalk(seconds, send).ToPython();
    }
    public PyObject? DoAfterSpecial(params PyObject[] args)
    {
        using var _ = Py.GIL();
        var seconds = GetDoubleArg(args, 0);
        var send = GetStringArg(args, 1);
        var sendto = GetIntArg(args, 2);
        return _api.DoAfterSpecial(seconds, send, sendto).ToPython();
    }

    public PyObject? DeleteGroup(params PyObject[] args)
    {
        using var _ = Py.GIL();
        var name = GetStringArg(args, 0);
        return _api.DeleteGroup(name).ToPython();
    }

    public PyObject? AddTimer(params PyObject[] args)
    {
        using var _ = Py.GIL();
        var name = GetStringArg(args, 0);
        var hour = GetIntArg(args, 1);
        var min = GetIntArg(args, 2);
        var seconds = GetDoubleArg(args, 3);
        var send = GetStringArg(args, 4);
        var flags = GetIntArg(args, 5);
        var script = GetStringArg(args, 6);
        return _api.AddTimer(name, hour, min, seconds, send, flags, script).ToPython();
    }
    public PyObject? DeleteTimer(params PyObject[] args)
    {
        using var _ = Py.GIL();
        var name = GetStringArg(args, 0);
        return _api.DeleteTimer(name).ToPython();
    }
    public PyObject? DeleteTemporaryTimers(params PyObject[] args)
    {
        using var _ = Py.GIL();
        return _api.DeleteTemporaryTimers().ToPython();
    }
    public PyObject? DeleteTimerGroup(params PyObject[] args)
    {
        using var _ = Py.GIL();
        var name = GetStringArg(args, 0);
        return _api.DeleteTimerGroup(name).ToPython();
    }

    public PyObject? EnableTimer(params PyObject[] args)
    {
        using var _ = Py.GIL();
        var name = GetStringArg(args, 0);
        var enabled = GetBoolArg(args, 1);
        return _api.EnableTimer(name, enabled).ToPython();
    }
    public PyObject? EnableTimerGroup(params PyObject[] args)
    {
        using var _ = Py.GIL();
        var group = GetStringArg(args, 0);
        var enabled = GetBoolArg(args, 1);
        return _api.EnableTimerGroup(group, enabled).ToPython();
    }

    public PyObject? GetTimerList(params PyObject[] args)
    {
        using var _ = Py.GIL();
        var list = _api.GetTimerList();
        var result = new PyList();
        foreach (var v in list)
        {
            result.Append(v.ToPython());
        }
        return result;
    }
    public PyObject? IsTimer(params PyObject[] args)
    {
        using var _ = Py.GIL();
        var name = GetStringArg(args, 0);
        return _api.IsTimer(name).ToPython();
    }

    public PyObject? ResetTimer(params PyObject[] args)
    {
        using var _ = Py.GIL();
        var name = GetStringArg(args, 0);
        return _api.ResetTimer(name).ToPython();
    }

    public PyObject? ResetTimers(params PyObject[] args)
    {
        using var _ = Py.GIL();
        _api.ResetTimers();
        return null;
    }

    public PyObject? GetTimerOption(params PyObject[] args)
    {
        using var _ = Py.GIL();
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
                    return (result == MushString.StringYes).ToPython();
                case "group":
                case "name":
                case "script":
                case "send":
                case "variable":
                    return result.ToPython();
                case "hour":
                case "minute":
                case "offset_hour":
                case "offset_minute":
                case "offset_second":
                case "send_to":
                case "user":
                    return (int.TryParse(result, out var i) ? i : 0).ToPython();
                case "second":
                    return (Double.TryParse(result, out var d) ? d : 0.0).ToPython();
            }
        }
        return null;
    }
    public PyObject? SetTimerOption(params PyObject[] args)
    {
        using var _ = Py.GIL();
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
        return _api.SetTimerOption(name, option, value).ToPython();
    }

    public PyObject? AddAlias(params PyObject[] args)
    {
        using var _ = Py.GIL();
        var name = GetStringArg(args, 0);
        var match = GetStringArg(args, 1);
        var send = GetStringArg(args, 2);
        var flags = GetIntArg(args, 3);
        var script = GetStringArg(args, 4);
        return _api.AddAlias(name, match, send, flags, script).ToPython();
    }
    public PyObject? DeleteAlias(params PyObject[] args)
    {
        using var _ = Py.GIL();
        var name = GetStringArg(args, 0);
        return _api.DeleteAlias(name).ToPython();
    }
    public PyObject? DeleteTemporaryAliases(params PyObject[] args)
    {
        using var _ = Py.GIL();
        return _api.DeleteTemporaryAliases().ToPython();
    }
    public PyObject? DeleteAliasGroup(params PyObject[] args)
    {
        using var _ = Py.GIL();
        var name = GetStringArg(args, 0);
        return _api.DeleteAliasGroup(name).ToPython();
    }

    public PyObject? EnableAlias(params PyObject[] args)
    {
        using var _ = Py.GIL();
        var name = GetStringArg(args, 0);
        var enabled = GetBoolArg(args, 1);
        return _api.EnableAlias(name, enabled).ToPython();
    }
    public PyObject? EnableAliasGroup(params PyObject[] args)
    {
        using var _ = Py.GIL();
        var group = GetStringArg(args, 0);
        var enabled = GetBoolArg(args, 1);
        return _api.EnableAliasGroup(group, enabled).ToPython();
    }

    public PyObject? GetAliasList(params PyObject[] args)
    {
        using var _ = Py.GIL();
        var list = _api.GetAliasList();
        var result = new PyList();
        foreach (var v in list)
        {
            result.Append(v.ToPython());
        }
        return result;
    }
    public PyObject? IsAlias(params PyObject[] args)
    {
        using var _ = Py.GIL();
        var name = GetStringArg(args, 0);
        return _api.IsAlias(name).ToPython();
    }

    public PyObject? GetAliasOption(params PyObject[] args)
    {
        using var _ = Py.GIL();
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
                    return (result == MushString.StringYes).ToPython();
                case "group":
                case "name":
                case "match":
                case "script":
                case "send":
                case "variable":
                    return result.ToPython();
                case "send_to":
                case "user":
                case "sequence":
                    var ri = int.TryParse(result, out var i) ? i : 0;
                    return ri.ToPython();
            }
            return null;
        }
    }
    public PyObject? SetAliasOption(params PyObject[] args)
    {
        using var _ = Py.GIL();
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
        return _api.SetAliasOption(name, option, value).ToPython();
    }

    public PyObject? AddTrigger(params PyObject[] args)
    {
        using var _ = Py.GIL();
        var name = GetStringArg(args, 0);
        var match = GetStringArg(args, 1);
        var send = GetStringArg(args, 2);
        var flags = GetIntArg(args, 3);
        var color = GetIntArg(args, 4);
        var wildcard = GetIntArg(args, 5);
        var sound = GetStringArg(args, 6);
        var script = GetStringArg(args, 7);
        return _api.AddTrigger(name, match, send, flags, color, wildcard, sound, script).ToPython();
    }
    public PyObject? AddTriggerEx(params PyObject[] args)
    {
        using var _ = Py.GIL();
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
        return _api.AddTriggerEx(name, match, send, flags, color, wildcard, sound, script, sendto, sequence).ToPython();
    }
    public PyObject? DeleteTrigger(params PyObject[] args)
    {
        using var _ = Py.GIL();
        var name = GetStringArg(args, 0);
        return _api.DeleteTrigger(name).ToPython();
    }
    public PyObject? DeleteTemporaryTriggers(params PyObject[] args)
    {
        using var _ = Py.GIL();
        return _api.DeleteTemporaryTriggers().ToPython();
    }
    public PyObject? DeleteTriggerGroup(params PyObject[] args)
    {
        using var _ = Py.GIL();
        var name = GetStringArg(args, 0);
        return _api.DeleteTriggerGroup(name).ToPython();
    }

    public PyObject? EnableTrigger(params PyObject[] args)
    {
        using var _ = Py.GIL();
        var name = GetStringArg(args, 0);
        var enabled = GetBoolArg(args, 1);
        return _api.EnableTrigger(name, enabled).ToPython();
    }
    public PyObject? EnableTriggerGroup(params PyObject[] args)
    {
        using var _ = Py.GIL();
        var name = GetStringArg(args, 0);
        var enabled = GetBoolArg(args, 1);
        return _api.EnableTriggerGroup(name, enabled).ToPython();
    }
    public PyObject? GetTriggerList(params PyObject[] args)
    {
        using var _ = Py.GIL();
        var list = _api.GetTriggerList();
        var result = new PyList();
        foreach (var v in list)
        {
            result.Append(v.ToPython());
        }
        return result;
    }
    public PyObject? IsTrigger(params PyObject[] args)
    {
        using var _ = Py.GIL();
        var name = GetStringArg(args, 0);
        return _api.IsTrigger(name).ToPython();
    }

    public PyObject? GetTriggerOption(params PyObject[] args)
    {
        using var _ = Py.GIL();
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
                    return (result == MushString.StringYes).ToPython();
                case "group":
                case "name":
                case "match":
                case "script":
                case "send":
                case "variable":
                    return result.ToPython();
                case "send_to":
                case "user":
                case "sequence":
                    var ri = int.TryParse(result, out var i) ? i : 0;
                    return ri.ToPython();
            }
        }
        return null;
    }
    public PyObject? SetTriggerOption(params PyObject[] args)
    {
        using var _ = Py.GIL();
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
        return _api.SetTriggerOption(name, option, value).ToPython();
    }

    public PyObject? StopEvaluatingTriggers(params PyObject[] args)
    {
        using var _ = Py.GIL();
        _api.StopEvaluatingTriggers();
        return null;
    }
    public PyObject? GetTriggerWildcard(params PyObject[] args)
    {
        using var _ = Py.GIL();
        var result = _api.GetTriggerWildcard(GetStringArg(args, 0), GetStringArg(args, 1));
        if (result == null)
        {
            return null;
        }
        return result.ToPython();
    }

    public PyObject? ColourNameToRGB(params PyObject[] args)
    {
        using var _ = Py.GIL();
        var v = _api.ColourNameToRGB(GetStringArg(args, 0));
        return v.ToPython();
    }
    public PyObject? SetSpeedWalkDelay(params PyObject[] args)
    {
        using var _ = Py.GIL();
        _api.SetSpeedWalkDelay(GetIntArg(args, 0));
        return null;
    }
    public PyObject? GetSpeedWalkDelay(params PyObject[] args)
    {
        using var _ = Py.GIL();
        return _api.SpeedWalkDelay().ToPython();
    }

    public PyObject? NewGetModInfoAPI(params PyObject[] args)
    {
        using var _ = Py.GIL();
        var mod = _api.GetModInfo();
        if (mod == null)
        {
            return null;
        }
        var result = new PyDict();
        result["Enabled"] = mod.Enabled.ToPython();
        result["Exists"] = mod.Exists.ToPython();
        var fl = new PyList();
        result["FolderList"] = fl;
        mod.FolderList.ForEach(folder => fl.Append(folder.ToPython()));
        var fl2 = new PyList();
        result["FileList"] = fl2;
        mod.FileList.ForEach(file => fl2.Append(file.ToPython()));
        return result;
    }
    public PyObject? NewHasFileAPI(params PyObject[] args)
    {
        using var _ = Py.GIL();
        return _api.HasFile(GetStringArg(args, 0)).ToPython();
    }
    public PyObject? NewReadFileAPI(params PyObject[] args)
    {
        using var _ = Py.GIL();
        return _api.ReadFile(GetStringArg(args, 0)).ToPython();
    }
    public PyObject? NewHasModFileAPI(params PyObject[] args)
    {
        using var _ = Py.GIL();
        return _api.HasModFile(GetStringArg(args, 0)).ToPython();
    }
    public PyObject? NewMakeHomeFolderAPI(params PyObject[] args)
    {
        using var _ = Py.GIL();
        return _api.MakeHomeFolder(GetStringArg(args, 0)).ToPython();
    }
    public PyObject? NewHasHomeFileAPI(params PyObject[] args)
    {
        using var _ = Py.GIL();
        return _api.HasHomeFile(GetStringArg(args, 0)).ToPython();
    }
    public PyObject? NewWriteHomeFileAPI(params PyObject[] args)
    {
        using var _ = Py.GIL();
        _api.WriteHomeFile(GetStringArg(args, 0), Encoding.UTF8.GetBytes(GetStringArg(args, 1)));
        return null;
    }
    public PyObject? NewReadHomeFileAPI(params PyObject[] args)
    {
        using var _ = Py.GIL();
        return _api.ReadHomeFile(GetStringArg(args, 0)).ToPython();
    }
    public PyObject? NewReadHomeLinesAPI(params PyObject[] args)
    {
        using var _ = Py.GIL();
        var lines = _api.ReadHomeLines(GetStringArg(args, 0));
        return ToPyArray(lines);
    }

    public PyObject? NewMakeSharedFolderAPI(params PyObject[] args)
    {
        using var _ = Py.GIL();
        return _api.MakeSharedFolder(GetStringArg(args, 0)).ToPython();
    }
    public PyObject? NewHasSharedFileAPI(params PyObject[] args)
    {
        using var _ = Py.GIL();
        return _api.HasSharedFile(GetStringArg(args, 0)).ToPython();
    }
    public PyObject? NewWriteSharedFileAPI(params PyObject[] args)
    {
        using var _ = Py.GIL();
        _api.WriteSharedFile(GetStringArg(args, 0), Encoding.UTF8.GetBytes(GetStringArg(args, 1)));
        return null;
    }
    public PyObject? NewReadSharedFileAPI(params PyObject[] args)
    {
        using var _ = Py.GIL();
        return _api.ReadSharedFile(GetStringArg(args, 0)).ToPython();
    }
    public PyObject? NewReadSharedLinesAPI(params PyObject[] args)
    {
        using var _ = Py.GIL();
        var lines = _api.ReadSharedLines(GetStringArg(args, 0));
        return ToPyArray(lines);
    }

    public PyObject? NewReadModFileAPI(params PyObject[] args)
    {
        using var _ = Py.GIL();
        return _api.ReadModFile(GetStringArg(args, 0)).ToPython();
    }

    public PyObject? NewReadLinesAPI(params PyObject[] args)
    {
        using var _ = Py.GIL();
        var lines = _api.ReadLines(GetStringArg(args, 0));
        return ToPyArray(lines);
    }
    public PyObject? NewReadModLinesAPI(params PyObject[] args)
    {
        using var _ = Py.GIL();
        var lines = _api.ReadModLines(GetStringArg(args, 0));
        return ToPyArray(lines);
    }
    public PyObject? SplitNfunc(params PyObject[] args)
    {
        using var _ = Py.GIL();
        var text = GetStringArg(args, 0);
        var sep = GetStringArg(args, 1);
        var n = GetIntArg(args, 2);
        var s = _api.SplitN(text, sep, n);
        return ToPyArray(s);
    }

    public PyObject? UTF8Len(params PyObject[] args)
    {
        using var _ = Py.GIL();
        var text = GetStringArg(args, 0);
        return _api.UTF8Len(text).ToPython();
    }
    public PyObject? UTF8Index(params PyObject[] args)
    {
        using var _ = Py.GIL();
        var text = GetStringArg(args, 0);
        var sub = GetStringArg(args, 1);
        return _api.UTF8Index(text, sub).ToPython();
    }
    public PyObject? ToUTF8(params PyObject[] args)
    {
        using var _ = Py.GIL();
        var code = GetStringArg(args, 0);
        var text = GetStringArg(args, 1);
        var result = _api.ToUTF8(code, Encoding.UTF8.GetBytes(text));
        return result.ToPython();
    }
    public PyObject? FromUTF8(params PyObject[] args)
    {
        using var _ = Py.GIL();
        var code = GetStringArg(args, 0);
        var text = GetStringArg(args, 1);
        var result = _api.FromUTF8(code, text);
        return result.ToPython();
    }
    public PyObject? UTF8Sub(params PyObject[] args)
    {
        using var _ = Py.GIL();
        var text = GetStringArg(args, 0);
        var start = GetIntArg(args, 1);
        var end = GetIntArg(args, 2);
        return _api.UTF8Sub(text, start, end).ToPython();
    }
    public PyObject? Info(params PyObject[] args)
    {
        using var _ = Py.GIL();
        var text = GetStringArg(args, 0);
        _api.Info(text);
        return null;
    }
    public PyObject? InfoClear(params PyObject[] args)
    {
        using var _ = Py.GIL();
        _api.InfoClear();
        return null;
    }

    public PyObject? GetAlphaOption(params PyObject[] args)
    {
        using var _ = Py.GIL();
        return _api.GetAlphaOption(GetStringArg(args, 0)).ToPython();
    }

    public PyObject? SetAlphaOption(params PyObject[] args)
    {
        using var _ = Py.GIL();
        return _api.SetAlphaOption(GetStringArg(args, 0), GetStringArg(args, 1)).ToPython();
    }
    public PyObject? WriteLog(params PyObject[] args)
    {
        using var _ = Py.GIL();
        return _api.WriteLog(GetStringArg(args, 0)).ToPython();
    }

    public PyObject? CloseLog(params PyObject[] args)
    {
        using var _ = Py.GIL();
        return _api.CloseLog().ToPython();
    }
    public PyObject? OpenLog(params PyObject[] args)
    {
        using var _ = Py.GIL();
        return _api.OpenLog().ToPython();
    }
    public PyObject? FlushLog(params PyObject[] args)
    {
        using var _ = Py.GIL();
        return _api.FlushLog().ToPython();
    }

    public PyObject? GetLinesInBufferCount(params PyObject[] args)
    {
        using var _ = Py.GIL();
        return _api.GetLinesInBufferCount().ToPython();
    }
    public PyObject? DeleteOutput(params PyObject[] args)
    {
        using var _ = Py.GIL();
        _api.DeleteOutput();
        return null;
    }
    public PyObject? DeleteLines(params PyObject[] args)
    {
        using var _ = Py.GIL();
        _api.DeleteLines(GetIntArg(args, 0));
        return null;
    }
    public PyObject? GetLineCount(params PyObject[] args)
    {
        using var _ = Py.GIL();
        return _api.GetLineCount().ToPython();
    }
    public PyObject? GetRecentLines(params PyObject[] args)
    {
        using var _ = Py.GIL();
        return _api.GetRecentLines(GetIntArg(args, 0)).ToPython();
    }
    public PyObject? GetLineInfo(params PyObject[] args)
    {
        using var _ = Py.GIL();
        var (val, ok) = _api.GetLineInfo(GetIntArg(args, 0), GetIntArg(args, 1));
        if (!ok)
        {
            return null;
        }
        switch (GetIntArg(args, 1))
        {
            case 1:
                return val.ToPython();
            case 2:
                return val.ToPython();
            case 3:
                return MushString.FromStringInt(val).ToPython();
            case 4:
                return MushString.FromStringBool(val).ToPython();
            case 5:
                return MushString.FromStringBool(val).ToPython();
            case 6:
                return MushString.FromStringBool(val).ToPython();
            case 7:
                return MushString.FromStringBool(val).ToPython();
            case 8:
                return MushString.FromStringBool(val).ToPython();
            case 9:
                return MushString.FromStringInt(val).ToPython();
            case 11:
                return MushString.FromStringInt(val).ToPython();
        }
        return null;
    }
    public PyObject? BoldColour(params PyObject[] args)
    {
        using var _ = Py.GIL();
        return _api.BoldColour(GetIntArg(args, 0)).ToPython();
    }
    public PyObject? NormalColour(params PyObject[] args)
    {
        using var _ = Py.GIL();
        return _api.NormalColour(GetIntArg(args, 0)).ToPython();
    }

    public PyObject? GetStyleInfo(params PyObject[] args)
    {
        using var _ = Py.GIL();
        var (val, ok) = _api.GetStyleInfo(GetIntArg(args, 0), GetIntArg(args, 1), GetIntArg(args, 2));
        if (!ok)
        {
            return null;
        }
        switch (GetIntArg(args, 2))
        {
            case 1:
                return val.ToPython();
            case 2:
                return MushString.FromStringInt(val).ToPython();
            case 3:
                return MushString.FromStringInt(val).ToPython();
            case 8:
                return MushString.FromStringBool(val).ToPython();
            case 9:
                return MushString.FromStringBool(val).ToPython();
            case 10:
                return MushString.FromStringBool(val).ToPython();
            case 11:
                return MushString.FromStringBool(val).ToPython();
            case 14:
                return MushString.FromStringInt(val).ToPython();
            case 15:
                return MushString.FromStringInt(val).ToPython();
        }
        return null;
    }

    public PyObject? GetInfo(params PyObject[] args)
    {
        using var _ = Py.GIL();
        return _api.GetInfo(GetIntArg(args, 0)).ToPython();
    }
    public PyObject? GetTimerInfo(params PyObject[] args)
    {
        using var _ = Py.GIL();
        var (v, ok) = _api.GetTimerInfo(GetStringArg(args, 0), GetIntArg(args, 1));
        if (ok != ScriptAPI.EOK)
        {
            return null;
        }
        switch (GetIntArg(args, 1))
        {
            case 1:
                return MushString.FromStringInt(v).ToPython();
            case 2:
                return MushString.FromStringInt(v).ToPython();
            case 3:
                return MushString.FromStringInt(v).ToPython();
            case 4:
                return v.ToPython();
            case 5:
                return v.ToPython();
            case 6:
                return MushString.FromStringBool(v).ToPython();
            case 7:
                return MushString.FromStringBool(v).ToPython();
            case 8:
                return MushString.FromStringBool(v).ToPython();
            case 14:
                return MushString.FromStringBool(v).ToPython();
            case 19:
                return v.ToPython();
            case 20:
                return MushString.FromStringInt(v).ToPython();
            case 21:
                return MushString.FromStringInt(v).ToPython();
            case 22:
                return v.ToPython();
            case 23:
                return MushString.FromStringBool(v).ToPython();
            case 24:
                return MushString.FromStringBool(v).ToPython();

        }
        return null;
    }
    public PyObject? GetTriggerInfo(params PyObject[] args)
    {
        using var _ = Py.GIL();
        var (v, ok) = _api.GetTriggerInfo(GetStringArg(args, 0), GetIntArg(args, 1));
        if (ok != ScriptAPI.EOK)
        {
            return null;
        }
        switch (GetIntArg(args, 1))
        {
            case 1:
                return v.ToPython();
            case 2:
                return v.ToPython();
            case 3:
                return v.ToPython();
            case 4:
                return v.ToPython();
            case 5:
                return MushString.FromStringBool(v).ToPython();
            case 6:
                return MushString.FromStringBool(v).ToPython();
            case 7:
                return MushString.FromStringBool(v).ToPython();
            case 8:
                return MushString.FromStringBool(v).ToPython();
            case 9:
                return MushString.FromStringBool(v).ToPython();
            case 10:
                return MushString.FromStringBool(v).ToPython();
            case 11:
                return MushString.FromStringBool(v).ToPython();
            case 13:
                return MushString.FromStringBool(v).ToPython();
            case 15:
                return MushString.FromStringInt(v).ToPython();
            case 16:
                return MushString.FromStringInt(v).ToPython();
            case 23:
                return MushString.FromStringBool(v).ToPython();
            case 25:
                return MushString.FromStringBool(v).ToPython();
            case 26:
                return v.ToPython();
            case 27:
                return v.ToPython();
            case 28:
                return MushString.FromStringInt(v).ToPython();
            case 31:
                return MushString.FromStringInt(v).ToPython();
            case 36:
                return MushString.FromStringBool(v).ToPython();
        }
        return null;
    }

    public PyObject? GetAliasInfo(params PyObject[] args)
    {
        using var _ = Py.GIL();
        var (v, ok) = _api.GetAliasInfo(GetStringArg(args, 0), GetIntArg(args, 1));
        if (ok != ScriptAPI.EOK)
        {
            return null;
        }
        switch (GetIntArg(args, 1))
        {
            case 1:
                return v.ToPython();
            case 2:
                return v.ToPython();
            case 3:
                return v.ToPython();
            case 4:
                return v.ToPython();
            case 5:
                return v.ToPython();
            case 6:
                return MushString.FromStringBool(v).ToPython();
            case 7:
                return MushString.FromStringBool(v).ToPython();
            case 8:
                return MushString.FromStringBool(v).ToPython();
            case 9:
                return MushString.FromStringBool(v).ToPython();
            case 14:
                return MushString.FromStringBool(v).ToPython();
            case 16:
                return v.ToPython();
            case 17:
                return v.ToPython();
            case 18:
                return MushString.FromStringInt(v).ToPython();
            case 19:
                return MushString.FromStringBool(v).ToPython();
            case 20:
                return MushString.FromStringInt(v).ToPython();
            case 22:
                return MushString.FromStringBool(v).ToPython();
            case 23:
                return MushString.FromStringInt(v).ToPython();
            case 29:
                return MushString.FromStringBool(v).ToPython();
        }
        return null;
    }

    public PyObject? Broadcast(params PyObject[] args)
    {
        using var _ = Py.GIL();
        _api.Broadcast(GetStringArg(args, 0), GetBoolArg(args, 1));
        return null;
    }
    public PyObject? Notify(params PyObject[] args)
    {
        using var _ = Py.GIL();
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
    public PyObject? GetGlobalOption(params PyObject[] args)
    {
        using var _ = Py.GIL();
        var result = _api.GetGlobalOption(GetStringArg(args, 0));
        switch (GetStringArg(args, 0))
        {
            default:
                switch (result)
                {
                    case "0":
                        return 0.ToPython();
                    case "1":
                        return 1.ToPython();
                    default:
                        return result.ToPython();
                }
        }
    }

    public PyObject? CheckPermissions(params PyObject[] args)
    {
        using var _ = Py.GIL();
        var items = GetStringArrayArg(args, 0);
        return _api.CheckPermissions(items).ToPython();
    }
    public PyObject? RequestPermissions(params PyObject[] args)
    {
        using var _ = Py.GIL();
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
    public PyObject? CheckTrustedDomains(params PyObject[] args)
    {
        using var _ = Py.GIL();
        var items = GetStringArrayArg(args, 0);
        return _api.CheckTrustedDomains(items).ToPython();
    }

    public PyObject? RequestTrustDomains(params PyObject[] args)
    {
        using var _ = Py.GIL();
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
    public PyObject? Encrypt(params PyObject[] args)
    {
        using var _ = Py.GIL();
        var data = GetStringArg(args, 0);
        var key = GetStringArg(args, 1);
        var result = _api.Encrypt(data, key);
        if (result == null)
        {
            return null;
        }
        return result.ToPython();
    }
    public PyObject? Decrypt(params PyObject[] args)
    {
        using var _ = Py.GIL();
        var data = GetStringArg(args, 0);
        var key = GetStringArg(args, 1);
        var result = _api.Decrypt(data, key);
        if (result == null)
        {
            return null;
        }
        return result.ToPython();
    }

    public PyObject? DumpOutput(params PyObject[] args)
    {
        using var _ = Py.GIL();
        var length = GetIntArg(args, 0);
        var offset = GetIntArg(args, 1);
        return _api.DumpOutput(length, offset).ToPython();
    }

    public PyObject? ConcatOutput(params PyObject[] args)
    {
        using var _ = Py.GIL();
        var output1 = GetStringArg(args, 0);
        var output2 = GetStringArg(args, 1);
        return _api.ConcatOutput(output1, output2).ToPython();
    }
    public PyObject? SliceOutput(params PyObject[] args)
    {
        using var _ = Py.GIL();
        var output = GetStringArg(args, 0);
        var start = GetIntArg(args, 1);
        var end = GetIntArg(args, 2);
        return _api.SliceOutput(output, start, end).ToPython();
    }
    public PyObject? OutputToText(params PyObject[] args)
    {
        using var _ = Py.GIL();
        var output = GetStringArg(args, 0);
        return _api.OutputToText(output).ToPython();
    }
    public PyObject? FormatOutput(params PyObject[] args)
    {
        using var _ = Py.GIL();
        var output = GetStringArg(args, 0);
        return _api.FormatOutput(output).ToPython();
    }
    public PyObject? PrintOutput(params PyObject[] args)
    {
        using var _ = Py.GIL();
        var output = GetStringArg(args, 0);
        return _api.PrintOutput(output).ToPython();
    }
    public PyObject? Simulate(params PyObject[] args)
    {
        using var _ = Py.GIL();
        var text = GetStringArg(args, 0);
        _api.Simulate(text);
        return null;
    }
    public PyObject? SimulateOutput(params PyObject[] args)
    {
        using var _ = Py.GIL();
        var output = GetStringArg(args, 0);
        _api.SimulateOutput(output);
        return null;
    }

    public PyObject? DumpTriggers(params PyObject[] args)
    {
        using var _ = Py.GIL();
        var byUser = GetBoolArg(args, 0);
        return _api.DumpTriggers(byUser).ToPython();
    }
    public PyObject? RestoreTriggers(params PyObject[] args)
    {
        using var _ = Py.GIL();
        var data = GetStringArg(args, 0);
        var byUser = GetBoolArg(args, 1);
        _api.RestoreTriggers(data, byUser);
        return null;
    }
    public PyObject? DumpTimers(params PyObject[] args)
    {
        using var _ = Py.GIL();
        var byUser = GetBoolArg(args, 0);
        return _api.DumpTimers(byUser).ToPython();
    }
    public PyObject? RestoreTimers(params PyObject[] args)
    {
        using var _ = Py.GIL();
        var data = GetStringArg(args, 0);
        var byUser = GetBoolArg(args, 1);
        _api.RestoreTimers(data, byUser);
        return null;
    }
    public PyObject? DumpAliases(params PyObject[] args)
    {
        using var _ = Py.GIL();
        var byUser = GetBoolArg(args, 0);
        return _api.DumpAliases(byUser).ToPython();
    }
    public PyObject? RestoreAliases(params PyObject[] args)
    {
        using var _ = Py.GIL();
        var data = GetStringArg(args, 0);
        var byUser = GetBoolArg(args, 1);
        _api.RestoreAliases(data, byUser);
        return null;
    }
    public PyObject? SetHUDSize(params PyObject[] args)
    {
        using var _ = Py.GIL();
        var size = GetIntArg(args, 0);
        _api.SetHUDSize(size);

        return null;
    }
    public PyObject? GetHUDContent(params PyObject[] args)
    {
        using var _ = Py.GIL();
        var content = _api.GetHUDContent();
        return content.ToPython();
    }
    public PyObject? GetHUDSize(params PyObject[] args)
    {
        using var _ = Py.GIL();
        var size = _api.GetHUDSize();
        return size.ToPython();
    }
    public PyObject? UpdateHUD(params PyObject[] args)
    {
        using var _ = Py.GIL();
        var start = GetIntArg(args, 0);
        var content = GetStringArg(args, 1);
        var result = _api.UpdateHUD(start, content);
        return result.ToPython();
    }
    public PyObject? NewLine(params PyObject[] args)
    {
        using var _ = Py.GIL();
        return _api.NewLine().ToPython();
    }
    public PyObject? NewWord(params PyObject[] args)
    {
        using var _ = Py.GIL();
        var text = GetStringArg(args, 0);
        return _api.NewWord(text).ToPython();
    }

    public PyObject? SetPriority(params PyObject[] args)
    {
        using var _ = Py.GIL();
        var value = GetIntArg(args, 0);
        _api.SetPriority(value);
        return null;
    }
    public PyObject? GetPriority(params PyObject[] args)
    {
        using var _ = Py.GIL();
        return _api.GetPriority().ToPython();
    }
    public PyObject? SetSummary(params PyObject[] args)
    {
        using var _ = Py.GIL();
        var content = GetStringArg(args, 0);
        _api.SetSummary(content);
        return null;
    }
    public PyObject? GetSummary(params PyObject[] args)
    {
        using var _ = Py.GIL();
        return _api.GetSummary().ToPython();
    }
    public PyObject? Save(params PyObject[] args)
    {
        using var _ = Py.GIL();
        return _api.Save().ToPython();
    }
    public PyObject? Milliseconds(params PyObject[] args)
    {
        using var _ = Py.GIL();
        return _api.Milliseconds().ToPython();
    }

    public PyObject? OmitOutput(params PyObject[] args)
    {
        using var _ = Py.GIL();
        _api.OmitOutput();
        return null;
    }
    public PyObject? AddAnsi(params PyObject[] args)
    {
        using var _ = Py.GIL();
        var data = GetStringArg(args, 0);
        _api.AddAnsi(data);
        return null;
    }
    public PyObject? LastAnsi(params PyObject[] args)
    {
        using var _ = Py.GIL();
        return _api.LastAnsi().ToPython();
    }
    public PyObject? GetScriptPath(params PyObject[] args)
    {
        using var _ = Py.GIL();
        return _api.GetScriptPath().ToPython();
    }

}