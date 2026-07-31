using Hellclient.Core.Types;

namespace Hellclient.Core.Infras.Components;

public class Users
{
    public Dictionary<string, IConnection> Identities { get; set; } = [];

    private IConnection? _conn(string id)
    {
        return Identities.TryGetValue(id, out var conn) ? conn : null;
    }
    private void onLogout(string id, IConnection conn)
    {
        conn.Close();
    }
    public void Login(string id, IConnection conn)
    {
        if (Identities.ContainsKey(id))
        {
            var oldConn = Identities[id];
            onLogout(id, oldConn);
        }
        Identities[id] = conn;
    }
    public void Logout(string id, IConnection conn)
    {
        var user = Identities.TryGetValue(id, out var c) ? c : null;
        if (user != null)
        {
            onLogout(id, user);
            Identities.Remove(id);
        }
    }
    public void SendByID(string id, byte[] data)
    {
        _conn(id)?.Send(data);
    }
}