using System.Net.Sockets;

namespace MGOL.Client.packets.impl;

public class DisconnectSessionPacket : BasePacket
{
    public DisconnectSessionPacket() : base(PacketType.DisconnectSession)
    {
        
    }
    public override void Handle(byte[] payload)
    {
        Program.Client.SessionConnected = false;
        Program.MainForm.SetConnected(false);
        Program.MainForm.SwitchTimer(false);
    }
}