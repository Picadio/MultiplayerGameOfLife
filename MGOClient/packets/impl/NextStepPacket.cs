namespace MGOClient.packets.impl;

public class NextStepPacket : BasePacket
{
    public NextStepPacket() : base(PacketType.NextStep)
    {
        
    }
    public override void Handle(byte[] payload)
    {
        Program.MainForm.NextStep();
    }
}