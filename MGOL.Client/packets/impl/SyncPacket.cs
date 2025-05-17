using MGOL.Client.algorithm;

namespace MGOL.Client.packets.impl;

public class SyncPacket : BasePacket
{
    public SyncPacket() : base(PacketType.Sync)
    {
        
    }

    public SyncPacket(int step, int[,] array) : base(PacketType.Sync)
    {
        _payload = BitConverter.GetBytes(step).Concat(Int2DArrayToByteArray(array)).ToArray();
    }
    public override void Handle(byte[] payload)
    {
        var step = BitConverter.ToInt32(payload);
        var array = ByteArrayToInt2DArray(GetSubArray(payload, 4, payload.Length - 4));
        if (Program.Algorithm.Step != step)
        {
            Program.Algorithm.Step = step;
            SwitchValues(array);
            Program.Algorithm.CurrentGrid = array;
            Program.MainForm.Invalidate();
        }
    }
    
    private static int[,] ByteArrayToInt2DArray(byte[] data)
    {
        int rows = BitConverter.ToInt32(data, 0);
        int cols = BitConverter.ToInt32(data, sizeof(int));
        int[,] array = new int[rows, cols];

        Buffer.BlockCopy(data, sizeof(int) * 2, array, 0, sizeof(int) * rows * cols);
        return array;
    }
    
    private static byte[] Int2DArrayToByteArray(int[,] array)
    {
        int rows = array.GetLength(0);
        int cols = array.GetLength(1);
        byte[] result = new byte[sizeof(int) * rows * cols + sizeof(int) * 2];

        Buffer.BlockCopy(BitConverter.GetBytes(rows), 0, result, 0, sizeof(int));
        Buffer.BlockCopy(BitConverter.GetBytes(cols), 0, result, sizeof(int), sizeof(int));
        Buffer.BlockCopy(array, 0, result, sizeof(int) * 2, sizeof(int) * rows * cols);

        return result;
    }
    
    private static byte[] GetSubArray(byte[] source, int startIndex, int length)
    {
        byte[] result = new byte[length];
        Array.Copy(source, startIndex, result, 0, length);
        return result;
    }

    private void SwitchValues(int[,] array)
    {
        for (var i = 0; i<Program.Algorithm.Cols; i++)
        {
            for (var j = 0; j<Program.Algorithm.Rows; j++)
            {
                if (array[i, j] == 2)
                {
                    array[i, j] = 1;
                }
                else if (array[i, j] == 1)
                {
                    array[i, j] = 2;
                }
            }
        }
    }
}