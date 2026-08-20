
namespace Hellclient.World.Types;

public interface IMudConnection
{
    public string Host { get; set; }
    public int Port { get; set; }
    public void Connect(string host, int port);
    public void Disconnect();
    public void Send(byte[] data);
    public EventHandler<byte>? OnDataReceived { get; set; }
    public EventHandler<TelnetCommand>? OnCommandReceived { get; set; }
    public EventHandler? OnDisconnected { get; set; }
    public EventHandler? OnConnected { get; set; }
    public bool IsConnected();
    public void SendTelnetCommand(TelnetCommand command);
}