using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2018._23
{
    public class Nanobot
    {
        public Point Point { get; }
        public int Radius { get; }

        public Nanobot(Point point, int radius)
        {
            Point = point;
            Radius = radius;
        }
    }
}
