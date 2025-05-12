using System.Net;
using System.Net.Sockets;
using MGOLServer.packets;
using MGOLServer.session;

namespace MGOLServer;

public class TcpServer(int port)
{
    private readonly TcpListener _tcpListener = new(IPAddress.Any, port);
    private bool _isRunning;
    
    public void Start()
    {
        _isRunning = true;
        _tcpListener.Start();
        Console.WriteLine($"Server started on {_tcpListener.LocalEndpoint}...");
        
        while (_isRunning)
        {
            var client = _tcpListener.AcceptTcpClient();
            _ = Task.Run(() => HandleClientAsync(client));
        }
    }
    private async void HandleClientAsync(object obj)
    {
        using var tcpClient = (TcpClient)obj;
        var client = new Client(tcpClient);
        var isRunning = true;
        try
        {
            while (isRunning)
            {
                var typeByte = await client.ReadByteAsync();
                
                if (typeByte == 0)
                {
                    Console.WriteLine("Client disconnected");
                    var session = SessionManager.Instance.GetSession(client);
                    session?.Disconnect(client);
                    break;
                }
                IPacket packet = PacketsManager.Instance.GetPacket(typeByte);
                
                var length = BitConverter.ToInt32(await client.ReadBytesAsync(4));
                Console.WriteLine("Receive packet: " + packet.GetType() + ", payload length: " + length);
                byte[] payload;
                if (length != 0)
                {
                    payload = await client.ReadBytesAsync(length);
                }
                else
                {
                    payload = new byte[]{0};
                }
                
                var response = packet.Handle(client, payload);

                client.Send(response);
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }
}