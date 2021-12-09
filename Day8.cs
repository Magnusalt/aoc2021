class Day8
{
    private string[] _input;

    public Day8()
    {
        _input = File.ReadAllLines("input8.txt");
    }

    public long Run()
    {
        var count = 0;
        foreach (var item in _input)
        {
            var io = item.Split('|');
            var o = io[1].Split(' ');
            count += o.Count(seg => seg.Length == 2 || seg.Length == 3 || seg.Length == 4 || seg.Length == 7);
        }
        return count;
    }

    public long RunB()
    {
        long count = 0;
        foreach (var item in _input)
        {
            var io = item.Split('|');
            var o = io[1].Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var i = io[0].Split(' ', StringSplitOptions.RemoveEmptyEntries);

            var one = i.Single(seg => seg.Length == 2);
            var seven = i.Single(seg => seg.Length == 3);
            var four = i.Single(seg => seg.Length == 4);
            var eight = i.Single(seg => seg.Length == 7);

            var topSegement = seven.Except(one).Single();

            var six = i.Where(seg => seg.Length == 6).Where(seg => !seg.Contains(one[0]) ^ !seg.Contains(one[1])).Single();

            var topRight = eight.Except(six);
            var bottomRight = one.Except(topRight);

            var two = i.Where(seg => seg.Length == 5 && !seg.Contains(bottomRight.Single())).Single();

            var threeOrFive = i.Where(seg => seg.Length == 5 && seg != two).Select(s => (s, new HashSet<char>(s.ToArray())));
            var zeroOrNine = i.Where(seg => seg.Length == 6 && seg != six).Select(s => (s, new HashSet<char>(s.ToArray())));
            var fourSet = new HashSet<char>(four);

            var nine = zeroOrNine.Where(s => s.Item2.IsSupersetOf(fourSet)).Single().s;
            var zero = i.Where(seg => seg.Length == 6 && seg != six && seg != nine).Single();
            var sevenSet = new HashSet<char>(seven);
            var three = threeOrFive.Where(s => s.Item2.IsSupersetOf(sevenSet)).Single().s;
            var five = i.Where(seg => seg.Length == 5 && seg != two && seg != three).Single();

            var oneSet = new HashSet<char>(one);
            var twoSet = new HashSet<char>(two);
            var threeSet = new HashSet<char>(three);
            var fiveSet = new HashSet<char>(five);
            var sixSet = new HashSet<char>(six);
            var eightSet = new HashSet<char>(eight);
            var nineSet = new HashSet<char>(nine);
            var zeroSet = new HashSet<char>(zero);

            var ten = 3;
            var displayOutput = 0;
            foreach (var output in o)
            {
                var value = output switch
                {
                    var d when new HashSet<char>(d).SetEquals(oneSet) => 1,
                    var d when new HashSet<char>(d).SetEquals(twoSet) => 2,
                    var d when new HashSet<char>(d).SetEquals(threeSet) => 3,
                    var d when new HashSet<char>(d).SetEquals(fourSet) => 4,
                    var d when new HashSet<char>(d).SetEquals(fiveSet) => 5,
                    var d when new HashSet<char>(d).SetEquals(sixSet) => 6,
                    var d when new HashSet<char>(d).SetEquals(sevenSet) => 7,
                    var d when new HashSet<char>(d).SetEquals(eightSet) => 8,
                    var d when new HashSet<char>(d).SetEquals(nineSet) => 9,
                    var d when new HashSet<char>(d).SetEquals(zeroSet) => 0,
                };
                displayOutput += value * (int)Math.Pow(10, ten);
                ten--;
            }
            count += displayOutput;

        }
        return count;
    }

}