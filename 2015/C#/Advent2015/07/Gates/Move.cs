using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2015._07.Gates
{
    public class Move : Gate
    {
        public Move(Signal input, Signal output)
        {
            InputSignals = new Signal[] { input };
            OutputSignal = output;
        }

        protected override ushort Propagate(params ushort[] inputSignals)
        {
            return inputSignals.Single();
        }
    }
}
