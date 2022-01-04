class Day24
{
    private IEnumerable<string> _program;
    public Day24()
    {

        _program = File.ReadAllLines("input24.txt");
    }

    public string Run()
    {
        var relevantInstructions = _program.Chunk(18).SelectMany(c => new[] { c[4], c[5], c[15] })
                .Select(c => Convert.ToInt32(c.Substring(6))).Chunk(3)
                .Select(c => (divZ: c[0], addX: c[1], addY: c[2]))
                .ToArray();

        var zIsZero = new List<List<int>>();
        var digits = new List<int> { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
        int step = 0;
        long z = 0;
        while (!digits.All(d => d == 9))
        {
            if (step == 14)
            {
                if (z == 0)
                {
                    break;
                }
            }
            var last = zIsZero.LastOrDefault();

            var instruction = relevantInstructions[step];

            var w = digits[step];

            var test = (z % 26) + instruction.addX == w;
            if (instruction.divZ == 26 && test)
            {
                z = z / instruction.divZ;
                step++;
                continue;
            }
            if (instruction.divZ == 1 && !test)
            {
                z = 26 * (z / instruction.divZ) + w + instruction.addY;
                step++;
                continue;
            }

            if (digits[step] < 9)
            {
                digits[step]++;
            }
            if (digits[step] == 9)
            {
                var b = step - 1;
                while (digits[b] == 9 && b > 0)
                {
                    b--;
                }
                digits[b]++;
                for (int a = b + 1; a <= step; a++)
                {
                    digits[a] = 1;
                }
                step = 0;
                z = 0;
                continue;
            }
        }
        return string.Join("", digits);
    }
}