using System.Net.Sockets;


namespace MGOClient.packets.impl;

public class CreateSessionPacket : BasePacket
{
    public CreateSessionPacket() : base(packets.PacketType.CreateSession)
    {
        
    }

    public override void Handle(byte[] payload)
    {
        Program.Client.SessionConnected = true;
        Program.MainForm.SetConnected(true);
        Program.MainForm.IsConnectedLabel.Text = Program.MainForm.IsConnectedLabel.Text.Replace("false", "true");
    }
}