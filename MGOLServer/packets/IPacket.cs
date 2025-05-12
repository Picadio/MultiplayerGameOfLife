using System.Net.Sockets;

namespace MGOLServer.packets;

public interface IPacket
{
    public byte Type { get; }
    byte[] Handle(Client from);
    void Read(byte[] payload);
    byte[] GetBytes();
}