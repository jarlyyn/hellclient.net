namespace Hellclient.World.Types;

public class Timers
{

}
public class Triggers
{

}
public class Aliases
{

}
public class Automation
{
    public Timers Timers { get; set; } = new Timers();
    public Triggers Triggers { get; set; } = new Triggers();
    public Aliases Aliases { get; set; } = new Aliases();
    public List<string> MultiLines { get; set; } = [];
}