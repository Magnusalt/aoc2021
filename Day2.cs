class Day2
{
    private IEnumerable<(string, int)> _input;

    public Day2()
    {
        _input = File.ReadAllLines("input2.txt").Select(l => l.Split(' ')).Select(ar => (ar[0], int.Parse(ar[1])));
    }

    public long Run()
    {
        var horizontal = 0;
        var depth = 0;
        foreach (var item in _input)
        {
            switch (item.Item1)
            {
                case "up":
                    depth -= item.Item2;
                    break;
                case "down":
                    depth += item.Item2;
                    break;
                case "forward":
                    horizontal += item.Item2;
                    break;
            }
        }

        return horizontal * depth;
    }

    public long RunB()
    {
        var aim = 0;
        var horizontal = 0;
        var depth =0;
        foreach (var item in _input)
        {
            switch (item.Item1)
            {
                case "up":
                    aim -= item.Item2;
                    break;
                case "down":
                    aim += item.Item2;
                    break;
                case "forward":
                    horizontal += item.Item2;
                    depth += item.Item2 * aim;
                    break;
            }
        }

        return horizontal * depth;
    }
}