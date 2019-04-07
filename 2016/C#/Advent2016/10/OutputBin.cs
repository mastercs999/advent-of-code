using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2016._10
{
    public class OutputBin
    {
        public int Id { get; set; }
        public List<int> Chips { get; set; }

        public OutputBin(int id)
        {
            Id = id;
            Chips = new List<int>();
        }

        public void Receive(int chip)
        {
            Chips.Add(chip);
        }
    }
}
