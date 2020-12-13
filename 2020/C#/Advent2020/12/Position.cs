using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2020._12
{
    public record Position
    {
        public char Direction { get; init; }
        public int X { get; init; }
        public int Y { get; init; }
    }
}
