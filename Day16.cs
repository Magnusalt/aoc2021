using System.Linq;
using System.Text;

class Day16
{
    private string _binary;

    public Day16()
    {
        var input = File.ReadAllText("input16.txt");
        _binary = BuildBinary(input);
    }

    public long Run()
    {
        long sum = 0;
        TraversePackage(_binary, ref sum);
        return sum;
    }

    public long RunB()
    {
        var root = TraversePackageB(_binary, 0);

        return root.Evaluate();
    }

    private Node TraversePackageB(string package, int index)
    {
        long packageIndex = 0;
        var version = package.Substring((int)packageIndex, 3);
        packageIndex += 3;
        var typeId = package.Substring((int)packageIndex, 3);
        packageIndex += 3;
        var versionValue = FromBinaryToDecimal(version);
        var typeIdValue = FromBinaryToDecimal(typeId);

        if (typeIdValue == 4)
        {
            var payload = package.Substring((int)packageIndex, 5);
            packageIndex += 5;
            var sb = new StringBuilder();
            sb.Append(payload.Substring(1));
            while (payload[0] == '1')
            {
                payload = package.Substring((int)packageIndex, 5);
                sb.Append(payload.Substring(1));
                packageIndex += 5;
            }
            return new LiteralNode(FromBinaryToDecimal(sb.ToString()), (int)packageIndex);
        }

        Node node = typeIdValue switch
        {
            0 => new SumNode(index),
            1 => new ProductNode(index),
            2 => new MinNode(index),
            3 => new MaxNode(index),
            5 => new GreaterNode(index),
            6 => new LessNode(index),
            7 => new EqualNode(index),
            _ => throw new ArgumentOutOfRangeException()
        };


        if (package[(int)packageIndex] == '0')
        {
            packageIndex++;
            long subPackageTotalLength = FromBinaryToDecimal(package.Substring((int)packageIndex, 15));
            packageIndex += 15;

            long subPackageIndex = 0;
            while (subPackageIndex < subPackageTotalLength)
            {
                var subPackageNode = TraversePackageB(package.Substring((int)(packageIndex + subPackageIndex), (int)(subPackageTotalLength - subPackageIndex)), (int)(packageIndex + subPackageIndex));
                node.AddChildNode(subPackageNode);
                subPackageIndex += subPackageNode.Index;
            }
            packageIndex += subPackageTotalLength;
            node.Index = packageIndex;
            return node;
        }
        if (package[(int)packageIndex] == '1')
        {
            packageIndex++;
            var numberOfSubPacakges = FromBinaryToDecimal(package.Substring((int)packageIndex, 11));
            packageIndex += 11;

            for (int i = 0; i < numberOfSubPacakges; i++)
            {
                var subPackageNode = TraversePackageB(package.Substring((int)packageIndex), (int)packageIndex);
                node.AddChildNode(subPackageNode);
                packageIndex += subPackageNode.Index;
                node.Index = packageIndex;
            }
        }

        return node;
    }

    private int TraversePackage(string package, ref long versionSum)
    {
        long packageIndex = 0;
        var version = package.Substring((int)packageIndex, 3);
        packageIndex += 3;
        var typeId = package.Substring((int)packageIndex, 3);
        packageIndex += 3;
        long versionValue = FromBinaryToDecimal(version);
        long typeIdValue = FromBinaryToDecimal(typeId);

        versionSum += versionValue;

        if (typeIdValue == 4)
        {
            var payload = package.Substring((int)packageIndex, 5);
            packageIndex += 5;
            while (payload[0] == '1')
            {
                payload = package.Substring((int)packageIndex, 5);
                packageIndex += 5;
            }
            return (int)packageIndex;
        }

        if (package[(int)packageIndex] == '0')
        {
            packageIndex++;
            long subPackageTotalLength = FromBinaryToDecimal(package.Substring((int)packageIndex, 15));
            packageIndex += 15;

            long subPackageIndex = 0;
            while (subPackageIndex < subPackageTotalLength)
            {
                subPackageIndex += TraversePackage(package.Substring((int)(packageIndex + subPackageIndex), (int)(subPackageTotalLength - subPackageIndex)), ref versionSum);
            }
            packageIndex += subPackageTotalLength;
            return (int)packageIndex;
        }
        if (package[(int)packageIndex] == '1')
        {
            packageIndex++;
            var numberOfSubPacakges = FromBinaryToDecimal(package.Substring((int)packageIndex, 11));
            packageIndex += 11;

            for (int i = 0; i < numberOfSubPacakges; i++)
            {
                packageIndex += TraversePackage(package.Substring((int)packageIndex), ref versionSum);
            }
        }

        return (int)packageIndex;
    }

    private long FromBinaryToDecimal(string binary)
    {
        if (binary.Length == 3)
        {
            binary = "0" + binary;
        }

        var exp = binary.Length - 1;
        long decimalValue = 0;
        foreach (var bit in binary)
        {
            if (bit == '1')
            {
                decimalValue += (long)Math.Pow(2, exp);
            }
            exp--;
        }
        return decimalValue;
    }

    private string BuildBinary(string hex)
    {
        var sb = new StringBuilder();
        foreach (var c in hex)
        {
            sb.Append(FromCharToBinary(c));
        }
        return sb.ToString();
    }

    private string FromCharToBinary(char c)
    {
        return c switch
        {
            '0' => "0000",
            '1' => "0001",
            '2' => "0010",
            '3' => "0011",
            '4' => "0100",
            '5' => "0101",
            '6' => "0110",
            '7' => "0111",
            '8' => "1000",
            '9' => "1001",
            'A' => "1010",
            'B' => "1011",
            'C' => "1100",
            'D' => "1101",
            'E' => "1110",
            'F' => "1111",
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    abstract class Node
    {
        public Node(int index)
        {
            ChildNodes = new List<Node>();
            Index = index;
        }
        public abstract long Evaluate();
        public List<Node> ChildNodes { get; }
        public long Index { get; set; }

        public void AddChildNode(Node node)
        {
            ChildNodes.Add(node);
        }
    }

    class LiteralNode : Node
    {
        private readonly long value;

        public LiteralNode(long value, int index) : base(index)
        {
            this.value = value;
        }
        public override long Evaluate()
        {
            return value;
        }
    }

    class SumNode : Node
    {
        public SumNode(int index) : base(index)
        {
        }

        public override long Evaluate()
        {
            return ChildNodes.Select(n => n.Evaluate()).Sum();
        }
    }
    class ProductNode : Node
    {
        public ProductNode(int index) : base(index)
        {
        }

        public override long Evaluate()
        {
            return ChildNodes.Select(n => n.Evaluate()).Aggregate((long)1, (acc, i) => acc *= i);
        }
    }
    class MinNode : Node
    {
        public MinNode(int index) : base(index)
        {
        }

        public override long Evaluate()
        {
            return ChildNodes.Select(n => n.Evaluate()).Min();
        }
    }
    class MaxNode : Node
    {
        public MaxNode(int index) : base(index)
        {
        }

        public override long Evaluate()
        {
            return ChildNodes.Select(n => n.Evaluate()).Max();
        }
    }

    class GreaterNode : Node
    {
        public GreaterNode(int index) : base(index)
        {
        }

        public override long Evaluate()
        {
            return ChildNodes[0].Evaluate() > ChildNodes[1].Evaluate() ? 1 : 0;
        }
    }
    class LessNode : Node
    {
        public LessNode(int index) : base(index)
        {
        }

        public override long Evaluate()
        {
            return ChildNodes[0].Evaluate() < ChildNodes[1].Evaluate() ? 1 : 0;
        }
    }
    class EqualNode : Node
    {
        public EqualNode(int index) : base(index)
        {
        }

        public override long Evaluate()
        {
            return ChildNodes[0].Evaluate() == ChildNodes[1].Evaluate() ? 1 : 0;
        }
    }
}