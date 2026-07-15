namespace Hellclient.Mapper.Models;

public class Mapper
{
    public List<Room> Rooms { get; set; } = [];
    public Dictionary<string, bool> Tags { get; set; } = [];
    public List<Path> Fly { get; set; } = [];

}