using System.Net.Sockets;

namespace MGOLServer;

public class Client(TcpClient client)
{
    public TcpClient TcpClient { get; } = client;
    private NetworkStream _stream = client.GetStream();

    public void Send(byte[] bytes)
    {
        _stream.Write(bytes);
    }

    public async Task<byte[]> ReadBytesAsync(int count)
    {
        byte[] buffer = new byte[count];
        await _stream.ReadAsync(buffer, 0, count);
        return buffer;
    }
    
    public byte[] ReadBytes(int count)
    {
        byte[] buffer = new byte[count];
        _stream.Read(buffer, 0, count);
        return buffer;
    }

    public async Task<byte> ReadByteAsync()
    {
        byte[] buffer = new byte[1];
        var count = await _stream.ReadAsync(buffer, 0, 1);
        if (count == 0) return 0;
        return buffer[0];
    }
}