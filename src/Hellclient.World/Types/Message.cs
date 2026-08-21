using Hellclient.World.Infras.Adapters;

namespace Hellclient.World.Types;

public class Message
{
    public const int MessageCommandResponse = 0;
    public const int MessageCommandBatchCommand = 1;
    public string World { get; set; } = string.Empty;
    public string ID { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string Data { get; set; } = string.Empty;
    public int Command { get; set; } = 0;
    public string Desc()
    {
        return $"{Type} {ID} {Data}";
    }
    public static Message Create(string world, string msgtype, string data)
    {
        return new Message
        {
            ID = SimpleID.Instance.GenerateID(),
            World = world,
            Type = msgtype,
            Data = data
        };
    }
}