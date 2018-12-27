using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2018._25
{
    public class Point
    {
        public int[] Dimensions { get; set; }

        public Point(int[] dimensions)
        {
            Dimensions = dimensions;
        }
    }
}
