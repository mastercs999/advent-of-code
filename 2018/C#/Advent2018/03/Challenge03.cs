using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2018._03
{
    public static class Challenge03
    {
        public static void Run()
        {
            // Load input
            List<Claim> claims = LoadClaims(Path.Combine("03", "input")).ToList();

            // Calculate fabric size
            int maxWidth = claims.Max(x => x.Rectangle.Left + x.Rectangle.Width);
            int maxHeight = claims.Max(x => x.Rectangle.Top + x.Rectangle.Height);
            int duplicates = 0;

            // Task 1
            // Calculate duplicates
            for (int h = 0; h < maxHeight; ++h)
                for (int w = 0; w < maxWidth; ++w)
                {
                    Rectangle rect = new Rectangle(w, h, 1, 1);

                    if (claims.Count(x => x.Rectangle.IntersectsWith(rect)) > 1)
                        ++duplicates;
                }

            Console.WriteLine(duplicates);

            // Task 2
            Console.WriteLine(claims.Single(x => claims.Where(y => x != y).All(y => !x.Rectangle.IntersectsWith(y.Rectangle))).Id);
        }

        public static IEnumerable<Claim> LoadClaims(string path)
        {
            foreach (string line in File.ReadLines(path))
            {
                string[] parts = line.Split(' ', ',', 'x', ':', '@', '#');

                yield return new Claim(int.Parse(parts[1]), int.Parse(parts[4]), int.Parse(parts[5]), int.Parse(parts[7]), int.Parse(parts[8]));
            }
        }
    }
}
