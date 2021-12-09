class Day9
{
    private int[][] _input;
    private int _xMax;
    private int _yMax;

    public Day9()
    {
        _input = File.ReadAllLines("input9.txt").Select(row => row.Select(c => (int)char.GetNumericValue(c)).ToArray()).ToArray();
    }

    public long Run()
    {
        var xMax = _input[0].Length;
        var yMax = _input.Length;
        long riskSum = 0;
        for (int y = 0; y < yMax; y++)
        {
            for (int x = 0; x < xMax; x++)
            {
                var current = _input[y][x];

                var above = y > 0 ? _input[y - 1][x] : 10;
                var below = y < yMax - 1 ? _input[y + 1][x] : 10;
                var left = x > 0 ? _input[y][x - 1] : 10;
                var right = x < xMax - 1 ? _input[y][x + 1] : 10;

                if (above > current && below > current && left > current && right > current)
                {
                    riskSum += current + 1;
                }

            }
        }
        return riskSum;
    }

    public long RunB()
    {
        _xMax = _input[0].Length;
        _yMax = _input.Length;

        var basinLows = new List<(int x, int y)>();
        for (int y = 0; y < _yMax; y++)
        {
            for (int x = 0; x < _xMax; x++)
            {
                var current = _input[y][x];

                var above = y > 0 ? _input[y - 1][x] : 10;
                var below = y < _yMax - 1 ? _input[y + 1][x] : 10;
                var left = x > 0 ? _input[y][x - 1] : 10;
                var right = x < _xMax - 1 ? _input[y][x + 1] : 10;

                if (above > current && below > current && left > current && right > current)
                {
                    basinLows.Add((x, y));
                }
            }
        }

        var basinSizes = new List<int>();
        foreach (var basin in basinLows)
        {
            var visited = new HashSet<(int x, int y)>();
            Dfs(basin, visited);
            basinSizes.Add(visited.Count);
        }

        return basinSizes.OrderByDescending(b => b).Take(3).Aggregate(1, (s, v) => s * v);
    }

    private void Dfs((int x, int y) root, HashSet<(int x, int y)> visited)
    {
        visited.Add(root);
        var adjacent = new List<(int x, int y)>();
        if (root.y > 0 && _input[root.y - 1][root.x] < 9)
        {
            adjacent.Add((root.x, root.y - 1));
        }
        if (root.y < _yMax - 1 && _input[root.y + 1][root.x] < 9)
        {
            adjacent.Add((root.x, root.y + 1));
        }
        if (root.x > 0 && _input[root.y][root.x - 1] < 9)
        {
            adjacent.Add((root.x - 1, root.y));
        }
        if (root.x < _xMax - 1 && _input[root.y][root.x + 1] < 9)
        {
            adjacent.Add((root.x + 1, root.y));
        }

        foreach (var item in adjacent)
        {
            if (!visited.Contains(item))
            {
                Dfs(item, visited);
            }
        }
    }
}