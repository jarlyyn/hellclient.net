using Hellclient.World.Types;

namespace Hellclient.World.Types;

public interface IMudConnection
{
    public string Host { get; set; }
    public int Port { get; set; }
    public Task Connect(string host, int port);
    public Task Disconnect();
    public Task Send(byte[] data);
    public EventHandler<byte>? OnDataReceived { get; set; }
    public EventHandler<TelnetCommand>? OnCommandReceived { get; set; }
    public EventHandler? OnDisconnected { get; set; }
    public EventHandler? OnConnected { get; set; }
    public bool IsConnected();
}