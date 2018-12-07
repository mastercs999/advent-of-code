using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2018._07
{
    public class Elf
    {
        public char? Step { get; set; }
        public int SecondsLeft { get; set; }
        public bool IsWorking => Step.HasValue;
    }
}
