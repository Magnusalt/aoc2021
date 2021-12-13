class Day13
{
    private List<(string, int)> _folds;
    private int _maxX;
    private int _maxY;
    private char[,] _matrix;
    private List<(int, int)> _dots;

    public Day13()
    {
        var input = File.ReadAllLines("input13.txt");
        _dots = input.Where(i => i.Contains(",")).Select(dot => dot.Split(',')).Select(dot => (int.Parse(dot[0]), int.Parse(dot[1]))).ToList();
        var fold = "fold along ";
        _folds = input.Where(i => i.Contains(fold)).Select(f => f.Substring(fold.Length)).Select(f => f.Split('=')).Select(f => (f[0], int.Parse(f[1]))).ToList();
        _maxX = _dots.Max(d => d.Item1) + 1;
        _maxY = _dots.Max(d => d.Item2) + 1;

        _matrix = new char[_maxX, _maxY];

        foreach (var dot in _dots)
        {
            _matrix[dot.Item1, dot.Item2] = '#';
        }
    }

    public long Run()
    {
        var folds = _folds.Take(1);

        foreach (var fold in folds)
        {
            if (fold.Item1 == "y")
            {
                var upper = new char[_maxX, fold.Item2];
                var upperDots = _dots.Where(d => d.Item2 < fold.Item2);
                foreach (var dot in upperDots)
                {
                    upper[dot.Item1, dot.Item2] = '#';
                }
                var lowerDots = _dots.Where(d => d.Item2 > fold.Item2);
                var translate = lowerDots.Select(d => (d.Item1, _maxY - d.Item2 - 1));
                foreach (var dot in translate)
                {
                    upper[dot.Item1, dot.Item2] = '#';
                }
                _matrix = upper;
                _maxY = fold.Item2;
            }
            if (fold.Item1 == "x")
            {
                var left = new char[fold.Item2, _maxY];
                var leftDots = _dots.Where(d => d.Item1 < fold.Item2);
                foreach (var dot in leftDots)
                {
                    left[dot.Item1, dot.Item2] = '#';
                }
                var rightDots = _dots.Where(d => d.Item1 > fold.Item2);
                var translate = rightDots.Select(d => (_maxX - d.Item1 - 1, d.Item2));
                foreach (var dot in translate)
                {
                    left[dot.Item1, dot.Item2] = '#';
                }
                _matrix = left;
                _maxX = fold.Item2;
            }
        }
        long count = 0;
        for (int i = 0; i < _maxY; i++)
        {
            for (int j = 0; j < _maxX; j++)
            {
                if (_matrix[j, i] == '#')
                {
                    count++;
                }
            }
        }

        return count;
    }
    public long RunB()
    {
        foreach (var fold in _folds)
        {
            if (fold.Item1 == "y")
            {
                var upper = new char[_maxX, fold.Item2];
                var upperDots = _dots.Where(d => d.Item2 < fold.Item2);
                foreach (var dot in upperDots)
                {
                    upper[dot.Item1, dot.Item2] = '#';
                }
                var lowerDots = _dots.Where(d => d.Item2 > fold.Item2);
                var translate = lowerDots.Select(d => (d.Item1, 2 * fold.Item2 - d.Item2)).ToList();
                foreach (var dot in translate)
                {

                    upper[dot.Item1, dot.Item2] = '#';


                }
                _dots = upperDots.Concat(translate).Distinct().ToList();
                _matrix = upper;
                _maxY = fold.Item2;
            }
            if (fold.Item1 == "x")
            {
                var left = new char[fold.Item2, _maxY];
                var leftDots = _dots.Where(d => d.Item1 < fold.Item2);
                foreach (var dot in leftDots)
                {
                    left[dot.Item1, dot.Item2] = '#';
                }
                var rightDots = _dots.Where(d => d.Item1 > fold.Item2);
                var translate = rightDots.Select(d => (2 * fold.Item2 - d.Item1, d.Item2)).ToList();
                foreach (var dot in translate)
                {
                    left[dot.Item1, dot.Item2] = '#';
                }
                _dots = leftDots.Concat(translate).Distinct().ToList();
                _matrix = left;
                _maxX = fold.Item2;
            }
        }
        for (int i = 0; i < _maxY; i++)
        {
            for (int j = 0; j < _maxX; j++)
            {
                if (_matrix[j, i] == '#')
                {
                    Console.Write('█');
                }
                else
                {
                    Console.Write(' ');
                }
            }
            System.Console.WriteLine();
        }

        return 0;
    }

}