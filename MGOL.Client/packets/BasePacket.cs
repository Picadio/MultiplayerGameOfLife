namespace MGOL.Client.packets;

public abstract class BasePacket(byte packetType) : IPacket
{
    protected byte[]? _payload = null;
    public byte Type { get; } = packetType;
    
    
    public byte[] GetBytes()
    {
        if (_payload == null)
        {
            return new[] { Type }.Concat(BitConverter.GetBytes(0)).ToArray();
        }
        return new[]{Type}.Concat(BitConverter.GetBytes(_payload.Length)).Concat(_payload).ToArray();
    }

    public abstract void Handle(byte[] payload);
}