class Day17
{
    private (int, int) _targetX = (79, 137);
    private (int, int) _targetY = (-176, -117);
    public Day17()
    {

    }

    public long Run()
    {
        var x = 0;
        while (Enumerable.Range(1, x).Sum() < _targetX.Item1)
        {
            x++;
        }

        var hieghtScore = new List<int>();
        for (int i = 0; i < 1000; i++)
        {
            var yMax = Enumerable.Range(1, i).Sum();
            var yV = 0;
            var y = yMax;
            while (y > _targetY.Item2)
            {
                y = y + yV;
                yV--;
            }
            if (IsInTargetAreaY(y))
            {
                hieghtScore.Add(yMax);
            }
        }

        return hieghtScore.Max();
    }

    public long RunB()
    {
        var minX = 0;
        while (Enumerable.Range(1, minX).Sum() < _targetX.Item1)
        {
            minX++;
        }
        var maxX = _targetX.Item2;

        var hieghtScore = new List<(int, int)>();
        for (int i = 0; i < Math.Abs(_targetY.Item1); i++)
        {
            var yMax = Enumerable.Range(1, i).Sum();
            var yV = 0;
            var y = yMax;
            while (y > _targetY.Item2)
            {
                y = y + yV;
                yV--;
            }
            if (IsInTargetAreaY(y))
            {
                hieghtScore.Add((yMax, i));
            }
        }
        var maxY = hieghtScore.OrderBy(p => p.Item1).Last().Item2;
        var minY = _targetY.Item1;

        var foundIntialVelocities = 0;
        for (int y = minY; y <= maxY; y++)
        {
            for (int x = minX; x <= maxX; x++)
            {
                if (IsInTargetAreaX(x) && y >= 0)
                {
                    continue;
                }
                var time = 0;
                var posX = 0;
                var posY = 0;
                var veloX = x;
                var veloY = y;
                var trajectory = new List<(int, int)>();
                while (time < 2 * Math.Abs(_targetY.Item1) + 1)
                {
                    if (trajectory.Any(p => IsInTargetAreaX(p.Item1) && IsInTargetAreaY(p.Item2)))
                    {
                        foundIntialVelocities++;
                        break;
                    }
                    posX += veloX;
                    posY += veloY;
                    trajectory.Add((posX, posY));
                    veloY--;
                    veloX = veloX > 0 ? veloX - 1 : 0;
                    time++;
                }
            }
        }

        return foundIntialVelocities;
    }
    private bool IsInTargetAreaX(int x)
    {
        return x >= _targetX.Item1 && x <= _targetX.Item2;
    }
    private bool IsInTargetAreaY(int y)
    {
        return y >= _targetY.Item1 && y <= _targetY.Item2;
    }
}