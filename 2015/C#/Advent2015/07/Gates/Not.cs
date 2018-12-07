using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2015._07.Gates
{
    public class Not : Gate
    {
        public Not(Signal input, Signal output)
        {
            InputSignals = new Signal[] { input };
            OutputSignal = output;
        }

        protected override ushort Propagate(params ushort[] signals)
        {
            return (ushort)~signals.Single();
        }
    }
}
