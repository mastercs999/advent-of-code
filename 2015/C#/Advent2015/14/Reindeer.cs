using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2015._14
{
    public class Reindeer
    {
        public string Name { get; set; }
        public int Speed { get; set; }
        public int FlyDuration { get; set; }
        public int RestDuration { get; set; }

        public IEnumerable<int> Distances()
        {
            while (true)
            {
                for (int i = 0; i < FlyDuration; ++i)
                    yield return Speed;
                for (int i = 0; i < RestDuration; ++i)
                    yield return 0;
            }
        }
    }
}
