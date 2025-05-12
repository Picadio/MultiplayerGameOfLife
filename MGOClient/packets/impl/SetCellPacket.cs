using System.Net.Sockets;
using System.Text;


namespace MGOClient.packets.impl;

public class SetCellPacket : BasePacket
{
    public SetCellPacket(int x, int y) : base(packets.PacketType.SetCell)
    {
        _payload = BitConverter.GetBytes(x).Concat(BitConverter.GetBytes(y)).ToArray();
    }
    public SetCellPacket() : base(packets.PacketType.SetCell)
    {
    }

    public override void Handle(byte[] payload)
    {
        var x = BitConverter.ToInt32(payload, 0);
        var y = BitConverter.ToInt32(payload, 4);
        
        Program.MainForm.SetCell(x, y, 2);
    }
}