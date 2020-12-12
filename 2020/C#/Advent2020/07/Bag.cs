using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2020._07
{
    public class Bag
    {
        public string Name { get; init; }
        public Dictionary<string, int> BagsInside { get; init; }
    }
}
