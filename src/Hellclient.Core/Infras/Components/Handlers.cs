using Hellclient.Core.Types;

namespace Hellclient.Core.Infras.Components;

public class Handlers
{
    private Dictionary<string, Action<IConnection,SeparatedCommand>> handlers { get; set; } = new();
    public void RegisterHandler(string commandType, Action<IConnection,SeparatedCommand> handler)
    {
        handlers[commandType] = handler;
    }
    public bool Exec(ConnectionMessage msg)
    {
        var cmd=new SeparatedCommand();
        cmd.Decode(msg.Message);
        
        var handler = handlers.TryGetValue(cmd.CommandType, out var h) ? h : null;
        if (handler == null)
        {
            return false;
        }
        handler(msg.Connection, cmd);
        return true;
    }
}