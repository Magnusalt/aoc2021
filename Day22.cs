using System.Numerics;

class Day22
{
    private IEnumerable<CubicAction> _actions;
    public Day22()
    {
        _actions = File.ReadAllLines("input22.txt").Select(i => new CubicAction(i)).ToList();
        ;
    }

    public long Run()
    {
        var inRange = new Func<int, bool>(i => i >= -50 && i <= 50);
        var cubicsTurnedOn = new HashSet<(int, int, int)>();
        foreach (var cubic in _actions)
        {
            foreach (var z in cubic.ZRange.Where(inRange))
            {
                foreach (var y in cubic.YRange.Where(inRange))
                {
                    foreach (var x in cubic.XRange.Where(inRange))
                    {
                        var c = (x, y, z);
                        if (cubic.On)
                        {
                            cubicsTurnedOn.Add((x, y, z));
                        }
                        else
                        {
                            if (cubicsTurnedOn.Contains(c))
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

        var onSpaces = new List<CubicAction>();


        foreach (var action in _actions)
        {
            onSpaces.AddRange(
                onSpaces
                    .Select(added => Overlap(action, added))
                    .Where(o => o.MinX <= o.MaxX
                        && o.MinY <= o.MaxY
                        && o.MinZ <= o.MaxZ).ToList()
            );
            if (action.On)
            {
                onSpaces.Add(action);
            }
        }

        return onSpaces.Sum(on => on.Size * (on.On ? 1 : -1));
    }

    private CubicAction Overlap(CubicAction c1, CubicAction c2)
    {
        var overlap = $"{(c2.On ? "off" : "on")} x={Math.Max(c1.MinX, c2.MinX)}..{Math.Min(c1.MaxX, c2.MaxX)},y={Math.Max(c1.MinY, c2.MinY)}..{Math.Min(c1.MaxY, c2.MaxY)},z={Math.Max(c1.MinZ, c2.MinZ)}..{Math.Min(c1.MaxZ, c2.MaxZ)}";
        return new CubicAction(overlap);
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
        MinX = x.First();
        MaxX = x.Last();

        var y = ranges[1].Split("..").Select(int.Parse);
        MinY = y.First();
        MaxY = y.Last();
        var z = ranges[2].Split("..").Select(int.Parse);
        MinZ = z.First();
        MaxZ = z.Last();
    }

    public bool On { get; set; }
    public IEnumerable<int> XRange => Enumerable.Range(MinX, MaxX - MinX + 1);
    public IEnumerable<int> YRange => Enumerable.Range(MinY, MaxY - MinY + 1);
    public IEnumerable<int> ZRange => Enumerable.Range(MinZ, MaxZ - MinZ + 1);

    public int MinX { get; set; }
    public int MaxX { get; set; }
    public int MinY { get; set; }
    public int MaxY { get; set; }
    public int MinZ { get; set; }
    public int MaxZ { get; set; }

    public long DeltaX => MaxX - MinX + 1;
    public long DeltaY => MaxY - MinY + 1;
    public long DeltaZ => MaxZ - MinZ + 1;

    public long Size => DeltaX * DeltaY * DeltaZ;

}