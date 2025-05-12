using MGOLServer.session;

namespace MGOLServer.packets.impl;

public class SyncPacket : BasePacket
{
    private byte[] _payload;
    public SyncPacket() : base(PacketType.Sync)
    {
        
    }
    public SyncPacket(byte[] payload) : base(PacketType.Sync)
    {
        _payload = payload;
    }

    public override void Read(byte[] payload)
    {
        _payload = payload;
    }

    public override byte[] Handle(Client client)
    {
        var session = SessionManager.Instance.GetSession(client);
        if (session == null) return new byte[] { 0x0 };
        Console.WriteLine("Send Client");
        session.Send(client, this);
        return new byte[]{0x0};
    }

    protected override byte[]? GetPayload()
    {
        return _payload;
    }
}