using MGOL.Server.session;

namespace MGOL.Server.packets.impl;

public class CreateSessionPacket : BasePacket
{
    private bool _isHost;
    public CreateSessionPacket() : base(PacketType.CreateSession)
    {
        
    }
    public CreateSessionPacket(bool isHost) : base(PacketType.CreateSession)
    {
        _isHost = isHost;
    }

    public override void Read(byte[] payload)
    {
        _isHost = BitConverter.ToBoolean(payload);
    }
    
    protected override byte[]? GetPayload()
    {
        return BitConverter.GetBytes(_isHost);
    }

    public override byte[] Handle(Client client)
    {
        var session = SessionManager.Instance.CreateSession(client);
        
        return new JoinSessionPacket(session.Id).GetBytes();;
    }
}