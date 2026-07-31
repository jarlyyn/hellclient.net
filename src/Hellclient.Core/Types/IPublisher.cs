namespace Hellclient.Core.Types;

public interface IPublisher
{
    void Publish(Message msg);
}