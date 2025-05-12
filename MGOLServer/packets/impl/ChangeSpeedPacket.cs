using MGOLServer.session;

namespace MGOLServer.packets.impl;

public class ChangeSpeedPacket : BasePacket
{
    public ChangeSpeedPacket() : base(PacketType.ChangeSpeed)
    {
        
    }
    public ChangeSpeedPacket(byte[] payload) : base(PacketType.ChangeSpeed)
    {
        Payload = payload;
    }
    public override byte[] Handle(Client client, byte[] payload)
    {
        var session = SessionManager.Instance.GetSession(client);
        if (session == null) return new byte[] { 0x0 };
        Console.WriteLine("Send Client");
        session.Send(client, new ChangeSpeedPacket(payload));
        return new byte[]{0x0};
    }
}