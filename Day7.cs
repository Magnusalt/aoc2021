class Day7
{
    private List<int> _input;
    private int _min;
    private int _max;

    public Day7()
    {
        _input = File.ReadAllText("input7.txt")
            .Split(',')
            .Select(int.Parse)
            .ToList();
        _min = _input.Min();
        _max = _input.Max();
    }

    public long Run()
    {
        var lowest = int.MaxValue;
        foreach (int i in Enumerable.Range(_min, _max))
        {
            var fuelCost = _input
                .Aggregate(0, (sum, pos) => sum += Math.Abs(pos - i));

            lowest = fuelCost < lowest ? fuelCost : lowest;
        }
        return lowest;
    }

    public double RunB()
    {
        var lowest = double.MaxValue;
        foreach (int i in Enumerable.Range(_min, _max))
        {
            var fuelCost = _input
                .Aggregate(0.0, (sum, pos) => sum += ((double)Math.Abs(pos - i) / 2) * (Math.Abs(pos - i) + 1));
            lowest = fuelCost < lowest ? fuelCost : lowest;
        }
        return lowest;
    }
}