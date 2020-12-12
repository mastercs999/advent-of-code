using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2020._08
{
    public record Instruction
    {
        public string Name { get; init; }
        public int Value { get; init; }
    }
}
