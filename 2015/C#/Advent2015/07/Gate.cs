using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2015._07
{
    public abstract class Gate
    {
        public Signal[] InputSignals { get; set; }
        public Signal OutputSignal { get; set; }

        public void Calculate()
        {
            OutputSignal.Value = Propagate(InputSignals.Select(x => x.Value.Value).ToArray());
        }

        protected abstract ushort Propagate(params ushort[] inputSignals);
    }
}
