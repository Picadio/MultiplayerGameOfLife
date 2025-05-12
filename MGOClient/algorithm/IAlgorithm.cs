namespace MGOClient.algorithm;

public interface IAlgorithm<T>
{
    public int Rows { get; }
    public int Cols { get; }
    public int Step { get; set; }
    public T[,] CurrentGrid { get; set; }
    public void NextGeneration();
    public void SetCellValue(int x, int y, T value);
    public bool InBounds(int x, int y);
    public void Rollback(int step);
    public void Reset();
}