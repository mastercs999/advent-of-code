using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2018._15
{
    public class PathPoint
    {
        public Node Node { get; set; } 
        public PathPoint From { get; set; }
        public int Distance { get; set; }
    }
}
