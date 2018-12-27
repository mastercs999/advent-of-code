using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2018._25
{
    public static class Utilities
    {
        public static int Distance(Point p1, Point p2)
        {
            return p1.Dimensions.Zip(p2.Dimensions, (d1, d2) => Math.Abs(d1 - d2)).Sum();
        }
    }
}
