class Day11
{
    private const int _steps = 100;
    private int _xMax;
    private int[][] _input;
    private int _yMax;

    public Day11()
    {
        _input = File.ReadAllLines("input11.txt").Select(row => row.Select(c => (int)char.GetNumericValue(c)).ToArray()).ToArray();
        _yMax = _input.Length;
        _xMax = _input[0].Length;
    }

    public long Run()
    {
        var flashes = 0;
        for (int step = 0; step < _steps; step++)
        {
            var willFlash = new Queue<(int x, int y)>();
            for (int y = 0; y < _yMax; y++)
            {
                for (int x = 0; x < _xMax; x++)
                {
                    _input[y][x]++;
                    if (_input[y][x] == 10)
                    {
                        willFlash.Enqueue((x, y));
                    }
                }
            }

            while (willFlash.Count > 0)
            {
                var curr = willFlash.Dequeue();
                flashes++;

                var x_s = curr.x > 0 ? curr.x - 1 : curr.x;
                var x_e = curr.x < _xMax - 1 ? curr.x + 1 : curr.x;
                var y_s = curr.y > 0 ? curr.y - 1 : curr.y;
                var y_e = curr.y < _yMax - 1 ? curr.y + 1 : curr.y;

                for (var y_c = y_s; y_c <= y_e; y_c++)
                {
                    for (var x_c = x_s; x_c <= x_e; x_c++)
                    {
                        _input[y_c][x_c]++;

                        if (_input[y_c][x_c] == 10)
                        {
                            willFlash.Enqueue((x_c, y_c));
                        }
                    }
                }
            }
            for (int y = 0; y < _yMax; y++)
            {
                for (int x = 0; x < _xMax; x++)
                {
                    if (_input[y][x] > 9)
                    {
                        _input[y][x] = 0;
                    }
                }
            }

            //PrintMatrix();

        }

        return flashes;
    }
    public long RunB()
    {
        var index = 0;
        var allFlashed = false;
        while (!allFlashed)
        {
            var willFlash = new Queue<(int x, int y)>();
            for (int y = 0; y < _yMax; y++)
            {
                for (int x = 0; x < _xMax; x++)
                {
                    _input[y][x]++;
                    if (_input[y][x] == 10)
                    {
                        willFlash.Enqueue((x, y));
                    }
                }
            }

            while (willFlash.Count > 0)
            {
                var curr = willFlash.Dequeue();

                var x_s = curr.x > 0 ? curr.x - 1 : curr.x;
                var x_e = curr.x < _xMax - 1 ? curr.x + 1 : curr.x;
                var y_s = curr.y > 0 ? curr.y - 1 : curr.y;
                var y_e = curr.y < _yMax - 1 ? curr.y + 1 : curr.y;

                for (var y_c = y_s; y_c <= y_e; y_c++)
                {
                    for (var x_c = x_s; x_c <= x_e; x_c++)
                    {
                        _input[y_c][x_c]++;

                        if (_input[y_c][x_c] == 10)
                        {
                            willFlash.Enqueue((x_c, y_c));
                        }
                    }
                }
            }
            for (int y = 0; y < _yMax; y++)
            {
                for (int x = 0; x < _xMax; x++)
                {
                    if (_input[y][x] > 9)
                    {
                        _input[y][x] = 0;
                    }
                }
            }

            allFlashed = _input.SelectMany(i => i).All(i => i == 0);
            //PrintMatrix();
            index++;
        }

        return index;
    }

    private void PrintMatrix()
    {
        for (int y = 0; y < _yMax; y++)
        {
            for (int x = 0; x < _xMax; x++)
            {
                System.Console.Write(_input[y][x]);
            }
            System.Console.WriteLine();
        }
        System.Console.WriteLine("==============");
    }
}