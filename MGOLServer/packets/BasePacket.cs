using System.Net.Sockets;

namespace MGOLServer.packets;

public abstract class BasePacket(byte packetType) : IPacket
{
    public byte Type { get; } = packetType;

    public byte[] GetBytes()
    {
        var payload = GetPayload();
        if (payload == null)
        {
            return new[] { Type }.Concat(BitConverter.GetBytes(0)).ToArray();
        }
        return new[]{Type}.Concat(BitConverter.GetBytes(payload.Length)).Concat(payload).ToArray();
    }

    public abstract void Read(byte[] payload);

    public abstract byte[] Handle(Client client);

    protected abstract byte[]? GetPayload();
}