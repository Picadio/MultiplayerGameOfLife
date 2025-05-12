using MGOLServer.session;

namespace MGOLServer.packets.impl;

public class ResetPacket : BasePacket
{
    public ResetPacket() : base(PacketType.ResetPacket)
    {
        
    }
    public override byte[] Handle(Client client, byte[] payload)
    {
        var session = SessionManager.Instance.GetSession(client);
        if (session == null) return new byte[] { 0x0 };
        Console.WriteLine("Send Client");
        session.Send(client, new ResetPacket());
        return new byte[]{0x0};
    }
}