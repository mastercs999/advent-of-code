using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2018._22
{
    public class Origin
    {
        public PathPoint From { get; set; }
        public Tool Tool { get; set; }
        public int Distance { get; set; }

        public Origin(PathPoint from, Tool tool, int distance)
        {
            From = from;
            Tool = tool;
            Distance = distance;
        }
    }
}
