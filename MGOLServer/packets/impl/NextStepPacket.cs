using MGOLServer.session;

namespace MGOLServer.packets.impl;

public class NextStepPacket : BasePacket
{
    public NextStepPacket() : base(PacketType.NextStep)
    {
        
    }

    public override byte[] Handle(Client client, byte[] payload)
    {
        var session = SessionManager.Instance.GetSession(client);
        if (session == null) return new byte[] { 0x0 };
        Console.WriteLine("Send Client");
        session.Send(client, new NextStepPacket());
        return new byte[]{0x0};
    }
}