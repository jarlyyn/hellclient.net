using System.Net;
using System.Net.Sockets;
using System.Text;
using Hellclient.World.Types;

namespace Hellclient.World.Infras.Components;

class TelnetClient
{
    public string ProxyUser { get; set; } = "";
    public string ProxyPassword { get; set; } = "";
    public int ProxyPort { get; set; } = 0;
    public string ProxyHost { get; set; } = "";
    public bool Connected { get => _client.Connected; }

    private TcpClient _client { get; set; } = new TcpClient()
    {
        SendTimeout = 3000
    };

    public void Connect(string hostname, int port)
    {
        if (ProxyHost == "" || ProxyPort == 0)
        {
            _client.Connect(hostname, port);
            return;
        }
        _client.Connect(ProxyHost, ProxyPort);
        ExecuteSocks5Handshake(hostname, port);
    }
    private void ExecuteSocks5Handshake(string RemoteHost, int RemotePort)
    {
        Socket socket = _client.Client;
        bool hasAuth = !string.IsNullOrEmpty(ProxyUser);
        byte[] handshake = hasAuth
            ? new byte[] { 0x05, 0x02, 0x00, 0x02 }
            : new byte[] { 0x05, 0x01, 0x00 };
        socket.Send(handshake);
        byte[] response = new byte[2];
        int received = socket.Receive(response);
        if (received < 2 || response[0] != 0x05)
        {
            throw new IOException("SOCKS5 代理服务器握手失败。");
        }
        if (response[1] == 0x02)
        {
            if (!hasAuth) throw new IOException("代理服务器需要账号密码，但未提供。");

            byte[] userBytes = Encoding.UTF8.GetBytes(ProxyUser);
            byte[] passBytes = Encoding.UTF8.GetBytes(ProxyPassword ?? "");
            byte[] authRequest = new byte[3 + userBytes.Length + passBytes.Length];

            authRequest[0] = 0x01;
            authRequest[1] = (byte)userBytes.Length;
            Buffer.BlockCopy(userBytes, 0, authRequest, 2, userBytes.Length);
            int passOffset = 2 + userBytes.Length;
            authRequest[passOffset] = (byte)passBytes.Length;
            Buffer.BlockCopy(passBytes, 0, authRequest, passOffset + 1, passBytes.Length);

            socket.Send(authRequest);

            byte[] authResponse = new byte[2];
            socket.Receive(authResponse);
            if (authResponse[1] != 0x00)
            {
                throw new IOException("SOCKS5 代理身份验证失败。");
            }
        }
        else if (response[1] == 0xFF)
        {
            throw new IOException("代理服务器拒绝了客户端的验证方法。");
        }

        // 3. 发送 CONNECT 请求告知目标地址
        byte[] requestHeader = new byte[] { 0x05, 0x01, 0x00 };
        byte[] addressBytes;
        byte addressType;

        if (IPAddress.TryParse(RemoteHost!, out IPAddress? ipAddress))
        {
            if (ipAddress == null) throw new IOException($"无法解析目标主机的 IP 地址: {RemoteHost}");
            addressBytes = ipAddress.GetAddressBytes();
            addressType = (byte)(ipAddress.AddressFamily == AddressFamily.InterNetwork ? 0x01 : 0x04);
        }
        else
        {
            addressType = 0x03;
            byte[] domainBytes = Encoding.ASCII.GetBytes(RemoteHost);
            addressBytes = new byte[domainBytes.Length + 1];
            addressBytes[0] = (byte)domainBytes.Length;
            Buffer.BlockCopy(domainBytes, 0, addressBytes, 1, domainBytes.Length);
        }

        byte[] portBytes = BitConverter.GetBytes((ushort)RemotePort);
        if (BitConverter.IsLittleEndian) Array.Reverse(portBytes);

        byte[] connectRequest = new byte[requestHeader.Length + 1 + addressBytes.Length + 2];
        int offset = 0;
        Buffer.BlockCopy(requestHeader, 0, connectRequest, offset, requestHeader.Length); offset += requestHeader.Length;
        connectRequest[offset++] = addressType;
        Buffer.BlockCopy(addressBytes, 0, connectRequest, offset, addressBytes.Length); offset += addressBytes.Length;
        Buffer.BlockCopy(portBytes, 0, connectRequest, offset, 2);

        socket.Send(connectRequest);

        // 4. 接收连接响应
        byte[] connectResponse = new byte[256];
        int resLen = socket.Receive(connectResponse);

        if (resLen < 4 || connectResponse[0] != 0x05 || connectResponse[1] != 0x00)
        {
            throw new IOException($"无法通过代理连接到目标服务器。错误码: {connectResponse[1]}");
        }

    }
    public NetworkStream GetStream()
    {
        return _client.GetStream();
    }
    public void Close()
    {
        _client.Close();
    }
    public void Dispose()
    {
        _client.Dispose();
    }
}
public class Telnet : IMudConnection
{
    public const int StatusNormal = 0;
    public const int StatusIAC = 1;
    public const int StatusCmd = 2;
    public const int StatusSb = 3;
    public const int StatusSbIac = 4;
    private TelnetClient _client { get; set; } = new TelnetClient();
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
                catch (IOException)
                {
                    Disconnected();
                    return;
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
    public void Connect(string host, int port, string proxytype, string proxyhost, int proxyport, string proxyusername, string proxypassword)
    {
        Host = host;
        Port = port;
        Disconnect();
        _client = new TelnetClient();
        if (proxytype != "")
        {
            var pt = proxytype.ToLower();
            if (pt != "socks5" && pt != "socks")
            {
                throw new NotSupportedException("Only SOCKS5 proxy is supported.");
            }
            _client.ProxyHost = proxyhost;
            _client.ProxyPort = proxyport;
            _client.ProxyUser = proxyusername;
            _client.ProxyPassword = proxypassword;
        }
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