using MGOLServer.session;

namespace MGOLServer.packets.impl;

public class CreateSessionPacket : BasePacket
{
    public CreateSessionPacket() : base(PacketType.CreateSession)
    {
        
    }

    public override byte[] Handle(Client client, byte[] payload)
    {
        var session = SessionManager.Instance.CreateSession(client);
        
        return new JoinSessionPacket(session.Id.ToByteArray()).GetBytes();;
    }
}