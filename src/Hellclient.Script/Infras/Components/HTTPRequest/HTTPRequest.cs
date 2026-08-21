using static System.Net.WebRequestMethods;

using System.Net.Http;
using System.Net.Http.Headers;
using Hellclient.World.Types;
using Hellclient.World.Helpers;

public class HttpRequestResponse
{
    public required string ErrorMessage { get; set; } = "";
    public required Int64 FinishedAt { get; set; }
    public required int StatusCode { get; set; }
    public required HttpHeaders Headers { get; set; }
    public required byte[] Body { get; set; }
}
public class HttpRequest
{
    public const int StatusReady = 0;
    public const int StatusExecuting = 1;
    public const int StatusSuccess = 2;
    public const int StatusFail = 3;
    private static readonly HttpClient client = new HttpClient();

    public const string permission = "http";
    public static readonly Exception RequestNotExecuted = new Exception("request not executed");
    public static readonly Exception RequestExecuted = new Exception("request executed");
    public string ID = "";
    public string Method { get; set; } = "";
    public string URL { get; set; } = "";
    public string Proxy { get; set; } = "";
    public byte[] Body { get; set; } = Array.Empty<byte>();
    public int Status { get; set; } = StatusReady;

    public HttpRequestResponse? Response { get; set; } = null;
    public Dictionary<string, List<string>> Headers = new();

    public void SetProxy(string proxy)
    {
        Proxy = proxy;
    }
    public string GetProxy()
    {
        return Proxy;
    }
    public string GetID()
    {
        return ID;
    }
    public string GetURL()
    {
        return URL;
    }
    public void SetURL(string url)
    {
        URL = url;
    }
    public string GetMethod()
    {
        return Method;
    }
    public void SetMethod(string method)
    {
        Method = method;
    }
    public byte[] GetBody()
    {
        return Body;
    }
    public void SetBody(byte[] body)
    {
        Body = body;
    }
    public void SetHeader(string key, string value)
    {
        Headers[key] = new() { value };
    }
    public void AddHeader(string key, string value)
    {
        if (!Headers.ContainsKey(key))
        {
            Headers[key] = new();
        }
        Headers[key].Add(value);
    }
    public void DeleteHeader(string key)
    {
        Headers.Remove(key);
    }
    public void ResetHeaders()
    {
        Headers.Clear();
    }
    public string GetHeader(string key)
    {
        if (Headers.TryGetValue(key, out var values))
        {
            if (values is not null && values.Count() > 0)
            {
                return values.First();
            }
        }
        return "";
    }
    public List<string> HeaderValues(string key)
    {
        if (Headers.TryGetValue(key, out var values))
        {
            if (values is not null && values.Count() > 0)
            {
                return values.ToList();
            }
        }
        return new List<string>();
    }
    public List<string> HeaderFields()
    {
        return Headers.Select(h => h.Key).ToList();
    }
    public Int64 FinishedAt()
    {
        if (Response is null)
        {
            throw RequestNotExecuted;
        }
        return Response.FinishedAt;
    }
    public byte[] ResponseBody()
    {
        if (Response is null)
        {
            throw RequestNotExecuted;
        }
        return Response.Body;
    }
    public int ResponseStatusCode()
    {
        if (Response is null)
        {
            throw RequestNotExecuted;
        }
        return Response.StatusCode;
    }
    public string ResponseHeader(string key)
    {
        if (Response is null)
        {
            throw RequestNotExecuted;
        }
        if (Response.Headers.TryGetValues(key, out var values))
        {
            if (values is not null && values.Count() > 0)
            {
                return values.First();
            }
        }
        return "";
    }
    public List<string> ResponseHeaderValues(string key)
    {
        if (Response is null)
        {
            throw RequestNotExecuted;
        }
        if (Response.Headers.TryGetValues(key, out var values))
        {
            if (values is not null && values.Count() > 0)
            {
                return values.ToList();
            }
        }
        return new List<string>();
    }
    public List<string> ResponseHeaderFields()
    {
        if (Response is null)
        {
            throw RequestNotExecuted;
        }
        return Response.Headers.Select(h => h.Key).ToList();
    }
    public int ExecuteStatus()
    {
        return Status;
    }
    public void AsyncExecute(PlainOptions opts, Action callback)
    {
        authorize(opts);
        Task.Run(async () =>
        {
            Execute(opts);
            callback();
        });
    }
    private void authorize(PlainOptions opts)
    {
        if (!AuthorizeHelper.AuthorizePermission(opts.Permissions, permission))
        {
            throw new Exception($"Permission denied: {permission}");
        }
        var u = new Uri(URL);
        if (!AuthorizeHelper.AuthorizeDomain(opts.Trusted, u.Host))
        {
            throw new Exception($"Domain not allowed: {u.Host}");
        }
        var h = GetHeader("Host");
        if (h != "" && !AuthorizeHelper.AuthorizeDomain(opts.Trusted, h))
        {
            throw new Exception($"Domain not allowed: {u.Host}");
        }

    }
    public void Execute(PlainOptions opts)
    {
        lock (this)
        {
            if (Status != StatusReady)
            {
                throw RequestExecuted;
            }
            authorize(opts);
            var request = new HttpRequestMessage(new HttpMethod(Method), URL);
            foreach (var header in Headers)
            {
                request.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }
            if (Body.Length > 0)
            {
                request.Content = new ByteArrayContent(Body);

            }
            Status = StatusExecuting;
            try
            {
                var resp = client.SendAsync(request).GetAwaiter().GetResult();
                if (resp is null)
                {
                    Response = new HttpRequestResponse()
                    {
                        ErrorMessage = "null",
                        FinishedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                        StatusCode = 0,
                        Headers = new HttpResponseMessage().Headers,
                        Body = new byte[0]
                    };
                    Status = StatusFail;
                    return;
                }
                var responseBody = resp?.Content.ReadAsByteArrayAsync().GetAwaiter().GetResult() ?? new byte[0];
                Response = new HttpRequestResponse()
                {
                    ErrorMessage = "",
                    FinishedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                    StatusCode = (int)resp!.StatusCode,
                    Headers = resp.Headers,
                    Body = responseBody
                };
                Status = StatusSuccess;

            }
            catch (Exception ex)
            {
                Status = StatusFail;
                Response = new HttpRequestResponse()
                {
                    ErrorMessage = ex.Message,
                    FinishedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                    StatusCode = 0,
                    Headers = new HttpResponseMessage().Headers,
                    Body = new byte[0]
                };
                Status = StatusFail;
            }
        }
    }
}