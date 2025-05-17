using System.Net.Sockets;
using System.Text;

namespace MGOL.Client.packets.impl;

public class JoinSessionPacket : BasePacket
{
    public JoinSessionPacket(Guid id) : base(packets.PacketType.JoinSession)
    {
        _payload = id.ToByteArray();
    }
    public JoinSessionPacket() : base(packets.PacketType.JoinSession)
    {
    }

    public override void Handle(byte[] payload)
    {
        Program.MainForm.IpTextBox.Text = new Guid(payload).ToString();
    }
}