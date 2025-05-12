
namespace MGOClient.packets;

public class PacketsManager
{
    public static PacketsManager Instance = new PacketsManager();

    private readonly Dictionary<byte, IPacket> _packets = new();

    public void RegisterPacket(IPacket packet)
    {
        _packets.Add(packet.Type, packet);
    }

    public IPacket GetPacket(byte packetType)
    {
        return _packets[packetType];
    }
}