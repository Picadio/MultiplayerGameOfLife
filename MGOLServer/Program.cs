using MGOLServer.packets;
using MGOLServer.packets.impl;

namespace MGOLServer;

public class Program
{
    public static void Main(string[] args)
    {
        PacketsManager.Instance.RegisterPacket(new JoinSessionPacket());
        PacketsManager.Instance.RegisterPacket(new CreateSessionPacket());
        PacketsManager.Instance.RegisterPacket(new SetCellPacket());
        PacketsManager.Instance.RegisterPacket(new StartGamePacket());
        PacketsManager.Instance.RegisterPacket(new NextStepPacket());
        PacketsManager.Instance.RegisterPacket(new ResetPacket());
        PacketsManager.Instance.RegisterPacket(new DisconnectSessionPacket());
        PacketsManager.Instance.RegisterPacket(new ChangeSpeedPacket());

        var tcpServer = new TcpServer(8080);
        tcpServer.Start();
    }
}