using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Advent2015._25
{
    public static class Challenge25
    {
        public static void Run()
        {
            // Load coords
            Point coords = LoadCoords(Path.Combine("25", "input"));

            // Calculate what number corresponds to given coords
            int targetNumber = 1 - coords.Y + (coords.X + coords.Y) * (coords.X + coords.Y - 1) / 2;

            // Do given calculation
            ulong code = 20151125;
            for (int i = 1; i < targetNumber; ++i)
                code = (code * 252533) % 33554393;

            Console.WriteLine(code);
        }

        private static Point LoadCoords(string path)
        {
            Match match = Regex.Match(File.ReadAllText(path), @".*row (\d+).*column (\d+)");

            return new Point(int.Parse(match.Groups[2].Value), int.Parse(match.Groups[1].Value));
        }
    }
}
