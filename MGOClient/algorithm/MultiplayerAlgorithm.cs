namespace MGOClient.algorithm;

public class MultiplayerAlgorithm(int rows, int cols) : BaseAlgorithm<int>(rows, cols)
{
    public override void NextGeneration()
    {
        for (int x = 0; x < Rows; x++)
        {
            for (int y = 0; y < Cols; y++)
            {
                var currentValue = CurrentGrid[x, y];
                var info = GetNeighborsInfo(x, y, currentValue);
                
                bool isAlive = currentValue > 0;

                if (isAlive && (info.FriendsCount + 1 < info.EnemiesCount))
                    NextGrid[x, y] = 0;
                else if (isAlive && (info.FriendsCount < 2 || info.FriendsCount > 3))
                    NextGrid[x, y] = 0;  // Die
                else if (!isAlive && info.FriendsCount == 3)
                    NextGrid[x, y] = info.FriendValue;   // Born
                else
                    NextGrid[x, y] = CurrentGrid[x, y]; // Stay the same
                
            }
        }

        // Swap grids
        (CurrentGrid, NextGrid) = (NextGrid, CurrentGrid);
    }

    public override void Reset()
    {
        for (int x = 0; x < cols; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                CurrentGrid[x, y] = 0;
                NextGrid[x, y] = 0;
            }
        }
    }

    private NeighboursInfo GetNeighborsInfo(int x, int y, int value)
    {
        Dictionary<int, int> neighbors = new Dictionary<int, int>();
        for (int dx = -1; dx <= 1; dx++)
        {
            for (int dy = -1; dy <= 1; dy++)
            {
                if (dx == 0 && dy == 0) continue;
                int nx = x + dx;
                int ny = y + dy;
                if (InBounds(nx, ny))
                {
                    var currentValue = CurrentGrid[nx, ny];
                    if (currentValue == 0) continue;
                    if (!neighbors.ContainsKey(currentValue))
                        neighbors[currentValue] = 1;
                    else
                        neighbors[currentValue]++;
                }
                
            }
        }

        var info = new NeighboursInfo
        {
            EnemiesCount = neighbors.Where(neighbor => neighbor.Key != value && neighbor.Key != 0).Sum(val => val.Value),
        };
        if (value == 0)
        {
            var maxCountNeighbor = neighbors.FirstOrDefault(neighbor => neighbor.Value == neighbors.Max(neighbor2 => neighbor2.Value));
            if (!maxCountNeighbor.Equals(default(KeyValuePair<int, int>)))
            {
                info.FriendValue = maxCountNeighbor.Key;
            }
        }
        else
        {
            info.FriendValue = value;
        }
        
        var friends = neighbors
            .FirstOrDefault(neighbor => neighbor.Key == info.FriendValue);
        
        if (!friends.Equals(default(KeyValuePair<int, int>)))
        {
            info.FriendsCount = friends.Value;
        }
        
        return info;
    }
    
    private struct NeighboursInfo
    {
        public int FriendsCount { get; set; } = 0;
        public int EnemiesCount { get; set; } = 0;
        public int FriendValue { get; set; } = 0;
        
        public NeighboursInfo()
        {
        }
    }
}
