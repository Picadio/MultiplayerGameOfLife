namespace MGOClient.packets;

public class PacketType
{
    public static byte JoinSession = 0x01;
    public static byte CreateSession = 0x02;
    public static byte SetCell = 0x03;
    public static byte StartGame = 0x04;
    public static byte DisconnectSession = 0x05;
    public static byte NextStep = 0x06;
    public static byte ResetPacket = 0x07;
    public static byte ChangeSpeed = 0x08;
}