using System.Net.Sockets;

namespace MGOClient.packets.impl;

public class StartGamePacket : BasePacket
{
    public StartGamePacket() : base(packets.PacketType.StartGame)
    {
        
    }

    public override void Handle(byte[] payload)
    {
        Program.MainForm.SwitchTimer();
    }
}