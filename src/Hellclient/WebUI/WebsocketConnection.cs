using System.Net.WebSockets;
using System.Text;
using Hellclient.Core.Types;
using Hellclient.World.Infras.Adapters;

namespace Hellclient.WebUI;

public class WebsocketConnection : IConnection
{
    public WebsocketConnection(System.Net.WebSockets.WebSocket socket)
    {
        _socket = socket;
        id = SimpleID.Instance.GenerateID();
    }
    private string id { get; init; }
    private System.Net.WebSockets.WebSocket _socket { init; get; }
    public async Task Run()
    {
        var buffer = new Memory<byte>(new byte[4096]);
        try
        {
            while (_socket.State == WebSocketState.Open)
            {
                using var ms = new MemoryStream();
                ValueWebSocketReceiveResult result;
                do
                {
                    try
                    {
                        result = await _socket.ReceiveAsync(buffer, CancellationToken.None);
                    }
                    catch
                    {
                        return;
                    }
                    if (result.MessageType == WebSocketMessageType.Close)
                    {
                        return;
                    }
                    ms.Write(buffer.Span[..result.Count]);

                } while (!result.EndOfMessage);
                ms.Seek(0, SeekOrigin.Begin);
                OnMessage?.Invoke(this, ms.ToArray());
            }
        }
        finally
        {
            await Close();
        }
    }
    public async Task Close()
    {
        OnClose?.Invoke(this, EventArgs.Empty);
        if (_socket.State == WebSocketState.Open)
        {
            await _socket.CloseAsync(WebSocketCloseStatus.NormalClosure,
                "",
                System.Threading.CancellationToken.None);
        }
    }
    public async Task Send(byte[] data)
    {
        await _socket.SendAsync(
            new ArraySegment<byte>(data, 0, data.Count()),
            System.Net.WebSockets.WebSocketMessageType.Text,
            true,
            System.Threading.CancellationToken.None);

    }
    public string ID()
    {
        return id;
    }
    public EventHandler<byte[]>? OnMessage { get; set; }
    public EventHandler? OnClose { get; set; }

}