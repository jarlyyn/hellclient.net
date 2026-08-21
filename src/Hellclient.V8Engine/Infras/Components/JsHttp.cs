using System.Text;
using Hellclient.World.Cores;
using Hellclient.World.Types;
using Microsoft.ClearScript;
using Microsoft.ClearScript.V8;

namespace Hellclient.V8Engine.Infras.Components;

public class JsHTTPRequest(IWorld world, V8ScriptEngine engine, HttpRequest httpRequest)
{
    private readonly IWorld _world = world;

    private readonly V8ScriptEngine _engine = engine;
    private readonly HttpRequest _httpRequest = httpRequest;
    public Object? GetID(params object[] args)
    {
        return _httpRequest.GetID();
    }
    public Object? SetURL(params object[] args)
    {
        _httpRequest.SetURL(JsAPI.GetStringArg(args, 0));
        return null;
    }
    public Object? GetURL(params object[] args)
    {
        return _httpRequest.GetURL();
    }
    public Object? SetMethod(params object[] args)
    {
        _httpRequest.SetMethod(JsAPI.GetStringArg(args, 0));
        return null;

    }
    public Object? GetMethod(params object[] args)
    {
        return _httpRequest.GetMethod();
    }
    public Object? SetProxy(params object[] args)
    {
        _httpRequest.SetProxy(JsAPI.GetStringArg(args, 0));
        return null;
    }
    public Object? GetProxy(params object[] args)
    {
        return _httpRequest.GetProxy();
    }
    public Object? SetHeader(params object[] args)
    {
        _httpRequest.SetHeader(JsAPI.GetStringArg(args, 0), JsAPI.GetStringArg(args, 1));
        return null;
    }
    public Object? AddHeader(params object[] args)
    {
        _httpRequest.AddHeader(JsAPI.GetStringArg(args, 0), JsAPI.GetStringArg(args, 1));
        return null;
    }
    public Object? DeleteHeader(params object[] args)
    {
        _httpRequest.DeleteHeader(JsAPI.GetStringArg(args, 0));
        return null;
    }
    public Object? GetHeader(params object[] args)
    {
        return _httpRequest.GetHeader(JsAPI.GetStringArg(args, 0));
    }
    public Object? ResetHeaders(params object[] args)
    {
        _httpRequest.ResetHeaders();
        return null;
    }
    public Object? HeaderValues(params object[] args)
    {
        return _httpRequest.HeaderValues(JsAPI.GetStringArg(args, 0));
    }
    public Object? HeaderFields(params object[] args)
    {
        return _httpRequest.HeaderFields();
    }
    public Object? SetBody(params object[] args)
    {
        _httpRequest.SetBody(Encoding.UTF8.GetBytes(JsAPI.GetStringArg(args, 0)));
        return null;
    }
    public Object? GetBody(params object[] args)
    {
        return Encoding.UTF8.GetString(_httpRequest.GetBody());
    }
    public Object? FinishedAt(params object[] args)
    {
        return _httpRequest.FinishedAt();
    }
    public Object? ResponseStatusCode(params object[] args)
    {
        return _httpRequest.ResponseStatusCode();
    }
    public Object? ResponseBody(params object[] args)
    {
        return Encoding.UTF8.GetString(_httpRequest.ResponseBody());
    }
    public Object? ResponseHeader(params object[] args)
    {
        return _httpRequest.ResponseHeader(JsAPI.GetStringArg(args, 0));
    }
    public Object? ResponseHeaderValues(params object[] args)
    {
        return _httpRequest.ResponseHeaderValues(JsAPI.GetStringArg(args, 0));
    }
    public Object? ResponseHeaderFields(params object[] args)
    {
        return _httpRequest.ResponseHeaderFields();
    }
    public Object? AsyncExecute(params object[] args)
    {
        var callback = "";
        if (JsAPI.GetBoolArg(args, 0))
        {
            callback = JsAPI.GetStringArg(args, 0);
        }
        _httpRequest.AsyncExecute(_world.GetPluginOptions(), () =>
        {
            if (callback != "")
            {
                var cb = new Callback();
                cb.Name = "httpexecute";
                cb.ID = _httpRequest.GetID();
                cb.Script = callback;
                if (_httpRequest.Status == HttpRequest.StatusSuccess)
                {
                    cb.Code = 0;
                    cb.Data = _httpRequest.GetURL();
                }
                else
                {
                    cb.Code = -1;
                    cb.Data = _httpRequest.Response?.ErrorMessage ?? "";
                }
            }
        });
        return null;
    }
    public ScriptObject Convert()
    {
        var m = _engine.Evaluate("({})") as ScriptObject;
        if (m == null)
        {
            throw new Exception("Failed to create script object");
        }
#pragma warning disable CS8974 // 将方法组转换为非委托类型
        m["GetID"] = GetID;
        m["SetURL"] = SetURL;
        m["GetURL"] = GetURL;
        m["SetMethod"] = SetMethod;
        m["GetMethod"] = GetMethod;
        m["SetProxy"] = SetProxy;
        m["GetProxy"] = GetProxy;
        m["SetHeader"] = SetHeader;
        m["AddHeader"] = AddHeader;
        m["DeleteHeader"] = DeleteHeader;
        m["GetHeader"] = GetHeader;
        m["ResetHeaders"] = ResetHeaders;
        m["HeaderValues"] = HeaderValues;
        m["HeaderFields"] = HeaderFields;
        m["SetBody"] = SetBody;
        m["GetBody"] = GetBody;
        m["FinishedAt"] = FinishedAt;
        m["ResponseStatusCode"] = ResponseStatusCode;
        m["ResponseBody"] = ResponseBody;
        m["ResponseHeader"] = ResponseHeader;
        m["ResponseHeaderValues"] = ResponseHeaderValues;
        m["ResponseHeaderFields"] = ResponseHeaderFields;
        m["AsyncExecute"] = AsyncExecute;
#pragma warning restore CS8974 // 将方法组转换为非委托类型
        return m;
    }
}
public class JsHttp(IWorld world, V8ScriptEngine engine)
{
    private readonly IWorld _world = world;
    private readonly V8ScriptEngine _engine = engine;
    public Object? NewRequest(params object[] args)
    {
        var request = new HttpRequest()
        {
            Method = JsAPI.GetStringArg(args, 0),
            URL = JsAPI.GetStringArg(args, 1),
        };
        return new JsHTTPRequest(_world, _engine, request).Convert();
    }
    public Object? PraseURL(params object[] args)
    {
        var rawurl = JsAPI.GetStringArg(args, 0);
        try
        {
            var u = new Uri(rawurl);
            var result = _engine.Evaluate("({})") as ScriptObject;
            if (result is null)
            {
                throw new Exception("Failed to create script object");
            }
            result["Scheme"] = u.Scheme;
            result["Host"] = u.Host;
            result["Port"] = u.Port.ToString();
            result["Path"] = u.LocalPath;
            result["Query"] = u.Query;
            result["Fragment"] = u.Fragment;
            var ui = u.UserInfo.Split(':', 2);
            result["User"] = ui.Length > 0 ? ui[0] : "";
            result["Password"] = ui.Length > 1 ? ui[1] : "";
            return result;
        }
        catch (Exception)
        {
            return null;
        }
    }
    public ScriptObject Convert()
    {
        var m = _engine.Evaluate("({})") as ScriptObject;
        if (m == null)
        {
            throw new Exception("Failed to create script object");
        }
#pragma warning disable CS8974 // 将方法组转换为非委托类型
        m["ParseURL"] = PraseURL;
        m["New"] = NewRequest;
        return m;
#pragma warning restore CS8974 // 将方法组转换为非委托类型
    }
}