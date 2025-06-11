namespace MGOL.Client.packets;

public interface IPacket
{
    public byte Type { get; }
    byte[] GetBytes();
    public void Handle(byte[] payload);
}