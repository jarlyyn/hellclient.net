namespace Hellclient.Core.Types;

public interface IConnection
{
    public Task Close();
    public Task Send(byte[] data);
    public string ID();
    public EventHandler<byte[]>? OnMessage { get; set; }
    public EventHandler? OnClose { get; set; }

}