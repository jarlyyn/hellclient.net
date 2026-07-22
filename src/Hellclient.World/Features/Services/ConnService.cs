using Hellclient.World.States;

namespace Hellclient.World.Features.Services;

public interface IConnService
{
    public void InstallTo(WorldContext context);
    public void Connect(WorldContext context);
    public void Stop(WorldContext context);
    public void Close(WorldContext context);
    public void Send(WorldContext context, byte[] message);
    public bool IsConnected(WorldContext context);
    public byte[] GetBuffer(WorldContext context);
}

// 连接服务，用于处理Mud当前连接的状态和数据传输，Prompt维护
public class ConnService : IConnService
{
    public void InstallTo(WorldContext context)
    {
        // Implementation for installing the connection service to the world context
    }
    public void Connect(WorldContext context)
    {
        context.Convert.Charset = context.Data.Charset;
        _ = context.Connection.Connect(context.Data.Host, int.TryParse(context.Data.Port, out int port) ? port : 0);
    }
    public void Stop(WorldContext context)
    {
        // Implementation for stopping the connection to the world using the provided context
    }
    public void Close(WorldContext context)
    {
        _ = context.Connection.Disconnect();
    }
    public void Send(WorldContext context, byte[] message)
    {
        context.Connection.Send(message);
    }
    public bool IsConnected(WorldContext context)
    {
        return context.Connection.IsConnected();
    }
    public byte[] GetBuffer(WorldContext context)
    {
        // Implementation for retrieving the buffer from the world using the provided context
        return new byte[0]; // Replace with actual implementation
    }
}