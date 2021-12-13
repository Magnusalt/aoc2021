class Day12
{
    private string[] _input;

    private Dictionary<string, List<string>> _nodes;
    public Day12()
    {
        _input = File.ReadAllLines("input12.txt");
        _nodes = new Dictionary<string, List<string>>();
        foreach (var node in _input)
        {
            var edge = node.Split('-');
            if (_nodes.TryAdd(edge[0], new List<string> { edge[1] }))
            {
            }
            else
            {
                _nodes[edge[0]].Add(edge[1]);
            }
            if (_nodes.TryAdd(edge[1], new List<string> { edge[0] }))
            {
            }
            else
            {
                _nodes[edge[1]].Add(edge[0]);
            }
        }
    }

    public long Run()
    {
        var stack = new Queue<(string, List<string>, string)>();
        stack.Enqueue(("start", _nodes["start"], ""));
        var paths = new HashSet<string>();

        while (stack.Count > 0)
        {
            var node = stack.Dequeue();
            if (node.Item1 == "end")
            {
                paths.Add(node.Item3 + "," + "end");
                continue;
            }

            foreach (var item in node.Item2.Where(n => n != "start" && !node.Item3.Contains(n.ToLower())))
            {
                stack.Enqueue((item, _nodes[item], node.Item3 + "," + node.Item1));
            }
        }

        return paths.Count;
    }
    public long RunB()
    {
        var stack = new Queue<(string, List<string>, string)>();
        stack.Enqueue(("start", _nodes["start"], ""));
        var paths = new HashSet<string>();

        while (stack.Count > 0)
        {
            var node = stack.Dequeue();
            if (node.Item1 == "end")
            {
                paths.Add(node.Item3 + "," + "end");
                continue;
            }
            if (node.Item3.Contains(node.Item1.ToLower()))
            {
                if (node.Item3
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Where(c => c == c.ToLower())
                    .GroupBy(c => c).Any(g => g.Count() > 1))
                {
                    continue;
                }
            }

            foreach (var item in node.Item2.Where(n => n != "start"))
            {
                if (item == item.ToUpper())
                {
                    stack.Enqueue((item, _nodes[item], node.Item3 + "," + node.Item1));
                    continue;
                }
                if (!node.Item3.Contains(item.ToLower()))
                {
                    stack.Enqueue((item, _nodes[item], node.Item3 + "," + node.Item1));
                    continue;
                }
                else if (node.Item3.Contains(item.ToLower()))
                {
                    if (node.Item3
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Where(c => c == c.ToLower())
                    .GroupBy(c => c).All(g => g.Count() == 1))
                    {
                        stack.Enqueue((item, _nodes[item], node.Item3 + "," + node.Item1));
                        continue;
                    }
                }
            }
        }

        return paths.Count;
    }
}
