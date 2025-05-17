using System.Net;
using System.Runtime.InteropServices;
using MGOL.Client.algorithm;
using MGOL.Client.packets.impl;
using MGOL.Client.packets;

namespace MGOL.Client;

static class Program
{
    public static IAlgorithm<int> Algorithm;
    public static MainForm MainForm;
    public static Client Client { get; private set; }
    
    [STAThread]
    static async Task Main()
    {
        PacketsManager.Instance.RegisterPacket(new CreateSessionPacket());
        PacketsManager.Instance.RegisterPacket(new StartGamePacket());
        PacketsManager.Instance.RegisterPacket(new SetCellPacket());
        PacketsManager.Instance.RegisterPacket(new JoinSessionPacket());
        PacketsManager.Instance.RegisterPacket(new NextStepPacket());
        PacketsManager.Instance.RegisterPacket(new ResetPacket());
        PacketsManager.Instance.RegisterPacket(new DisconnectSessionPacket());
        PacketsManager.Instance.RegisterPacket(new ChangeSpeedPacket());
        PacketsManager.Instance.RegisterPacket(new SyncPacket());
        
        ApplicationConfiguration.Initialize();
        AllocConsole();
        
        Client = new Client(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8080));
        Client.StartAsync();
        
        Algorithm = new MultiplayerAlgorithm(64, 64);
        MainForm = new MainForm(Algorithm);
        Application.Run(MainForm);
    }
    [DllImport("kernel32.dll")]
    static extern bool AllocConsole();
}