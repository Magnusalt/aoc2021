class Day5
{
    private IEnumerable<Line> _input;

    public record Line(int x1, int y1, int x2, int y2);
    public Day5()
    {

        _input = File.ReadAllLines("input5.txt").Select(s =>
        {
            var three = s.Split(',');
            var x1 = int.Parse(three[0]);
            var y2 = int.Parse(three[2]);
            var mid = three[1].Split(' ');
            var y1 = int.Parse(mid[0]);
            var x2 = int.Parse(mid[2]);
            return new Line(x1, y1, x2, y2);
        });
    }


    public long RunA()
    {
        var significantLines = _input.Where(l => l.x1 == l.x2 || l.y1 == l.y2);
        var maxX = Math.Max(significantLines.Max(l => l.x1), significantLines.Max(l => l.x2)) + 1;
        var maxY = Math.Max(significantLines.Max(l => l.y1), significantLines.Max(l => l.y2)) + 1;

        var array = new int[maxX, maxY];

        foreach (var line in significantLines)
        {
            if (line.x1 == line.x2)
            {
                if (line.y1 < line.y2)
                {
                    for (int i = line.y1; i <= line.y2; i++)
                    {
                        array[line.x1, i] += 1;
                    }
                }
                if (line.y1 > line.y2)
                {
                    for (int i = line.y2; i <= line.y1; i++)
                    {
                        array[line.x1, i] += 1;
                    }
                }
            }

            if (line.y1 == line.y2)
            {
                if (line.x1 < line.x2)
                {
                    for (int i = line.x1; i <= line.x2; i++)
                    {
                        array[i, line.y1] += 1;
                    }
                }
                if (line.x1 > line.x2)
                {
                    for (int i = line.x2; i <= line.x1; i++)
                    {
                        array[i, line.y1] += 1;
                    }
                }
            }
        }

        var total = 0;
        for (int i = 0; i < maxX; i++)
        {
            for (int j = 0; j < maxY; j++)
            {
                if (array[i, j] >= 2)
                {
                    total++;
                }
            }
        }
        return total;
    }

    public long RunB()
    {
        var maxX = Math.Max(_input.Max(l => l.x1), _input.Max(l => l.x2)) + 1;
        var maxY = Math.Max(_input.Max(l => l.y1), _input.Max(l => l.y2)) + 1;

        var array = new int[maxX, maxY];

        foreach (var line in _input)
        {
            if (line.x1 == line.x2)
            {
                if (line.y1 < line.y2)
                {
                    for (int i = line.y1; i <= line.y2; i++)
                    {
                        array[line.x1, i] += 1;
                    }
                }
                if (line.y1 > line.y2)
                {
                    for (int i = line.y2; i <= line.y1; i++)
                    {
                        array[line.x1, i] += 1;
                    }
                }
            }

            if (line.y1 == line.y2)
            {
                if (line.x1 < line.x2)
                {
                    for (int i = line.x1; i <= line.x2; i++)
                    {
                        array[i, line.y1] += 1;
                    }
                }
                if (line.x1 > line.x2)
                {
                    for (int i = line.x2; i <= line.x1; i++)
                    {
                        array[i, line.y1] += 1;
                    }
                }
            }

            if (line.x1 != line.x2 && line.y1 != line.y2)
            {
                if (line.x1 < line.x2)
                {
                    if (line.y1 < line.y2)
                    {
                        var x = line.x1;
                        var y = line.y1;
                        while (x <= line.x2 && y <= line.y2)
                        {
                            array[x, y] += 1;
                            x++;
                            y++;
                        }
                    }
                    if (line.y1 > line.y2)
                    {
                        var x = line.x1;
                        var y = line.y1;
                        while (x <= line.x2 && y >= line.y2)
                        {
                            array[x, y] += 1;
                            x++;
                            y--;
                        }
                    }
                }

                if (line.x1 > line.x2)
                {
                    if (line.y1 < line.y2)
                    {
                        var x = line.x1;
                        var y = line.y1;
                        while (x >= line.x2 && y <= line.y2)
                        {
                            array[x, y] += 1;
                            x--;
                            y++;
                        }
                    }

                    if (line.y1 > line.y2)
                    {
                        var x = line.x1;
                        var y = line.y1;
                        while (x >= line.x2 && y >= line.y2)
                        {
                            array[x, y] += 1;
                            x--;
                            y--;
                        }
                    }
                }
            }
        }

        var total = 0;
        for (int i = 0; i < maxY; i++)
        {
            for (int j = 0; j < maxX; j++)
            {
                if (array[i, j] >= 2)
                {
                    total++;
                }
            }
        }
        return total;
    }
}