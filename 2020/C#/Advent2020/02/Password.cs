using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2020._02
{
    public record Password
    {
        public int MinNumber { get; init; }
        public int MaxNumber { get; init; }
        public char Character { get; init; }
        public string Text { get; init; }
    }
}
