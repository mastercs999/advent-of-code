using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2015._07
{
    public class Signal
    {
        public string Id { get; set; }
        public ushort? Value { get; set; }

        public Signal(string idOrValue)
        {
            if (ushort.TryParse(idOrValue, out ushort value))
                Value = value;
            else
                Id = idOrValue;
        }
    }
}
