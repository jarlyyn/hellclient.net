namespace Hellclient.Mapper.Models;

public class Room
{
    public string ID { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public List<Path> Exits { get; set; } =[];

}