using System.Net.Sockets;
using System.Text;
using Hellclient.World.Types;
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
    private CancellationTokenSource _cts;
    public string Host { get; set; } = "";
    public int Port { get; set; } = 0;
    private List<byte> _buffer = [];
    private int status = StatusNormal;
    private byte currentcmd = 0;
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
                    Publish(data);
                    status = StatusNormal;

                }
                else
                {
                    if (data == TelnetCommand.CmdSubnegotiation)
                    {
                        status = StatusSb;
                    }
                    else
                    {
                        currentcmd = data;
                        status = StatusCmd;
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
                    OnCommandReceived?.Invoke(this, new TelnetCommand(TelnetCommand.CmdEndSubnegotiation, [.. _buffer]));
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
    public async Task Connect(string host, int port)
    {
        Host = host;
        Port = port;
        await _client.ConnectAsync(host, port);
        _ = Connected();
        using (NetworkStream stream = _client.GetStream())
        {
            _stream = stream;
            byte[] buffer = new byte[1];
            try
            {
                while (_client.Connected)
                {
                    int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                    if (bytesRead == 0)
                    {
                        Console.WriteLine("【连接中止】服务器已主动断开连接。");
                        break;
                    }

                    OnByte(buffer[0]);
                }
            }
            catch (SocketException ex)
            {
                // 物理断网、超时或服务器崩溃会触发此异常
                Console.WriteLine($"【连接中止】网络套接字异常: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"【连接中止】读取流时发生未知错误: {ex.Message}");
            }
            finally
            {
                _ = Disconnected();
                Console.WriteLine("已退出监听循环，正在清理当前连接资源...");
            }

        }
    }
    public async Task Disconnect()
    {
        if (_client.Connected)
        {
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
    public async Task SendCommand(TelnetCommand command)
    {

        await Send(command.ToByteArray());
    }
}