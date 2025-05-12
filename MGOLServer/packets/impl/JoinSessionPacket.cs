using MGOLServer.session;

namespace MGOLServer.packets.impl;

public class JoinSessionPacket : BasePacket
{
    private Guid _guid;
    public JoinSessionPacket() : base(PacketType.JoinSession)
    {
        
    }
    
    public JoinSessionPacket(Guid guid) : base(PacketType.JoinSession)
    {
        _guid = guid;
    }

    public override void Read(byte[] payload)
    {
        _guid = new Guid(payload);
    }
    
    protected override byte[]? GetPayload()
    {
        return _guid.ToByteArray();
    }

    public override byte[] Handle(Client client)
    {
        Console.WriteLine("Receive session id: " + _guid);
        var session = SessionManager.Instance.GetSession(_guid);
        if (session == null)
        {
            return new byte[] { 0x0 };
        }
        if(session.SecondClient != null) return new byte[] { 0x0 };
        session.JoinClient(client);
        session.Send(client, new CreateSessionPacket(true));
        return new CreateSessionPacket(false).GetBytes();
    }
}