using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2018._10
{
    public class Star
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int VelocityX { get; set; }
        public int VelocityY { get; set; }

        public void Move()
        {
            X += VelocityX;
            Y += VelocityY;
        }
    }
}
