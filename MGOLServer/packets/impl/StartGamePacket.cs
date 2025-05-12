using MGOLServer.session;

namespace MGOLServer.packets.impl;

public class StartGamePacket : BasePacket
{
    public StartGamePacket() : base(PacketType.StartGame)
    {
        
    }
    public override byte[] Handle(Client client, byte[] payload)
    {
        var session = SessionManager.Instance.GetSession(client);
        if (session == null) return new byte[] { 0x0 };
        Console.WriteLine("Send Client");
        session.Send(client, this);
        return new byte[]{0x0};
    }
}