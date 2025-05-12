using System.Net.Sockets;

namespace MGOLServer.packets;

public interface IPacket
{
    public byte Type { get; }
    byte[] Handle(Client from, byte[] payload);
    byte[] GetBytes();
}