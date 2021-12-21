using System.Text;

class Day20
{
    private string _algorithm;
    private List<string> _image;
    private int _width;
    private int _height;

    public Day20()
    {
        var input = File.ReadAllLines("input20.txt");

        _algorithm = input[0];

        _image = input.Skip(2).ToList();
        _width = _image[0].Length;
        _height = _image.Count;
        ExpandImage(0);

    }

    private void ExpandImage(int iteration)
    {
        _image.Insert(0, GetDarkRow(iteration));
        _image.Insert(0, GetDarkRow(iteration));
        _image.Insert(0, GetDarkRow(iteration));
        _image.Add(GetDarkRow(iteration));
        _image.Add(GetDarkRow(iteration));
        _image.Add(GetDarkRow(iteration));

        var colPadding = iteration % 2 == 0 ? "..." : "###";

        _image = _image.Select(s => $"{colPadding}{s}{colPadding}").ToList();

        _width = _image[0].Length;
        _height = _image.Count;
    }

    public long Run()
    {
        for (int i = 1; i < 51; i++)
        {
            var nextImage = new List<string>();

            for (int row = 1; row < _height - 1; row++)
            {
                var nextRow = new StringBuilder();
                for (int col = 1; col < _width - 1; col++)
                {
                    var binary = $"{_image[row - 1].Substring(col - 1, 3)}{_image[row].Substring(col - 1, 3)}{_image[row + 1].Substring(col - 1, 3)}";
                    var algoIndex = ToDecimal(binary);
                    nextRow.Append(_algorithm[algoIndex]);
                }
                nextImage.Add(nextRow.ToString());
            }
            _image = nextImage;
            _width = _image[0].Length;
            _height = _image.Count;
            ExpandImage(i);
        }

        return _image.SelectMany(s => s).LongCount(c => c == '#');
    }

    private int ToDecimal(string binaryRep)
    {
        var exp = binaryRep.Length - 1;
        var decimalValue = 0;
        foreach (var bit in binaryRep)
        {
            if (bit == '#')
            {
                decimalValue += (int)Math.Pow(2, exp);
            }
            exp--;
        }
        return decimalValue;
    }
    private string GetDarkRow(int iteration)
    {
        return new string(iteration % 2 == 0 ? '.' : '#', _width);
    }
}