using Hellclient.World.Types;

namespace Hellclient.World.Cores;

public partial class World
{
    public void DoSendToQueue(Command command) => Service.QueueService.Append(Context, command);
    public int DoDiscardQueue(bool force) => Service.QueueService.Flush(Context, force);
    public List<Command> GetQueue() => Service.QueueService.ListQueue(Context);
    public void DoLockQueue() => Service.QueueService.LockQueue(Context);


}