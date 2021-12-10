class Day10
{
    private string[] _input;

    public Day10()
    {
        _input = File.ReadAllLines("input10.txt");
    }

    public long Run()
    {
        long score = 0;
        foreach (var item in _input)
        {
            var stack = new Stack<char>();
            var corrupted = false;
            foreach (var c in item)
            {
                switch (c)
                {
                    case '(':
                    case '[':
                    case '{':
                    case '<':
                        stack.Push(c);
                        break;
                    case ')' when stack.Peek() == '(':
                        stack.Pop();
                        break;
                    case ']' when stack.Peek() == '[':
                        stack.Pop();
                        break;
                    case '}' when stack.Peek() == '{':
                        stack.Pop();
                        break;
                    case '>' when stack.Peek() == '<':
                        stack.Pop();
                        break;
                    case ')' when stack.Peek() != '(':
                        stack.Push(c);
                        corrupted = true;
                        break;
                    case ']' when stack.Peek() != '[':
                        stack.Push(c);
                        corrupted = true;
                        break;
                    case '}' when stack.Peek() != '{':
                        stack.Push(c);
                        corrupted = true;
                        break;
                    case '>' when stack.Peek() != '<':
                        corrupted = true;
                        stack.Push(c);
                        break;
                }
                if (corrupted)
                {
                    break;
                }
            }
            if (corrupted)
            {
                switch (stack.Peek())
                {
                    case ')':
                        score += 3;
                        break;
                    case ']':
                        score += 57;
                        break;
                    case '}':
                        score += 1197;
                        break;
                    case '>':
                        score += 25137;
                        break;
                }
            }
        }

        return score;
    }

    public long RunB()
    {
        var rowScores = new List<long>();
        foreach (var item in _input)
        {
            var stack = new Stack<char>();
            var corrupted = false;
            foreach (var c in item)
            {
                switch (c)
                {
                    case '(':
                    case '[':
                    case '{':
                    case '<':
                        stack.Push(c);
                        break;
                    case ')' when stack.Peek() == '(':
                        stack.Pop();
                        break;
                    case ']' when stack.Peek() == '[':
                        stack.Pop();
                        break;
                    case '}' when stack.Peek() == '{':
                        stack.Pop();
                        break;
                    case '>' when stack.Peek() == '<':
                        stack.Pop();
                        break;
                    case ')' when stack.Peek() != '(':
                        stack.Push(c);
                        corrupted = true;
                        break;
                    case ']' when stack.Peek() != '[':
                        stack.Push(c);
                        corrupted = true;
                        break;
                    case '}' when stack.Peek() != '{':
                        stack.Push(c);
                        corrupted = true;
                        break;
                    case '>' when stack.Peek() != '<':
                        corrupted = true;
                        stack.Push(c);
                        break;
                }
                if (corrupted)
                {
                    break;
                }
            }

            if (!corrupted)
            {
                long rowScore = 0;
                while (stack.Any())
                {
                    var curr = stack.Pop();
                    rowScore *= 5;
                    rowScore += curr switch
                    {
                        '(' => 1,
                        '[' => 2,
                        '{' => 3,
                        '<' => 4
                    };
                }
                rowScores.Add(rowScore);
            }
        }

        var sorted = rowScores.OrderBy(s => s).ToArray();

        return sorted[sorted.Length / 2];
    }
}