using MGOLServer.session;

namespace MGOLServer.packets.impl;

public class JoinSessionPacket : BasePacket
{
    public JoinSessionPacket() : base(PacketType.JoinSession)
    {
        
    }
    
    public JoinSessionPacket(byte[] payload) : base(PacketType.JoinSession)
    {
        Payload = payload;
    }
    public override byte[] Handle(Client client, byte[] payload)
    {
        var id = new Guid(payload);
        Console.WriteLine("Receive session id: " + id);
        var session = SessionManager.Instance.GetSession(id);
        if (session == null)
        {
            return new byte[] { 0x0 };
        }
        
        if(session.SecondClient != null) return new byte[] { 0x0 };
        session.JoinClient(client);
        session.Send(client, new CreateSessionPacket());
        return new CreateSessionPacket().GetBytes();
    }
}