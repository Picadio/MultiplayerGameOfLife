namespace MGOLServer.packets.impl;

public class DisconnectSessionPacket : BasePacket
{
    public DisconnectSessionPacket() : base(PacketType.DisconnectSession)
    {
        
    }
    
    public override byte[] Handle(Client client, byte[] payload)
    {
        throw new NotImplementedException();
    }
}