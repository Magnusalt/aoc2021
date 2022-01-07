using System.Numerics;
class Day19
{
    private List<Scanner> _scanners;

    public Day19()
    {
        var input = File.ReadAllLines("input19.txt");
        _scanners = new List<Scanner>();

        var currentScanner = new Scanner();
        _scanners.Add(currentScanner);
        foreach (var item in input)
        {
            if (item.StartsWith("---"))
            {
                currentScanner.Name = item;
                continue;
            }
            if (string.IsNullOrEmpty(item))
            {
                currentScanner = new Scanner();
                _scanners.Add(currentScanner);
                continue;
            }
            var coordinates = item.Split(',');
            var detection = new Vector3(int.Parse(coordinates[0]), int.Parse(coordinates[1]), int.Parse(coordinates[2]));
            currentScanner.Detections.Add(detection);
        }
    }

    public (long, long) Run()
    {

        var queue = new Queue<Scanner>();
        var dialedIn = new HashSet<Scanner>();

        queue.Enqueue(_scanners[0]);
        dialedIn.Add(_scanners[0]);

        while (dialedIn.Count < _scanners.Count)
        {
            var currentReference = queue.Dequeue();

            foreach (var item in _scanners.Except(dialedIn))
            {
                if (currentReference.Name == item.Name)
                {
                    continue;
                }
                var success = DialIn(currentReference, item);
                if (success)
                {
                    dialedIn.Add(item);
                    queue.Enqueue(item);
                }
            }
        }

        var unique = dialedIn.SelectMany(d => d.Detections).Distinct().Count();

        var apart = dialedIn.DifferentCombinations(2).Select(d => Vector3.Subtract(d.First().Position, d.Last().Position)).Select(p => (long)(Math.Abs(p.X) + Math.Abs(p.Y) + Math.Abs(p.Z)));

        return (unique, apart.Max());
    }

    private bool DialIn(Scanner reference, Scanner relative)
    {
        var combinations0 = reference.Detections.DifferentCombinations(2).Select(c => (c.First(), c.Last()));
        var combinations1 = relative.Detections.DifferentCombinations(2).Select(c => (c.First(), c.Last()));

        var lengths0 = combinations0.Select(c => (Start: c.Item1, End: c.Item2, Distance: Vector3.Distance(c.Item1, c.Item2)));
        var lengths1 = combinations1.Select(c => (Start: c.Item1, End: c.Item2, Distance: Vector3.Distance(c.Item1, c.Item2)));

        var common = lengths0.Join(lengths1, (d) => d.Distance, (d) => d.Distance, (d1, d2) => (Start1: d1.Start, End1: d1.End, Start2: d2.Start, End2: d2.End));

        if (common.Count() < 66)
        {
            return false;
        }

        var mappings = new Dictionary<Vector3, List<Vector3>>();

        foreach (var (Start1, End1, Start2, End2) in common)
        {
            if (mappings.ContainsKey(Start1))
            {
                mappings[Start1].Add(Start2);
                mappings[Start1].Add(End2);
            }
            else
            {
                mappings[Start1] = new List<Vector3> { Start2, End2 };
            }
            if (mappings.ContainsKey(End1))
            {
                mappings[End1].Add(Start2);
                mappings[End1].Add(End2);
            }
            else
            {
                mappings[End1] = new List<Vector3> { Start2, End2 };
            }
        }

        var finalMapping = new Dictionary<Vector3, Vector3>();

        foreach (var (key, value) in mappings)
        {
            finalMapping[key] = mappings[key].GroupBy(v => v).Select(g => (g.Key, g.Count())).OrderByDescending(t => t.Item2).Select(t => t.Key).First();
        }

        var diffs = finalMapping.Select((kv) => Vector3.Subtract(kv.Key, kv.Value));



        var rotateX = Matrix4x4.CreateRotationX((float)Math.PI / 2);
        var rotateY = Matrix4x4.CreateRotationY((float)Math.PI / 2);
        var rotateZ = Matrix4x4.CreateRotationZ((float)Math.PI / 2);

        var allEqual = new HashSet<Vector3>(diffs).Count == 1;

        var currentRotation = rotateX;
        var allReadings = relative.Detections;
        while (!allEqual)
        {
            finalMapping = finalMapping.ToDictionary(kv => kv.Key, (kv) =>
            {
                var transformed = Vector3.Transform(kv.Value, currentRotation);
                return new Vector3((float)Math.Round(transformed.X, 0), (float)Math.Round(transformed.Y, 0), (float)Math.Round(transformed.Z, 0));
            });
            diffs = finalMapping.Select((kv) => Vector3.Subtract(kv.Key, kv.Value));

            allReadings = allReadings.Select((d) =>
            {
                var transformed = Vector3.Transform(d, currentRotation);
                return new Vector3((float)Math.Round(transformed.X, 0), (float)Math.Round(transformed.Y, 0), (float)Math.Round(transformed.Z, 0));
            }).ToList();


            allEqual = new HashSet<Vector3>(diffs).Count == 1;
            if (allEqual)
            {
                relative.Detections = allReadings.Select(d => Vector3.Add(d, diffs.First())).ToList();
                relative.Position = diffs.First();
                return true;
            }
            var firstDiff = diffs.First();
            if (diffs.All(v => v.X == firstDiff.X))
            {
                currentRotation = rotateX;
            }
            if (diffs.All(v => v.Y == firstDiff.Y))
            {
                currentRotation = rotateY;
            }
            if (diffs.All(v => v.Z == firstDiff.Z))
            {
                currentRotation = rotateZ;
            }
        }
        return false;
    }

}

class Scanner
{
    public Scanner()
    {
        Detections = new List<Vector3>();
    }
    public string Name { get; set; }
    public Vector3 Position { get; set; }
    public List<Vector3> Detections { get; set; }
}

static class Extensions
{
    public static IEnumerable<IEnumerable<Vector3>> DifferentCombinations<Vector3>(this IEnumerable<Vector3> elements, int k)
    {
        return k == 0 ? new[] { new Vector3[0] } :
          elements.SelectMany((e, i) =>
            elements.Skip(i + 1).DifferentCombinations(k - 1).Select(c => (new[] { e }).Concat(c)));
    }
}