class Day4
{
    private IEnumerable<int> _numbers;
    private List<Tuple<bool, (int, bool)[][]>> _boards;

    public Day4()
    {
        var input = File.ReadAllLines("input4.txt");
        _numbers = input.First().Split(',').Select(int.Parse);

        _boards = input
        .Skip(1)
        .Chunk(6)
        .Select(c => new Tuple<bool, (int, bool)[][]>(false, c
            .Skip(1)
            .Select(n => n.Split(' ')
            .Where(n => !string.IsNullOrEmpty(n))
            .Select(i => (int.Parse(i), false))
            .ToArray())
            .ToArray())
        ).ToList();
    }

    public long Run()
    {
        foreach (var item in _numbers)
        {
            for (var index = 0; index < _boards.Count; index++)
            {
                if (_boards[index].Item1)
                {
                    continue;
                }
                var board = _boards[index];

                for (int i = 0; i < 5; i++)
                {
                    for (int j = 0; j < 5; j++)
                    {
                        if (board.Item2[i][j].Item1 == item)
                        {
                            board.Item2[i][j].Item2 = true;
                            break;
                        }
                    }
                }

                foreach (var row in board.Item2)
                {
                    if (row.All(i => i.Item2))
                    {
                        _boards[index] = new Tuple<bool, (int, bool)[][]>(true, board.Item2);
                        break;
                    }
                }

                for (int i = 0; i < 5; i++)
                {
                    var col = new List<(int, bool)>();
                    for (int j = 0; j < 5; j++)
                    {
                        col.Add(board.Item2[j][i]);
                    }
                    if (col.All(c => c.Item2))
                    {
                        _boards[index] = new Tuple<bool, (int, bool)[][]>(true, board.Item2);
                        break;
                    }
                }

                if (_boards.All(b => b.Item1))
                {
                    var a = board.Item2.SelectMany(i => i).Where(i => !i.Item2).Select(i => i.Item1).Sum() * item;
                    return a;
                }
            }
        }
        return 0;
    }
}