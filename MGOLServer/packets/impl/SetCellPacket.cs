using MGOLServer.session;

namespace MGOLServer.packets.impl;

public class SetCellPacket : BasePacket
{
    public SetCellPacket() : base(PacketType.SetCell)
    {
        
    }
    public SetCellPacket(byte[] payload) : base(PacketType.SetCell)
    {
        Payload = payload;
    }
    public override byte[] Handle(Client client, byte[] payload)
    {
        var session = SessionManager.Instance.GetSession(client);
        if (session == null) return new byte[] { 0x0 };
        Console.WriteLine("Send Client");
        session.Send(client, new SetCellPacket(payload));
        return new byte[]{0x0};
    }
}