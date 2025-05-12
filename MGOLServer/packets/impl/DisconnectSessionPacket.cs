namespace MGOLServer.packets.impl;

public class DisconnectSessionPacket : BasePacket
{
    public DisconnectSessionPacket() : base(PacketType.DisconnectSession)
    {
        
    }

    public override void Read(byte[] payload)
    {
    }

    public override byte[] Handle(Client client)
    {
        throw new NotImplementedException();
    }

    protected override byte[]? GetPayload()
    {
        return null;
    }
}