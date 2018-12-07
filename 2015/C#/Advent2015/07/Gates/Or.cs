using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2015._07.Gates
{
    public class Or : Gate
    {
        public Or(Signal input1, Signal input2, Signal output)
        {
            InputSignals = new Signal[] { input1, input2 };
            OutputSignal = output;
        }

        protected override ushort Propagate(params ushort[] signals)
        {
            return signals.Aggregate((x, y) => (ushort)(x | y));
        }
    }
}
