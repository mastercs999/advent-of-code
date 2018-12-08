using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2018._08
{
    public class Node
    {
        public List<int> Metadata { get; set; }
        public List<Node> ChildNodes { get; set; }

        public int Length { get; private set; }
        public int Value { get; private set; }

        public Node(List<int> metadata, List<Node> childNodes)
        {
            Metadata = metadata;
            ChildNodes = childNodes;
            Length = 2 + ChildNodes.Sum(x => x.Length) + Metadata.Count;
            Value = !ChildNodes.Any() ? Metadata.Sum() : Metadata.Sum(x => x > 0 && x <= ChildNodes.Count ? ChildNodes[x - 1].Value : 0);
        }
    }
}
