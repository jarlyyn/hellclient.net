namespace Hellclient.Core.Infras.Components;

using System.Net.WebSockets;
using System.Text;
using Hellclient.World.Configs;

public class HellSwitch
{
    public static readonly byte[] CmdBroadcast = Encoding.UTF8.GetBytes("broadcast ");
    public static readonly byte[] CmdHello = Encoding.UTF8.GetBytes("hello ");
    public static readonly byte[] CmdPing = Encoding.UTF8.GetBytes("ping ");
    public const int StatusDisabled = 0;
    public const int StatusDisconnected = 1;
    public const int StatusConnected = 2;
    public static readonly TimeSpan ReconnectDuration = TimeSpan.FromSeconds(30);
    public static readonly TimeSpan PingDuration = TimeSpan.FromSeconds(30);
    private PeriodicTimer PingTimer { get; set; } = new PeriodicTimer(PingDuration);
    private PeriodicTimer ReconnectTimer { get; set; } = new PeriodicTimer(ReconnectDuration);
    private SemaphoreSlim Lock { get; set; } = new SemaphoreSlim(1, 1);

    private volatile ClientWebSocket? _conn = null;
    public HellSwitch()
    {
        PingDamon();
        ReconnectDamon();
    }
    private async void PingDamon()
    {
        while (true)
        {
            await PingTimer.WaitForNextTickAsync();
            await Ping();
        }
    }

    private async void ReconnectDamon()
    {
        while (true)
        {
            await ReconnectTimer.WaitForNextTickAsync();
            Start();
        }
    }
    private async Task close()
    {
        await Lock.WaitAsync();
        try
        {
            if (_conn == null)
            {
                return;
            }
            _conn?.Dispose();
            _conn = null;
        }
        finally
        {
            Lock.Release();
        }
    }
    public int Status()
    {
        Lock.Wait();
        try
        {
            if (_conn == null)
            {
                return StatusDisabled;
            }
            if (_conn.State == WebSocketState.Open)
            {
                return StatusConnected;
            }
            return StatusDisconnected;
        }
        finally
        {
            Lock.Release();
        }
    }
    public async Task Ping()
    {
        await Send(CmdPing);
    }
    public async Task Broadcast(byte[] msg)
    {
        using var ms = new MemoryStream();
        {
            ms.Write(CmdBroadcast, 0, CmdBroadcast.Length);
            ms.Write(msg, 0, msg.Length);
            await Send(ms.ToArray());
        }
    }
    private async Task Send(byte[] msg)
    {
        await Lock.WaitAsync();
        try
        {
            if (_conn == null || _conn.State != WebSocketState.Open)
            {
                return;
            }
            await _conn.SendAsync(new ArraySegment<byte>(msg), WebSocketMessageType.Text, true, CancellationToken.None);
        }
        finally
        {
            Lock.Release();
        }
    }
    private async Task Listen()
    {
        byte[] buffer = new byte[4096];
        _ = Send(CmdHello);
        while (_conn != null && _conn.State == WebSocketState.Open)
        {
            WebSocketReceiveResult result;
            try
            {
                result = await _conn.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            }
            catch (Exception)
            {
                break;
            }
            if (result.MessageType == WebSocketMessageType.Close)
            {
                break;
            }
            if (result.MessageType == WebSocketMessageType.Text)
            {
                OnGlobalMessage?.Invoke(this, buffer.Take(result.Count).ToArray());
            }
        }
        await close();
        OnSwitchStatusChange?.Invoke(this, StatusDisconnected);
    }
    public void Start()
    {
        Lock.Wait();
        try
        {
            if (_conn != null)
            {
                return;
            }
            var s = AppConfig.System.Switch;
            if (s == "")
            {
                return;
            }
            var u = new Uri(s);
            _conn = new ClientWebSocket();
            var ui = u.UserInfo;
            if (ui != "")
            {
                _conn.Options.SetRequestHeader("Authorization", "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(ui)));
            }
            _conn.ConnectAsync(u, CancellationToken.None).Wait();
            Task.Run(Listen);
            OnSwitchStatusChange?.Invoke(this, StatusConnected);
        }
        catch (Exception)
        {
            close().Wait();
            OnSwitchStatusChange?.Invoke(this, StatusDisconnected);
        }
        finally
        {
            Lock.Release();
        }
    }
    public void Stop()
    {
        Lock.Wait();
        try
        {
            if (_conn != null)
            {
                _conn.CloseAsync(WebSocketCloseStatus.NormalClosure, "Stopping", CancellationToken.None).Wait();
            }

        }
        finally
        {
            Lock.Release();
        }
    }
    public EventHandler<byte[]>? OnGlobalMessage { get; set; }
    public EventHandler<int>? OnSwitchStatusChange { get; set; }
}
