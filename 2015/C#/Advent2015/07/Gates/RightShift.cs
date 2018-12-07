using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2015._07.Gates
{
    public class RightShift : Gate
    {
        private readonly int Length;

        public RightShift(Signal input, int length, Signal output)
        {
            Length = length;
            InputSignals = new Signal[] { input };
            OutputSignal = output;
        }

        protected override ushort Propagate(params ushort[] signals)
        {
            return (ushort)(signals.Single() >> Length);
        }
    }
}
