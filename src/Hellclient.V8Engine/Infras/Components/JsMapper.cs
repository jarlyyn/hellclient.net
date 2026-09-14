using Hellclient.World.Cores;
using Hellclient.World.Infras.Components;
using Hellclient.World.Types;
using Microsoft.ClearScript;
using Microsoft.ClearScript.V8;


namespace Hellclient.V8Engine.Infras.Components;

public class JsWalkAllResult(WalkAllResult result)
{
    public WalkAllResult Result { get; set; } = result;
    public ScriptObject Convert(V8ScriptEngine _engine)
    {
        var m = _engine.Evaluate("({})") as Microsoft.ClearScript.ScriptObject ?? throw new Exception("Failed to create script object");
        var steps=_engine.Evaluate("([])") as Microsoft.ClearScript.ScriptObject ?? throw new Exception("Failed to create script object");
        Result.Steps.ForEach(step => steps.InvokeMethod("push", new JsStep(step).Convert(_engine)));
        m["steps"] =steps;
        m["walked"] = Result.Walked;
        m["notwalked"] = Result.NotWalked;
        return m;
    }
}
public class JsStep(Step step)
{
    public Step step { get; set; } = step;
    public ScriptObject Convert(V8ScriptEngine _engine)
    {
        var m = _engine.Evaluate("({})") as Microsoft.ClearScript.ScriptObject;
        if (m == null)
        {
            throw new Exception("Failed to create script object");
        }
        m["to"] = step.To;
        m["from"] = step.From;
        m["command"] = step.Command;
        m["delay"] = step.Delay;
        return m;
    }
}

public class JsPath(WalkPath path)
{
    public WalkPath path { get; set; } = path;
    public ScriptObject Convert(V8ScriptEngine engine)
    {
        var m = engine.Evaluate("({})") as Microsoft.ClearScript.ScriptObject ?? throw new Exception("Failed to create script object");
        m["command"] = path.Command;
        m["from"] = path.From;
        m["to"] = path.To;
        m["delay"] = path.Delay;
        var tags = new List<string>();
        path.Tags.ToList().ForEach(tag => { if (tag.Value) tags.Add(tag.Key); });
        m["tags"] = tags;
        var excluds = new List<string>();
        path.ExcludeTags.ToList().ForEach(exclude => { if (exclude.Value) excluds.Add(exclude.Key); });
        m["excludetags"] = excluds;
        return m;
    }
    public static JsPath FromJS(ScriptObject? obj)
    {
        var path = new WalkPath();
        if (obj != null)
        {
            path.Command = obj.GetProperty("command") as string ?? string.Empty;
            path.From = obj.GetProperty("from") as string ?? string.Empty;
            path.To = obj.GetProperty("to") as string ?? string.Empty;
            path.Delay = obj.GetProperty("delay") as int? ?? 0;
            var tags = obj.GetProperty("tags") as ScriptObject;
            if (tags != null)
            {
                path.Tags.Clear();
                var jstags = JsAPI.StringArrayFromObject(tags);
                jstags.ForEach(tag => path.Tags[tag] = true);
            }
            var excluds = obj.GetProperty("excludetags") as ScriptObject;
            if (excluds != null)
            {
                path.ExcludeTags.Clear();
                var jsexcludes = JsAPI.StringArrayFromObject(excluds);
                jsexcludes.ForEach(exclude => path.ExcludeTags[exclude] = true);
            }
        }
        return new JsPath(path);
    }
}

public class JsMapper(Mapper mapper, V8ScriptEngine engine)
{
    private readonly Mapper _mapper = mapper;
    private readonly V8ScriptEngine _engine = engine;

    public ScriptObject ConvertWalkAllResult(WalkAllResult result)
    {
        var m = _engine.Evaluate("({})") as Microsoft.ClearScript.ScriptObject ?? throw new Exception("Failed to create script object");
        var steps = _engine.Evaluate("[]") as Microsoft.ClearScript.ScriptObject ?? throw new Exception("Failed to create script object");
        foreach (var step in result.Steps)
        {
            var jsStep = new JsStep(step);
            steps.InvokeMethod("push", jsStep.Convert(_engine));
        }
        m["steps"] = steps;
        m["walked"] = result.Walked;
        m["notwalked"] = result.NotWalked;
        return m;
    }
    public Object? Reset(params object[] args)
    {
        _mapper.Reset();
        return null;
    }
    public Object? ResetTemporary(params object[] args)
    {
        _mapper.ResetTemporary();
        return null;
    }
    public Object? AddTags(params object[] args)
    {
        var tags = new List<string>();
        foreach (var arg in args)
        {
            tags.Add(JsAPI.ConvertString(arg));
        }
        _mapper.AddTags(tags);
        return null;
    }
    public Object? SetTag(params object[] args)
    {
        _mapper.SetTag(JsAPI.GetStringArg(args, 0), JsAPI.GetBoolArg(args, 1));
        return null;
    }
    public Object? FlushTags(params object[] args)
    {
        _mapper.FlushTags();
        return null;
    }
    public Object? SetTags(params object[] args)
    {
        var tags = JsAPI.GetStringArrayArg(args, 0);
        _mapper.AddTags(tags);
        return null;
    }
    public Object? Tags(params object[] args)
    {
        var tags = _mapper.Tags;
        return tags;
    }
    public Option OptionFromJS(object? obj)
    {
        var opt = new Option();
        if (obj is not null && obj is ScriptObject so)
        {
            var jbl = JsAPI.StringArrayFromObject(so["blacklist"] as ScriptObject);
            var jwl = JsAPI.StringArrayFromObject(so["whitelist"] as ScriptObject);
            var blockedpath = new List<List<string>>();
            var bp = so["blockedpath"] as ScriptObject;
            if (bp != null)
            {
                for (int i = 0; i < (bp.GetProperty("length") as int? ?? 0); i++)
                {
                    var blocked = JsAPI.StringArrayFromObject(bp.GetProperty(i) as ScriptObject);
                    if (blocked.Count > 0)
                    {
                        blockedpath.Add(blocked);
                    }
                }
            }
            opt.Blacklist = jbl;
            opt.Whitelist = jwl;
            opt.BlockedPath = blockedpath;
        }
        return opt;

    }
    public Object? WalkAll(params object[] args)
    {
        var targets = JsAPI.GetStringArrayArg(args, 0);
        var fly = JsAPI.GetBoolArg(args, 1);
        var maxDistance = JsAPI.GetIntArg(args, 2);
        var v = JsAPI.GetArg(args, 3);
        var opt = OptionFromJS(v);
        var result = _mapper.WalkAll(targets, fly, maxDistance, opt);
        if (result == null)
        {
            return null;
        }
        var jsresult = new JsWalkAllResult(result);
        return jsresult.Convert(_engine);
    }
    public Object? GetPath(params object[] args)
    {
        if (args.Length < 3)
        {
            return null;
        }
        var form = JsAPI.GetStringArg(args, 0);
        var fly = JsAPI.GetBoolArg(args, 1);
        var to = JsAPI.GetStringArrayArg(args, 2);
        var opt = OptionFromJS(JsAPI.GetArg(args, 3));
        var path = _mapper.GetPath(form, fly, to, opt);
        if (path == null)
        {
            return null;
        }
        var steps=_engine.Evaluate("([])") as Microsoft.ClearScript.ScriptObject ?? throw new Exception("Failed to create script object");
        path.ForEach(step => steps.InvokeMethod("push", new JsStep(step).Convert(_engine)));
        return steps;
    }
    public Object? AddPath(params object[] args)
    {
        if (args.Length < 2)
        {
            return null;
        }
        var id = JsAPI.GetStringArg(args, 0);
        var path = JsPath.FromJS(args[1] as ScriptObject);
        var result = _mapper.AddPath(id, path.path);
        return result;
    }
    public Object? AddTemporaryPath(params object[] args)
    {
        if (args.Length < 2)
        {
            return null;
        }
        var id = JsAPI.GetStringArg(args, 0);
        var path = JsPath.FromJS(args[1] as ScriptObject);
        var result = _mapper.AddTemporaryPath(id, path.path);
        return result;
    }
    public Object? NewPath(params object[] args)
    {
        return new JsPath(new WalkPath()).Convert(_engine);
    }
    public Object? GetRoomID(params object[] args)
    {
        var name = JsAPI.GetStringArg(args, 0);
        var ids = _mapper.GetRoomID(name);
        return ids;
    }
    public Object? GetRoomName(params object[] args)
    {
        return _mapper.GetRoomName(JsAPI.GetStringArg(args, 0));
    }
    public Object? SetRoomName(params object[] args)
    {
        _mapper.SetRoomName(JsAPI.GetStringArg(args, 0), JsAPI.GetStringArg(args, 1));
        return null;
    }
    public Object? ClearRoom(params object[] args)
    {
        _mapper.ClearRoom(JsAPI.GetStringArg(args, 0));
        return null;
    }
    public Object? RemoveRoom(params object[] args)
    {
        var id = JsAPI.GetStringArg(args, 0);
        var result = _mapper.RemoveRoom(id);
        return result;
    }
    public Object? NewArea(params object[] args)
    {
        return _mapper.NewArea(JsAPI.GetIntArg(args, 0));
    }
    public Object? GetExits(params object[] args)
    {
        var id = JsAPI.GetStringArg(args, 0);
        var exits = _mapper.GetExits(id, JsAPI.GetBoolArg(args, 1));
        return exits.Select(e => new JsPath(e).Convert(_engine)).ToArray();
    }
    public Object? SetFlyList(params object[] args)
    {
        var result = new List<WalkPath>();
        var flv = JsAPI.GetArg(args, 0);
        if (flv is not null && flv is ScriptObject so)
        {
            var l = JsAPI.LoadArray(so);
            foreach (var item in l)
            {
                var path = JsPath.FromJS(item);
                if (path != null)
                {
                    result.Add(path.path);
                }
            }
        }
        return result;
    }
    public Object? FlyList(params object[] args)
    {
        var result = _mapper.FlyList();
        return result.Select(e => new JsPath(e).Convert(_engine)).ToArray();
    }

    public ScriptObject Convert()
    {
        var m = _engine.Evaluate("({})") as Microsoft.ClearScript.ScriptObject ?? throw new Exception("Failed to create script object");
#pragma warning disable CS8974 // 将方法组转换为非委托类型
        m["reset"] = Reset;
        m["Reset"] = Reset;
        m["resettemporary"] = ResetTemporary;
        m["ResetTemporary"] = ResetTemporary;
        m["addtags"] = AddTags;
        m["AddTags"] = AddTags;
        m["settag"] = SetTag;
        m["SetTag"] = SetTag;
        m["settags"] = SetTags;
        m["SetTags"] = SetTags;
        m["tags"] = Tags;
        m["Tags"] = Tags;
        m["getpath"] = GetPath;
        m["GetPath"] = GetPath;
        m["addpath"] = AddPath;
        m["AddPath"] = AddPath;
        m["addtemporarypath"] = AddTemporaryPath;
        m["AddTemporaryPath"] = AddTemporaryPath;

        m["newpath"] = NewPath;
        m["NewPath"] = NewPath;
        m["getroomid"] = GetRoomID;
        m["GetRoomID"] = GetRoomID;
        m["removeroom"] = RemoveRoom;
        m["RemoveRoom"] = RemoveRoom;

        m["getroomname"] = GetRoomName;
        m["GetRoomName"] = GetRoomName;
        m["setroomname"] = SetRoomName;
        m["SetRoomName"] = SetRoomName;
        m["clearroom"] = ClearRoom;
        m["ClearRoom"] = ClearRoom;
        m["newarea"] = NewArea;
        m["NewArea"] = NewArea;
        m["getexits"] = GetExits;
        m["GetExits"] = GetExits;
        m["flushtags"] = FlushTags;
        m["FlushTags"] = FlushTags;
        m["flashtags"] = FlushTags;
        m["FlashTags"] = FlushTags;
        m["flylist"] = FlyList;
        m["FlyList"] = FlyList;
        m["setflylist"] = SetFlyList;
        m["SetFlyList"] = SetFlyList;
        m["WalkAll"] = WalkAll;
        m["walkall"] = WalkAll;

#pragma warning restore CS8974 // 将方法组转换为非委托类型
        return m;
    }

}