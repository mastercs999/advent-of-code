using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2015._06
{
    public class Command
    {
        public CommandType CommandType { get; set; }
        public Point TopLeft { get; set; }
        public Point BottomRight { get; set; }

        public void Apply(int[,] field, Func<CommandType, int, int> actionFunc)
        {
            for (int x = TopLeft.X; x <= BottomRight.X; ++x)
                for (int y = TopLeft.Y; y <= BottomRight.Y; ++y)
                    field[y, x] = actionFunc(CommandType, field[y, x]);
        }
    }
}
