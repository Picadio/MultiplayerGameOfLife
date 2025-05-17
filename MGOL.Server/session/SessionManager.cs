using System.Net.Sockets;

namespace MGOL.Server.session;

public class SessionManager
{
    public static SessionManager Instance = new();
    private readonly Dictionary<Guid, Session> _sessions = new();

    public Session CreateSession(Client client)
    {
        var session = new Session(client);
        _sessions.Add(session.Id, session);
        return session;
    }
    
    public void RemoveSession(Session session)
    {
        _sessions.Remove(session.Id);
    }
    
    public Session? GetSession(Guid id)
    {
        if (_sessions.TryGetValue(id, out var session)) return session;
        return null;
    }

    public Session? GetSession(Client client)
    {
        return _sessions.Values
            .FirstOrDefault(s => s.FirstClient.TcpClient.Equals(client.TcpClient) || s.SecondClient.TcpClient.Equals(client.TcpClient));
    }
}