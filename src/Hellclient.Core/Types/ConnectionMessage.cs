namespace Hellclient.Core.Types;

public class ConnectionMessage{
    public required byte[] Message{get;set;}
    public required IConnection Connection{get;set;}
}