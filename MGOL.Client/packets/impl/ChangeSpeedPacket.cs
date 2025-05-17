namespace MGOL.Client.packets.impl;

public class ChangeSpeedPacket : BasePacket
{
    public ChangeSpeedPacket() : base(PacketType.ChangeSpeed)
    {
    }
    public ChangeSpeedPacket(int speed) : base(PacketType.ChangeSpeed)
    {
        _payload = BitConverter.GetBytes(speed);
    }
    public override void Handle(byte[] payload)
    {
        Program.MainForm.ChangeSpeed(BitConverter.ToInt32(payload));
    }
}