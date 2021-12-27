class Day22
{
    private IEnumerable<CubicAction> _actions;
    public Day22()
    {
        _actions = File.ReadAllLines("input22.txt").Select(i=> new CubicAction(i)).ToList();
    ;
    }

    public long Run()
    {
        var inRange = new Func<int,bool>(i=>i >= -50 && i <= 50);
        var cubicsTurnedOn = new HashSet<(int, int, int)>();
        foreach(var cubic in _actions)
        {
            foreach(var z in cubic.ZRange.Where(inRange))
            {
                foreach(var y in cubic.YRange.Where(inRange))
                {
                    foreach(var x in cubic.XRange.Where(inRange))
                    {
                        var c = (x, y, z);
                        if(cubic.On)
                        {
                            cubicsTurnedOn.Add((x,y,z));
                        }
                        else
                        {
                            if(cubicsTurnedOn.Contains(c))
                            {
                                cubicsTurnedOn.Remove(c);
                            }
                        }
                    }
                }
            }
        }
        return cubicsTurnedOn.Count();
    }
    public long RunB()
    {


        return 0;
    }
}
class CubicAction
{
    public CubicAction(string template)
    {
        var actionParts = template.Split(' ');
        On = actionParts[0] == "on";
        var ranges = actionParts[1].Split(',').Select(r => r.Substring(2)).ToList();

        var x = ranges[0].Split("..").Select(int.Parse);
        XRange = ParseRange((x.First(), x.Last()));
        
        var y = ranges[1].Split("..").Select(int.Parse);
        YRange = ParseRange((y.First(), y.Last()));

        var z = ranges[2].Split("..").Select(int.Parse);
        ZRange = ParseRange((z.First(), z.Last()));
    }

    private IEnumerable<int> ParseRange((int, int) startStop) {
        var max = Math.Max(startStop.Item1, startStop.Item2);
        var min = Math.Min(startStop.Item1, startStop.Item2);
        return Enumerable.Range(min, max - min + 1);
    }
    public bool On { get; set; }
    public IEnumerable<int> XRange { get; set; }
    public IEnumerable<int> YRange { get; set; }
    public IEnumerable<int> ZRange { get; set; }

}