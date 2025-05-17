using MGOL.Server.session;

namespace MGOL.Server.packets.impl;

public class SetCellPacket : BasePacket
{
    private int _x;
    private int _y;
    public SetCellPacket() : base(PacketType.SetCell)
    {
        
    }
    public SetCellPacket(int x, int y) : base(PacketType.SetCell)
    {
        _x = x;
        _y = y;
    }

    public override void Read(byte[] payload)
    {
        _x = BitConverter.ToInt32(payload);
        _y = BitConverter.ToInt32(payload, 4);
    }

    protected override byte[]? GetPayload()
    {
        return BitConverter.GetBytes(_x).Concat(BitConverter.GetBytes(_y)).ToArray();
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