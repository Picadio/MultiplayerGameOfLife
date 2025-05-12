using System.Net.Sockets;

namespace MGOLServer.packets;

public abstract class BasePacket(byte packetType) : IPacket
{
    public byte Type { get; } = packetType;
    public byte[]? Payload { get; protected set; } = null;
    
    public abstract byte[] Handle(Client client, byte[] payload);

    public byte[] GetBytes()
    {
        if (Payload == null)
        {
            return new[] { Type }.Concat(BitConverter.GetBytes(0)).ToArray();
        }
        return new[]{Type}.Concat(BitConverter.GetBytes(Payload.Length)).Concat(Payload).ToArray();
    }
}