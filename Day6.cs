using System.Collections.Concurrent;
using System.Diagnostics;

class Day6
{
    private List<int> _input;

    public Day6()
    {
        _input = File.ReadAllText("input6.txt").Split(',').Select(int.Parse).ToList();
    }

    public long RunA()
    {
        for (int _ = 0; _ < 80; _++)
        {
            var daysLength = _input.Count;
            for (int j = 0; j < daysLength; j++)
            {
                var curr = _input[j];

                switch (curr)
                {
                    case 0:
                        _input[j] = 6;
                        _input.Add(8);
                        break;
                    default:
                        _input[j]--;
                        break;
                }
            }
        }
        return _input.Count;
    }
    public long RunB_b()
    {
        var result = new ConcurrentBag<long>();
        Parallel.ForEach(_input, (i) => result.Add(CountKids(0, i)));
        return result.Sum() + _input.Count;
    }

    private long CountKids(int day, int fishCycle)
    {
        var nextSpawnDay = day + fishCycle + 1;
        long kids = 0;
        while (nextSpawnDay <= 256)
        {
            kids++;
            kids += CountKids(nextSpawnDay, 8);
            nextSpawnDay += 7;
        }
        return kids;
    }

    public long RunB()
    {
        // not my solution :(
        int dayToPass = 256;

        long[] fishDays = new long[9];
        long[] fishDaysTemp = new long[9];

        foreach (var s in _input)
        {
            fishDays[s]++;
        }

        for (int i = 0; i < dayToPass; i++)
        {
            long nextGen = fishDays[0];

            for (int d = 0; d < 8; d++)
            {
                fishDaysTemp[d] = fishDays[d + 1];
            }

            fishDaysTemp[6] += nextGen;
            fishDaysTemp[8] += nextGen;

            Array.Copy(fishDaysTemp, fishDays, fishDaysTemp.Length);
            Array.Clear(fishDaysTemp, 0, fishDaysTemp.Length);
        }

        return fishDays.Sum();
    }
}