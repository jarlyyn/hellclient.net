using Hellclient.Core.Types;

namespace Hellclient.Core.Infras.Components;

public class Room
{
    public string Id { get; set; } = "";
    public List<IConnection> Conns { get; set; } = new();
    public List<IConnection> Members()
    {
        return Conns;
    }
    public bool Join(IConnection conn)
    {
        if (Conns.Contains(conn))
        {
            return false;
        }
        Conns.Add(conn);
        return true;
    }
    public bool Leave(IConnection conn)
    {
        var newid=conn.ID();
        for (int i = 0; i < Conns.Count; i++)
        {
            if (Conns[i].ID() == newid)
            {
                Conns.RemoveAt(i);
                return true;
            }
        }
        return false;
    }
    public void Broadcast(byte[] data)
    {
        foreach (var conn in Conns)
        {
            conn.Send(data);
        }
    }
}

public class Rooms
{
    public Dictionary<string, Room> RoomsMap { get; set; } = [];
    public void Join(string roomId, IConnection conn)
    {
        if (!RoomsMap.ContainsKey(roomId))
        {
            RoomsMap[roomId] = new Room { Id = roomId };
        }
        RoomsMap[roomId].Join(conn);
    }
    public void Leave(string roomId, IConnection conn)
    {
        if (RoomsMap.ContainsKey(roomId))
        {
            RoomsMap[roomId].Leave(conn);
            if (RoomsMap[roomId].Conns.Count == 0)
            {
                RoomsMap.Remove(roomId);
            }
        }
    }
    public void Broadcast(string roomId, byte[] data)
    {
        if (RoomsMap.ContainsKey(roomId))
        {
            RoomsMap[roomId].Broadcast(data);
        }
    }
}