namespace Hellclient.Core.Infras.Components;

public class HellSwitch
{
    public const int StatusDisabled = 0;
    public const int StatusDisconnected = 1;
    public const int StatusConnected = 2;
    public int Status()
    {
        return StatusDisabled;
    }
    public void Ping()
    {
    }
    public void Broadcast(byte[] msg)
    {

    }
    public void Start()
    {
        
    }
    public void Stop()
    {
        
    }
    public void Close()
    {
        
    }
}