class Day3
{
    private string[] _input;

    public Day3()
    {
        _input = File.ReadAllLines("input3.txt");
    }

    public ulong RunA()
    {
        var wordLength = _input.First().Length;
        var threshold = _input.Length / 2;
        var gamma = new byte[wordLength];
        var epsilon = new byte[wordLength];

        for (int i = 0; i < wordLength; i++)
        {
            var ones = 0;
            foreach (var item in _input)
            {
                if (item[i] == '1')
                {
                    ones++;
                }
            }
            gamma[i] = ones > threshold ? (byte)1 : (byte)0;
        }

        ulong gammaint = 0;

        var exp = wordLength - 1;
        foreach (var bit in gamma)
        {
            if (bit == 1)
            {
                gammaint += (ulong)Math.Pow(2, exp);
            }
            exp--;
        }


        for (int i = 0; i < wordLength; i++)
        {
            var zeroes = 0;
            foreach (var item in _input)
            {
                if (item[i] == '0')
                {
                    zeroes++;
                }
            }
            epsilon[i] = zeroes < threshold ? (byte)0 : (byte)1;
        }

        ulong epsilonDecimal = 0;

        exp = wordLength - 1;
        foreach (var bit in epsilon)
        {
            if (bit == 1)
            {
                epsilonDecimal += (ulong)Math.Pow(2, exp);
            }
            exp--;
        }


        return gammaint * epsilonDecimal;
    }

    public ulong RunB()
    {
        var wordLength = _input.First().Length;

        var oxy = _input;
        for (int i = 0; i < wordLength; i++)
        {
            oxy = oxy.GroupBy(b => b[i])
             .Select(g => new { Key = g.Key, Words = g.AsEnumerable(), Count = g.Count() })
             .OrderBy(an => an.Count).ThenBy(a => a.Key)
             .Select(o => o.Words)
             .Last().ToArray();
            ;
        }

        var co2 = _input;
        for (int i = 0; i < wordLength; i++)
        {
            co2 = co2.GroupBy(b => b[i])
             .Select(g => new { Key = g.Key, Words = g.AsEnumerable(), Count = g.Count() })
             .OrderByDescending(an => an.Count).ThenByDescending(a => a.Key)
             .Select(o => o.Words)
             .Last().ToArray();
            ;
        }

        var exp = wordLength - 1;
        ulong oxyValue = 0;
        foreach (var bit in oxy.Single())
        {
            if (bit == '1')
            {
                oxyValue += (ulong)Math.Pow(2, exp);
            }
            exp--;
        }

        exp = wordLength - 1;
        ulong co2Value = 0;
        foreach (var bit in co2.Single())
        {
            if (bit == '1')
            {
                co2Value += (ulong)Math.Pow(2, exp);
            }
            exp--;
        }

        return oxyValue * co2Value;
    }
}