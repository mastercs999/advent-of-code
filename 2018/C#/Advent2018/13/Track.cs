using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2018._13
{
    public class Track
    {
        public int X { get; set; }
        public int Y { get; set; }
        public TrackType TrackType { get; set; }

        public Track(int x, int y, TrackType trackType)
        {
            X = x;
            Y = y;
            TrackType = trackType;
        }
    }
}
