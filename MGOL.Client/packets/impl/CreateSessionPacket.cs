using System.Net.Sockets;


namespace MGOL.Client.packets.impl;

public class CreateSessionPacket : BasePacket
{
    public CreateSessionPacket() : base(packets.PacketType.CreateSession)
    {
        
    }
    public override void Handle(byte[] payload)
    {
        var host = BitConverter.ToBoolean(payload);
        if (host)
        {
            var packet = new SyncPacket(Program.Algorithm.Step, Program.Algorithm.CurrentGrid);
            Program.Client.SendAsync(packet);
        }
        Program.Client.SessionConnected = true;
        Program.MainForm.SetConnected(true);
        Program.MainForm.IsConnectedLabel.Text = Program.MainForm.IsConnectedLabel.Text.Replace("false", "true");
    }
}