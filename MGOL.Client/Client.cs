using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using MGOL.Client.packets;

namespace MGOL.Client;

public class Client(IPEndPoint ipEndPoint)
{
    private TcpClient _client = new();
    private NetworkStream _stream;
    private CancellationTokenSource _cts;
    private readonly object _lock = new();
    public bool SessionConnected { get; set; } = false;

    public async Task StartAsync()
    {
        _cts = new CancellationTokenSource();
        await ConnectLoop(_cts.Token);
    }
    
    private async Task ConnectLoop(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            try
            {
                if (_client == null || !_client.Connected)
                {
                    Console.WriteLine("Trying to connect...");
                    _client = new TcpClient();
                    await _client.ConnectAsync(ipEndPoint, token);
                    _stream = _client.GetStream();
                    Console.WriteLine("Connected!");

                    // Читання у фоновому потоці
                    _ = Task.Run(() => Listen(token));
                }

                await Task.Delay(2000, token); // Повторна перевірка
            }
            catch (Exception ex)
            {
                Console.WriteLine("Connection failed: " + ex.Message);
                await Task.Delay(5000, token); // Зачекай перед повтором
            }
        }
    }
    public bool IsConnected()
    {
        return _client.Connected && SessionConnected;
    }

    public async Task Listen(CancellationToken token)
    {
        var buffer = new byte[1];
        try
        {
            while (!token.IsCancellationRequested && _client.Connected)
            {
                int bytesRead = await _stream.ReadAsync(buffer, 0, 1, token);
                if (bytesRead == 0)
                {
                    Console.WriteLine("Server closed connection.");
                    Disconnect();
                    break;
                }
                Console.WriteLine("Handle bytes: " + buffer[0] + " "  + bytesRead);
                byte typeByte = buffer[0];
                if(typeByte == 0) continue;
                
                var packet = PacketsManager.Instance.GetPacket(typeByte);
                Console.WriteLine("Handle packet: " + packet.GetType());
                var length = BitConverter.ToInt32(await ReadBytesAsync(4));
                byte[] payload;
                if (length != 0)
                {
                    payload = await ReadBytesAsync(length);
                }
                else
                {
                    payload = new byte[] { 0 };
                }
                
                
                packet.Handle(payload);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Receive error: " + ex.StackTrace);
            Disconnect();
        }
    }
    
    public async Task SendAsync(IPacket packet)
    {
        if (_client.Connected)
        {
            await _stream.WriteAsync(packet.GetBytes());
            await _stream.FlushAsync();
        }
    }
    
    private async Task<byte[]> ReadBytesAsync(int count)
    {
        byte[] buffer = new byte[count];
        await _stream.ReadAsync(buffer, 0, count);
        return buffer;
    }
    
    private void Disconnect()
    {
        lock (_lock)
        {
            _client?.Close();
            _stream = null;
        }
    }

    public void Stop()
    {
        _cts?.Cancel();
        Disconnect();
    }
}