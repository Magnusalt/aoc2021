using System.Text;

class Day14
{
    private string _starting;
    private Dictionary<string, List<string>> _insertRules;

    public Day14()
    {
        var input = File.ReadAllLines("input14.txt");
        _starting = input.First();
        _insertRules = input.Skip(2).Select(r => r.Split("->", StringSplitOptions.TrimEntries)).ToDictionary(r => r[0], r => new List<string> { $"{r[0][0]}{r[1]}", $"{r[1]}{r[0][1]}" });
    }

    public long Run()
    {
        var current = _starting;
        for (int _ = 0; _ < 10; _++)
        {
            var sb = new StringBuilder();

            var index = 0;
            while (index < current.Length - 1)
            {
                var pair = current.Substring(index, 2);
                if (_insertRules.TryGetValue(pair, out var insert))
                {
                    sb.Append(pair[0]);
                    sb.Append(insert);
                    //sb.Append(pair[1]);
                }
                index++;
            }
            sb.Append(current.Last());

            current = sb.ToString();
        }

        var ordered = current.GroupBy(c => c).OrderByDescending(g => g.Count());

        return ordered.First().Count() - ordered.Last().Count();
    }
    public long RunB()
    {
        var pairCounts = new Dictionary<string, long>();

        for (int i = 0; i < _starting.Length - 1; i++)
        {
            var pair = _starting.Substring(i, 2);
            if (pairCounts.ContainsKey(pair))
            {
                pairCounts[pair]++;
            }
            else
            {
                pairCounts.Add(pair, 1);
            }
        }



        for (int _ = 0; _ < 40; _++)
        {
            var nextCounts = new Dictionary<string, long>();
            foreach (var pair in pairCounts)
            {
                foreach (var item in _insertRules[pair.Key])
                {
                    if (nextCounts.ContainsKey(item))
                    {
                        nextCounts[item] += pair.Value;
                    }
                    else
                    {
                        nextCounts.Add(item, pair.Value);
                    }
                }
            }
            pairCounts = nextCounts;
        }

        var score = new Dictionary<char, long>();

        foreach (var item in pairCounts)
        {
            // var key1 = item.Key[0];
            var key2 = item.Key[1];

            // if (score.ContainsKey(key1))
            // {
            //     score[key1] += item.Value;
            // }
            // else
            // {
            //     score.Add(key1, item.Value);
            // }
            if (score.ContainsKey(key2))
            {
                score[key2] += item.Value;
            }
            else
            {
                score.Add(key2, item.Value);
            }
        }


        return score.Max(kv => kv.Value) - score.Min(kv => kv.Value);
    }

    // To slow...
    // private void Count(int level, string input, Dictionary<string, long> score)
    // {
    //     if (level == 10)
    //     {
    //         return;
    //     }
    //     var product = _insertRules[input];
    //     if (score.ContainsKey(product))
    //     {
    //         score[product]++;
    //     }
    //     else
    //     {
    //         score.Add(product, 1);
    //     }
    //     Count(level + 1, $"{input[0]}{product}", score);
    //     Count(level + 1, $"{product}{input[1]}", score);
    // }
}