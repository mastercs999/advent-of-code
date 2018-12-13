using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2018._13
{
    public class Cart
    {
        public Direction Direction { get; set; }
        public Track Track { get; set; }
        public Turn NextTurn { get; set; }

        public Cart(Direction direction, Track track)
        {
            Direction = direction;
            Track = track;
            NextTurn = Turn.Left;
        }
    }
}
