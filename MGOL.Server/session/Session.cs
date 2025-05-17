using System.Net.Sockets;
using MGOL.Server.packets;
using MGOL.Server.packets.impl;

namespace MGOL.Server.session;

public class Session
{
    public Guid Id { get; }
    public Client FirstClient { get; }
    public Client? SecondClient { get; private set; } = null;

    public Session(Client firstClient)
    {
        Id = Guid.NewGuid();
        Console.WriteLine("[Session] Session Created id: " + Id);
        Console.WriteLine("[Session] Client 1 connected");
        FirstClient = firstClient;
    }

    public void JoinClient(Client client)
    {
        Console.WriteLine("[Session] Client 2 connected");
        SecondClient = client;
    }
    
    public void Disconnect(Client client)
    {
        if (FirstClient.TcpClient.Equals(client.TcpClient))
        {
            if(SecondClient != null)
                SecondClient.Send(new DisconnectSessionPacket().GetBytes());
            SessionManager.Instance.RemoveSession(this);
        }
        else
        {
            SecondClient = null;
            FirstClient.Send(new DisconnectSessionPacket().GetBytes());
        }
    }

    public void Send(Client from, IPacket packet)
    {
        if (FirstClient.TcpClient.Equals(from.TcpClient))
        {
            SecondClient.Send(packet.GetBytes());
        }
        else
        {
            FirstClient.Send(packet.GetBytes());
        }
    }
}