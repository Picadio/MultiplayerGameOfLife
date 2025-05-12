namespace MGOClient.algorithm;

public interface IAlgorithm<T>
{
    public int Rows { get; }
    public int Cols { get; }
    public T[,] CurrentGrid { get; }
    public void NextGeneration();
    public void SetCellValue(int x, int y, T value);
    public bool InBounds(int x, int y);
    public void Reset();
}