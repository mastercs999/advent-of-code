using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2018._03
{
    public class Claim
    {
        public int Id { get; set; }
        public Rectangle Rectangle { get; set; }

        public Claim(int id, int x, int y, int width, int height)
        {
            Id = id;
            Rectangle = new Rectangle(x, y, width, height);
        }
    }
}
