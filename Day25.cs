class Day25
{
    public long Run()
    {
        var input = File.ReadAllLines("input25.txt");

        var current = input.Select(s => s.ToCharArray()).ToList();
        var next = current.Select(c => new char[c.Length]).ToList();

        var height = input.Length;
        var width = input.First().Length;
        var steps = 0;
        while (true)
        {
            var rowIndex = 0;
            foreach (var row in current)
            {
                var nextRow = next[rowIndex];
                for (var c = width - 1; c >= 0; c--)
                {
                    switch (row[c])
                    {
                        case '>' when c == width - 1 && row[0] == '.':
                            nextRow[0] = '>';
                            nextRow[c] = '.';
                            break;
                        case '>' when c < width - 1 && row[c + 1] == '.':
                            nextRow[c + 1] = '>';
                            nextRow[c] = '.';
                            break;
                        default:
                            if (nextRow[c] == default(char))
                                nextRow[c] = row[c];
                            break;
                    }
                }
                rowIndex++;
            }

            var moveDown = next.Select(c => new char[c.Length]).ToList();

            for (var ri = height - 1; ri >= 0; ri--)
            {
                var row = next[ri];
                var moveDownRow = moveDown[ri];
                var rowbelow = ri < height - 1 ? next[ri + 1] : next[0];
                var rowbelowCurrent = ri < height - 1 ? current[ri + 1] : current[0];
                var charIndex = 0;
                foreach (var c in row)
                {
                    if (c == 'v')
                    {
                        if (rowbelow[charIndex] == '.' && rowbelowCurrent[charIndex] == '.')
                        {
                            if (ri < height - 1)
                            {
                                moveDown[ri + 1][charIndex] = 'v';
                            }
                            else
                            {
                                moveDown[0][charIndex] = 'v';
                            }
                            moveDown[ri][charIndex] = '.';
                            charIndex++;
                            continue;
                        }
                        if (rowbelow[charIndex] == '.' && rowbelowCurrent[charIndex] == 'v')
                        {
                            charIndex++;
                            continue;
                        }
                        if (rowbelow[charIndex] == '.' && rowbelowCurrent[charIndex] == '>')
                        {
                            if (ri < height - 1)
                            {
                                moveDown[ri + 1][charIndex] = 'v';
                            }
                            else
                            {
                                moveDown[0][charIndex] = 'v';
                            }
                            moveDown[ri][charIndex] = '.';
                            charIndex++;
                            continue;
                        }
                        moveDown[ri][charIndex] = c;

                    }
                    else
                    {
                        if (moveDown[ri][charIndex] == default(char))
                            moveDown[ri][charIndex] = c;
                    }
                    charIndex++;
                }
            }

            var nextasstring = moveDown.Select(r => new string(r));
            steps++;

            var isStatusQuo = moveDown.Zip(current, (m, c) => new string(m) == new string(c)).All(e => e);
            if (isStatusQuo)
            {
                break;
            }

            current = moveDown.ToList();
            next = next.Select(c => new char[c.Length]).ToList();
            moveDown = moveDown.Select(c => new char[c.Length]).ToList();

        }
        var asstring = current.Select(r => new string(r));

        return steps;
    }
}