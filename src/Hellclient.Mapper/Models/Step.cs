namespace Hellclient.Mapper.Models;

public class Step
{
    public static Step Empty { get; } = new Step();
    public string To { get; set; } = string.Empty;
    public string From { get; set; } = string.Empty;
    public string Command { get; set; } = string.Empty;
    public int Delay { get; set; }
    public int Remain { get; set; }

}