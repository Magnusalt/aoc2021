class Day18
{
    private string[] _input;

    public Day18()
    {
        _input = File.ReadAllLines("input18.txt");
    }

    public long Run()
    {
        var current = Parse(_input.First());
        Reduce(current);
        foreach (var number in _input.Skip(1))
        {
            var next = Parse(number);
            Reduce(next);
            current = Add(current, next);
            Reduce(current);
        }


        return Magnitude(current);
    }

    public long RunB()
    {
        var pairs = _input.SelectMany(i => _input, (i1, i2) => (i1, i2)).Where(t => t.i1 != t.i2);

        long maxMagnitude = 0;

        foreach (var item in pairs)
        {
            var a = Parse(item.i1);
            var b = Parse(item.i2);
            Reduce(a);
            Reduce(b);
            var result = Add(a, b);
            Reduce(result);

            var magnitude = Magnitude(result);
            if (magnitude > maxMagnitude)
            {
                maxMagnitude = magnitude;
            }

        }
        return maxMagnitude;
    }


    private long Magnitude(PairNode root)
    {
        var x = root.X.HasValue ? 3 * root.X.Value : 3 * Magnitude(root.Left);
        var y = root.Y.HasValue ? 2 * root.Y.Value : 2 * Magnitude(root.Right);
        return x + y;
    }

    private void Reduce(PairNode node)
    {
        var instructions = new Stack<string>();
        instructions.Push("s");
        instructions.Push("e");

        while (instructions.Any())
        {
            var currentInstruction = instructions.Pop();
            if (currentInstruction == "e")
            {
                var explodable = FindFirstToExplode(node);
                if (explodable != null)
                {
                    Explode(explodable);
                    instructions.Push("e");
                }
            }
            if (currentInstruction == "s")
            {
                var splittable = FindFirstToSplit(node);
                if (splittable != null)
                {
                    Split(splittable);
                    instructions.Push("s");
                    instructions.Push("e");
                }
            }


        }

    }

    private PairNode Parse(string snailNumber)
    {
        var root = new PairNode();
        var current = root;
        var index = 1;
        bool addLeft = true;
        var rootPair = snailNumber.Skip(1);
        var skipCycles = 0;
        foreach (var c in rootPair)
        {
            if (skipCycles > 0)
            {
                index++;
                skipCycles--;
                continue;
            }
            switch (c)
            {
                case '[':
                    if (addLeft)
                    {
                        current.Left = new PairNode();
                        current.Left.Parent = current;
                        current = current.Left;
                    }
                    else
                    {
                        current.Right = new PairNode();
                        current.Right.Parent = current;
                        current = current.Right;
                        addLeft = true;
                    }
                    break;
                case var n when char.IsNumber(c):
                    var endOfNumber = new int[] { snailNumber.Substring(index).IndexOf(','), snailNumber.Substring(index).IndexOf(']') }.Where(n => n > 0).Min();
                    var v = int.Parse(snailNumber.Substring(index, endOfNumber));
                    skipCycles = endOfNumber - 1;
                    if (addLeft)
                    {
                        current.X = v;
                    }
                    else
                    {
                        current.Y = v;
                        addLeft = true;
                    }
                    break;
                case ']':
                    current = current.Parent;
                    break;
                case ',':
                    addLeft = false;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            index++;
        }

        return root;
    }

    private void Split(PairNode node)
    {
        if (node.X > 9)
        {
            var left = new PairNode();
            left.X = (int)Math.Floor((double)node.X / 2);
            left.Y = (int)Math.Ceiling((double)node.X / 2);
            node.X = null;
            node.Left = left;
            left.Parent = node;
            return;
        }
        if (node.Y > 9)
        {
            var right = new PairNode();
            right.X = (int)Math.Floor((double)node.Y / 2);
            right.Y = (int)Math.Ceiling((double)node.Y / 2);
            node.Y = null;
            node.Right = right;
            right.Parent = node;
        }
    }

    private PairNode Add(PairNode a, PairNode b)
    {
        var root = new PairNode();
        root.Left = a;
        a.Parent = root;
        root.Right = b;
        b.Parent = root;

        return root;
    }

    private PairNode FindFirstToExplode(PairNode root)
    {
        var stack = new Stack<PairNode>();
        stack.Push(root);
        while (stack.Any())
        {
            var current = stack.Pop();
            if (current.Parent?.Parent?.Parent?.Parent == root)
            {
                return current;
            }
            if (current.Right != null)
            {
                stack.Push(current.Right);
            }
            if (current.Left != null)
            {
                stack.Push(current.Left);
            }
        }
        return null;
    }

    private PairNode FindFirstToSplit(PairNode root)
    {
        var stack = new Stack<PairNode>();
        stack.Push(root);
        while (stack.Any())
        {
            var current = stack.Pop();
            if (current.X.HasValue && current.X > 9)
            {
                return current;
            }
            if (current.Y.HasValue && current.Y > 9)
            {
                if (current.Left != null)
                {
                    var moreLeft = FindFirstToSplit(current.Left);
                    return moreLeft ?? current;
                }
                return current;
            }
            if (current.Right != null)
            {
                stack.Push(current.Right);
            }
            if (current.Left != null)
            {
                stack.Push(current.Left);
            }
        }
        return null;
    }

    private void Explode(PairNode node)
    {
        if (node.Left != null || node.Right != null)
        {
            throw new Exception("hope this not happens");
        }

        SendYRight(node, node.Parent, node.Y.Value);
        SendXLeft(node, node.Parent, node.X.Value);

        if (node.Parent.Left == node)
        {
            node.Parent.Left = null;
            node.Parent.X = 0;
        }
        if (node.Parent.Right == node)
        {
            node.Parent.Right = null;
            node.Parent.Y = 0;
        }
        node.Parent = null;
        node = null;

    }

    private void SendYRight(PairNode prev, PairNode current, int y)
    {
        if (current.Left == prev && current.Y.HasValue)
        {
            current.Y += y;
            return;
        }
        if (current.Parent == prev && current.X.HasValue)
        {
            current.X += y;
            return;
        }
        if (current.Right != null && current.Right != prev && current.Parent != prev)
        {
            SendYRight(current, current.Right, y);
        }
        else if (current.Parent != null && current.Parent != prev)
        {
            SendYRight(current, current.Parent, y);
        }
        else if (current.Parent == prev && !current.X.HasValue && current.Left != null)
        {
            SendYRight(current, current.Left, y);
        }
    }
    private void SendXLeft(PairNode prev, PairNode current, int x)
    {
        if (current.Right == prev && current.X.HasValue)
        {
            current.X += x;
            return;
        }
        if (current.Parent == prev && current.Y.HasValue)
        {
            current.Y += x;
            return;
        }
        if (current.Left != null && current.Left != prev && current.Parent != prev)
        {
            SendXLeft(current, current.Left, x);
        }
        else if (current.Parent != null && current.Parent != prev)
        {
            SendXLeft(current, current.Parent, x);
        }
        else if (current.Parent == prev && !current.Y.HasValue && current.Right != null)
        {
            SendXLeft(current, current.Right, x);
        }
    }
}


class PairNode
{
    public int? X { get; set; }
    public int? Y { get; set; }
    public PairNode? Parent { get; set; }
    public PairNode? Left { get; set; }
    public PairNode? Right { get; set; }
}