using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2015._02
{
    public class Present
    {
        public int Length { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }

        public int[] BySize { get; private set; }

        public int Area { get; private set; }
        public int SmallestArea { get; private set; }
        
        public int RibbonWrap { get; private set; }
        public int RibbonBow { get; private set; }

        public Present(int length, int width, int height)
        {
            Length = length;
            Width = width;
            Height = height;

            BySize = new int[] { Length, Width, Height }.OrderBy(x => x).ToArray();

            Area = 2 * Length * Width + 2 * Width * Height + 2 * Length * Height;
            SmallestArea = BySize[0] * BySize[1];

            RibbonWrap = 2 * BySize[0] + 2 * BySize[1];
            RibbonBow = Length * Width * Height;
        }
    }
}
