using System.Net.Sockets;

namespace MGOL.Client.packets.impl;

public class StartGamePacket : BasePacket
{
    public StartGamePacket() : base(PacketType.StartGame)
    {
        
    }
    public StartGamePacket(bool enable) : base(PacketType.StartGame)
    {
        _payload = BitConverter.GetBytes(enable);
    }
    public override void Handle(byte[] payload)
    {
        var enable = BitConverter.ToBoolean(payload);
        Program.MainForm.SwitchTimer(enable);
    }
}