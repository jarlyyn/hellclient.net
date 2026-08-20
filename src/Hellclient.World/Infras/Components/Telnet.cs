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
    private CancellationTokenSource _cts = new CancellationTokenSource();
    public string Host { get; set; } = "";
    public int Port { get; set; } = 0;
    private List<byte> _buffer = new();
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
                    OnCommandReceived?.Invoke(this, new TelnetCommand(TelnetCommand.CmdSubnegotiation, _buffer.ToArray()));
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
    private void Connected()
    {
        OnConnected?.Invoke(this, EventArgs.Empty);
    }
    private void Disconnected()
    {
        OnDisconnected?.Invoke(this, EventArgs.Empty);
    }
    private void listen()
    {
        _cts = new CancellationTokenSource();
        using (NetworkStream stream = _client.GetStream())
        {
            byte[] buffer = new byte[1];
            while (_client.Connected)
            {
                int bytesRead = 0;
                try
                {
                    bytesRead = stream.Read(buffer, 0, buffer.Length);
                }
                catch (Exception ex)
                {
                    Logger(ex);
                    Disconnected();
                    return;
                }

                if (bytesRead == 0)
                {
                    break;
                }

                OnByte(buffer[0]);

            }
            Disconnected();
        }

    }
    public void Connect(string host, int port)
    {
        Host = host;
        Port = port;
        Disconnect();
        _client = new TcpClient
        {
            SendTimeout = 3000
        };
        _client.Connect(host, port);
        Connected();
        Task.Run(listen);
    }
    public void Disconnect()
    {
        if (_client.Connected)
        {
            _cts.Cancel();
            _client.Close();
            _client.Dispose();
        }
    }

    public void Send(byte[] data)
    {
        if (_client.Connected)
        {
            _client.GetStream().Write(data, 0, data.Length);
        }
    }
    public bool IsConnected()
    {
        return _client.Connected;
    }
    public void SendTelnetCommand(TelnetCommand command)
    {
        Send(command.ToByteArray());
    }
}