using Hellclient.Core.Types;

namespace Hellclient.Core.Infras.Components;

public class Adapter
{
    public Dictionary<string, Func<Message, Task>> Handlers { get; set; } = new();
    public void RegisterHandler(string msgType, Func<Message, Task> handler)
    {
        Handlers[msgType] = handler;
    }
    public async Task<bool> Exec(Message msg)
    {
        var handler= Handlers.TryGetValue(msg.Type, out var h) ? h : null;
        if (handler == null)
        {
            return false;
        }
        await handler(msg);
        return true;
    }
}
