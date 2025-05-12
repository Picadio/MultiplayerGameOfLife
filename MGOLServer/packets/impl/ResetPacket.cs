using MGOLServer.session;

namespace MGOLServer.packets.impl;

public class ResetPacket : BasePacket
{
    public ResetPacket() : base(PacketType.ResetPacket)
    {
        
    }

    public override void Read(byte[] payload)
    {
    }

    protected override byte[]? GetPayload()
    {
        return null;
    }
    
    public override byte[] Handle(Client client)
    {
        var session = SessionManager.Instance.GetSession(client);
        if (session == null) return new byte[] { 0x0 };
        Console.WriteLine("Send Client");
        session.Send(client, this);
        return new byte[]{0x0};
    }
}