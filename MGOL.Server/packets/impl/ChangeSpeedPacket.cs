using MGOL.Server.session;

namespace MGOL.Server.packets.impl;

public class ChangeSpeedPacket : BasePacket
{
    private int _speed;
    public ChangeSpeedPacket() : base(PacketType.ChangeSpeed)
    {
        
    }
    public ChangeSpeedPacket(int speed) : base(PacketType.ChangeSpeed)
    {
        _speed = speed;
    }

    protected override byte[] GetPayload()
    {
        return BitConverter.GetBytes(_speed);
    }

    public override void Read(byte[] payload)
    {
        _speed = BitConverter.ToInt32(payload);
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