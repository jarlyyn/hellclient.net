using System.Net.Sockets;
using Hellclient.World.Types;

namespace Hellclient.World.Infras.Components;

public class Telnet : IMudConnection
{
    public const int StatusNormal = 0;
    public const int StatusIAC = 1;
    public const int StatusCmd = 2;
    public const int StatusSb = 3;
    public const int StatusSbIac = 4;
    private TcpClient _client { get; set; } = new TcpClient();
    private NetworkStream? _stream;
    private CancellationTokenSource _cts = new CancellationTokenSource();
    public string Host { get; set; } = "";
    public int Port { get; set; } = 0;
    private List<byte> _buffer = [];
    private int status = StatusNormal;
    private byte currentcmd = 0;
    public required Action<Exception> Logger;
    public EventHandler<byte>? OnDataReceived { get; set; }
    public EventHandler<TelnetCommand>? OnCommandReceived { get; set; }

    public EventHandler? OnDisconnected { get; set; }
    public EventHandler? OnConnected { get; set; }
    private void reset()
    {
        _buffer.Clear();
        status = StatusNormal;
    }
    private void Publish(byte data)
    {
        OnDataReceived?.Invoke(this, data);
    }
    private void OnByte(byte data)
    {
        switch (status)
        {
            case StatusNormal:
                if (data == TelnetCommand.CmdIAC)
                {
                    status = StatusIAC;
                }
                else
                {
                    Publish(data);
                }
                return;
            case StatusIAC:
                if (data == TelnetCommand.CmdIAC)
                {
                    status = StatusNormal;
                    Publish(data);
                }
                else
                {
                    switch (data)
                    {
                        case TelnetCommand.CmdGoAhead:
                        case TelnetCommand.CmdEraseLine:
                            status = StatusNormal;
                            OnCommandReceived?.Invoke(this, new TelnetCommand(data, [data]));
                            break;
                        case TelnetCommand.CmdSubnegotiation:
                            status = StatusSb;
                            break;
                        default:
                            currentcmd = data;
                            status = StatusCmd;
                            break;
                    }
                }
                return;
            case StatusCmd:
                status = StatusNormal;
                OnCommandReceived?.Invoke(this, new TelnetCommand(currentcmd, [data]));
                return;
            case StatusSb:
                if (data == 0xFF) // IAC
                {
                    status = StatusSbIac;
                }
                else
                {
                    _buffer.Add(data);
                }
                return;
            case StatusSbIac:
                if (data == 0xFF) // IAC
                {
                    _buffer.Add(data);
                    status = StatusSb;
                }
                else if (data == TelnetCommand.CmdEndSubnegotiation)
                {
                    OnCommandReceived?.Invoke(this, new TelnetCommand(TelnetCommand.CmdSubnegotiation, [.. _buffer]));
                    _buffer.Clear();
                    status = StatusNormal;
                }
                else
                {
                    _buffer.Clear();
                    status = StatusNormal;
                }
                return;
        }
    }
    private async Task Connected()
    {
        OnConnected?.Invoke(this, EventArgs.Empty);
    }
    private async Task Disconnected()
    {
        OnDisconnected?.Invoke(this, EventArgs.Empty);
    }
    private async Task listen()
    {
        _cts = new CancellationTokenSource();
        using (NetworkStream stream = _client.GetStream())
        {
            _stream = stream;
            byte[] buffer = new byte[1];
            while (_client.Connected)
            {
                int bytesRead = 0;
                try
                {
                    bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length, _cts.Token);
                }
                catch (Exception ex)
                {
                    Logger(ex);
                    _ = Disconnected();
                    return;
                }

                if (bytesRead == 0)
                {
                    break;
                }

                OnByte(buffer[0]);

            }
            _ = Disconnected();
        }

    }
    public async Task Connect(string host, int port)
    {
        Host = host;
        Port = port;
        await Disconnect();
        _client = new TcpClient();
        await _client.ConnectAsync(host, port);
        await Connected();
        _ = listen();
    }
    public async Task Disconnect()
    {
        if (_client.Connected)
        {
            _cts.Cancel();
            _client.Close();
        }
    }

    public async Task Send(byte[] data)
    {
        if (_client.Connected && _stream != null)
        {
            await _stream.WriteAsync(data, 0, data.Length);
        }
    }
    public bool IsConnected()
    {
        return _client.Connected;
    }
    public async Task SendTelnetCommand(TelnetCommand command)
    {

        await Send(command.ToByteArray());
    }
}