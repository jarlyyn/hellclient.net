namespace Hellclient.World.Cores;

public partial class World
{
    private void init()
    {
        Context.Connection.OnDataReceived += (sender, data) =>
        {
            Context.Convert.OnByte(this, data);
        };
        Context.Connection.OnCommandReceived += (sender, cmd) =>
        {
            Context.Convert.OnCommandReceived(this, cmd);
        };
    }
    private void install()
    {
   
    }
    private void uninstall()
    {
        
    }
}