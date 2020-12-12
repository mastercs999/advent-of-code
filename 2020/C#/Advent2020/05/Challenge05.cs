using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2020._05
{
    public static class Challenge05
    {
        public static void Run()
        {
            // Load input
            string[] codes = File.ReadAllLines(Path.Combine("05", "input.txt"));

            // Task 1
            int[] seatIds = codes.Select(x => DecodeRow(x) * 8 + DecodeColumn(x)).OrderBy(x => x).ToArray();
            int topSeatID = seatIds.Max();
            Console.WriteLine(topSeatID);

            // Task 2
            int missingSeatId = seatIds.Zip(seatIds[1..], (x, y) => (x, y)).Where(x => x.y - x.x == 2).Select(x => x.x + 1).Single();
            Console.WriteLine(missingSeatId);
        }

        private static int DecodeRow(string code)
        {
            return Convert.ToInt32(code[0..7].Replace('F', '0').Replace('B', '1'), 2);
        }

        private static int DecodeColumn(string code)
        {
            return Convert.ToInt32(code[7..10].Replace('L', '0').Replace('R', '1'), 2);
        }
    }
}
