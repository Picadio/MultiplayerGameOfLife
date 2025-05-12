namespace MGOClient.packets.impl;

public class ResetPacket : BasePacket
{
    public ResetPacket() : base(PacketType.ResetPacket)
    {
        
    }
    public override void Handle(byte[] payload)
    {
        Program.MainForm.Reset();
    }
}