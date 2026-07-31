using Hellclient.Core.Types;

namespace Hellclient.Core.Infras.Components;

public class Adapter
{
    public Dictionary<string, Action<Message>> Handlers { get; set; } = new();
    public void RegisterHandler(string msgType, Action<Message> handler)
    {
        Handlers[msgType] = handler;
    }
    public bool Exec(Message msg)
    {
        var handler= Handlers.TryGetValue(msg.Type, out var h) ? h : null;
        if (handler == null)
        {
            return false;
        }
        handler(msg);
        return true;
    }
}
