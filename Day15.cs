class Day15
{
    private int[][] _input;
    private int[][] _inputB;

    public long RunB()
    {
        var matrix = new List<List<int>>();
        for (int i = 0; i < 5; i++)
        {
            for (int y = 0; y < _input.Length; y++)
            {
                var row = new List<int>();
                for (int x = 0; x < _input[0].Length; x++)
                {
                    var v = _input[y][x] + i;

                    row.Add(v > 9 ? v - 9 : v);
                }
                matrix.Add(row);
            }
        }
        var rowLength = _input[0].Length;
        foreach (var row in matrix)
        {
            for (int i = 1; i < 5; i++)
            {
                for (int x = 0; x < rowLength; x++)
                {
                    var v = row[row.Count - rowLength] + 1;
                    row.Add(v > 9 ? v - 9 : v);
                }
            }
        }
        _inputB = matrix.Select(r => r.ToArray()).ToArray();

        var openSet = new Dictionary<string, Node>();
        var closedSet = new Dictionary<string, Node>();

        var xMax = _inputB[0].Length - 1;
        var yMax = _inputB.Length - 1;

        var start = new Node(0, 0, 0, null);

        openSet.Add("0,0", start);

        while (openSet.Any())
        {
            var (key, current) = openSet.OrderBy(n => n.Value.Cost(xMax, yMax)).First();
            openSet.Remove(key);
            if (current.X == xMax && current.Y == yMax)
            {
                break;
            }
            var currentKey = $"{current.X},{current.Y}";

            if (closedSet.ContainsKey(currentKey))
            {
                if (closedSet[currentKey].Cost(xMax, yMax) < current.Cost(xMax, yMax))
                {
                    continue;
                }
                closedSet[currentKey] = current;
            }
            else
            {
                closedSet.Add(currentKey, current);
            }

            foreach (var neighbour in GetNeighboursB(current, xMax, yMax))
            {
                if (neighbour.X == xMax && neighbour.Y == yMax)
                {
                    return neighbour.TravelCost;
                }
                var nkey = $"{neighbour.X},{neighbour.Y}";
                if (closedSet.ContainsKey(nkey))
                {
                    if (closedSet[nkey].Cost(xMax, yMax) < neighbour.Cost(xMax, yMax))
                    {
                        continue;
                    }
                    closedSet[nkey] = neighbour;
                }

                if (openSet.ContainsKey(nkey))
                {
                    if (openSet[nkey].Cost(xMax, yMax) < neighbour.Cost(xMax, yMax))
                    {
                        continue;
                    }
                    openSet[nkey] = neighbour;
                }
                else
                {
                    openSet.Add(nkey, neighbour);
                }
            }
        }

        return 0;
    }

    public long Run()
    {
        var openSet = new Dictionary<string, Node>();
        var closedSet = new Dictionary<string, Node>();

        var xMax = _input[0].Length - 1;
        var yMax = _input.Length - 1;

        var start = new Node(0, 0, 0, null);

        openSet.Add("0,0", start);

        while (openSet.Any())
        {
            var (key, current) = openSet.OrderBy(n => n.Value.Cost(xMax, yMax)).First();
            openSet.Remove(key);
            if (current.X == xMax && current.Y == yMax)
            {
                break;
            }
            var currentKey = $"{current.X},{current.Y}";

            if (closedSet.ContainsKey(currentKey))
            {
                if (closedSet[currentKey].Cost(xMax, yMax) < current.Cost(xMax, yMax))
                {
                    continue;
                }
                closedSet[currentKey] = current;
            }
            else
            {
                closedSet.Add(currentKey, current);
            }

            foreach (var neighbour in GetNeighbours(current, xMax, yMax))
            {
                if (neighbour.X == xMax && neighbour.Y == yMax)
                {
                    return neighbour.TravelCost;
                }
                var nkey = $"{neighbour.X},{neighbour.Y}";
                if (closedSet.ContainsKey(nkey))
                {
                    if (closedSet[nkey].Cost(xMax, yMax) < neighbour.Cost(xMax, yMax))
                    {
                        continue;
                    }
                    closedSet[nkey] = neighbour;
                }

                if (openSet.ContainsKey(nkey))
                {
                    if (openSet[nkey].Cost(xMax, yMax) < neighbour.Cost(xMax, yMax))
                    {
                        continue;
                    }
                    openSet[nkey] = neighbour;
                }
                else
                {
                    openSet.Add(nkey, neighbour);
                }
            }
        }

        return 0;
    }

    private List<Node> GetNeighboursB(Node current, int xMax, int yMax)
    {
        var above = current.Y > 0 ? new Node(current.X, current.Y - 1, current.TravelCost + _inputB[current.Y - 1][current.X], current) : null;
        var below = current.Y < yMax ? new Node(current.X, current.Y + 1, current.TravelCost + _inputB[current.Y + 1][current.X], current) : null;
        var left = current.X > 0 ? new Node(current.X - 1, current.Y, current.TravelCost + _inputB[current.Y][current.X - 1], current) : null;
        var right = current.X < xMax ? new Node(current.X + 1, current.Y, current.TravelCost + _inputB[current.Y][current.X + 1], current) : null;

        return new List<Node> { above, below, left, right }.Where(n => n != null).ToList();
    }
    private List<Node> GetNeighbours(Node current, int xMax, int yMax)
    {
        var above = current.Y > 0 ? new Node(current.X, current.Y - 1, current.TravelCost + _input[current.Y - 1][current.X], current) : null;
        var below = current.Y < yMax ? new Node(current.X, current.Y + 1, current.TravelCost + _input[current.Y + 1][current.X], current) : null;
        var left = current.X > 0 ? new Node(current.X - 1, current.Y, current.TravelCost + _input[current.Y][current.X - 1], current) : null;
        var right = current.X < xMax ? new Node(current.X + 1, current.Y, current.TravelCost + _input[current.Y][current.X + 1], current) : null;

        return new List<Node> { above, below, left, right }.Where(n => n != null).ToList();
    }

    public Day15()
    {
        _input = File.ReadAllLines("input15.txt").Select(row => row.Select(c => (int)char.GetNumericValue(c)).ToArray()).ToArray();

        var x = _input[0].Length;
        var y = _input.Length;
    }
}

internal record Node(int X, int Y, int TravelCost, Node? Parent)
{
    public double Cost(int endX, int endY)
    {
        var manhattan = Math.Abs(endX - X) + Math.Abs(endY - Y);
        return manhattan + TravelCost;
    }
}