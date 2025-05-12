namespace MGOClient.algorithm;

public abstract class BaseAlgorithm<T>(int rows, int cols) : IAlgorithm<T>
{
    public T[,] CurrentGrid { get; set; } = new T[rows, cols];
    public int Step { get; set; } = 0;
    public int Rows { get; } = rows;
    public int Cols { get; } = cols;
    protected T[,] NextGrid = new T[rows, cols];

    public void SetCellValue(int x, int y, T value)
    {
        if (InBounds(x, y))
            CurrentGrid[x, y] = value;
    }

    public bool InBounds(int x, int y)
    {
        return x >= 0 && y >= 0 && x < Rows && y < Cols;
    }
    
    public abstract void NextGeneration();
    public abstract void Reset();
    public abstract void Rollback(int step);
}