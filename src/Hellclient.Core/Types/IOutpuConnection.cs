namespace Hellclient.Core.Types;

public interface IConnection
{
    public void Close();
    public void Send(byte[] data);
    public string ID();
    public EventHandler<byte[]>? OnMessage { get; set; }
    public EventHandler? OnClose { get; set; }

}