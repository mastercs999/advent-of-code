using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2018._25
{
    public class Constellation
    {
        public List<Point> Points { get; set; }

        public bool CanJoin(Constellation constellation2)
        {
            return Points.CartesianJoin(constellation2.Points).Any(x => Utilities.Distance(x.Item1, x.Item2) <= 3);
        }

        public void Join(Constellation constellation2)
        {
            Points.AddRange(constellation2.Points);
        }
    }
}
