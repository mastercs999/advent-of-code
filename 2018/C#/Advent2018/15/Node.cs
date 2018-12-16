using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2018._15
{
    public class Node
    {
        public Point Location { get; set; }
        public NodeType NodeType { get; set; }

        public override string ToString() => NodeType.ToString();

        public IEnumerable<Node> Neighbours(Node[,] map)
        {
            foreach ((int dx, int dy) in new (int, int)[] { (0, -1), (-1, 0), (1, 0), (0, 1) })
            {
                int x = Location.X + dx;
                int y = Location.Y + dy;

                if (x >= 0 && x < map.GetLength(1) && y >= 0 && y < map.GetLength(0))
                    yield return map[y, x];
            }
        }
    }
}
