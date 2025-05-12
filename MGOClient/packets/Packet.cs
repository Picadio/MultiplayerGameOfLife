using System.Net.Sockets;

namespace MGOClient.packets;

public interface IPacket
{
    public byte Type { get; }
    byte[] GetBytes();
    public void Handle(byte[] payload);
}