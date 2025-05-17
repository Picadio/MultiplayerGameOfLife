using MGOL.Server.session;

namespace MGOL.Server.packets.impl;

public class StartGamePacket : BasePacket
{
    private bool _enable;
    public StartGamePacket() : base(PacketType.StartGame)
    {
        
    }
    public StartGamePacket(bool enable) : base(PacketType.StartGame)
    {
        _enable = enable;
    }

    public override void Read(byte[] payload)
    {
        _enable = BitConverter.ToBoolean(payload);
    }

    protected override byte[]? GetPayload()
    {
        return BitConverter.GetBytes(_enable);
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