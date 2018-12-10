using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Advent2018._10
{
    public static class Challenge10
    {
        public static void Run()
        {
            // Load input
            List<Star> stars = LoadStars(Path.Combine("10", "input")).ToList();
            
            // Move until we think we have formation
            List<(Star, Star)> tuples = stars.CartesianJoinUnique().ToList();
            int second = 0;
            while (!HasFormed(tuples, (int)(stars.Count * 0.75)))
            {
                stars.Move();
                ++second;
            }

            // Task 1
            Print(stars, Path.Combine("10", "output.png"));

            // Task 2
            Console.WriteLine(second);
        }

        public static IEnumerable<Star> LoadStars(string path)
        {
            Regex regex = new Regex(@"position=<\s*(-?\d+), \s*(-?\d+)> velocity=<\s*(-?\d+),\s*(-?\d+)>");

            return File.ReadLines(path).Select(x => regex.Match(x)).Select(x => new Star()
            {
                X = int.Parse(x.Groups[1].Value),
                Y = int.Parse(x.Groups[2].Value),
                VelocityX = int.Parse(x.Groups[3].Value),
                VelocityY = int.Parse(x.Groups[4].Value),
            });
        }

        public static bool HasFormed(List<(Star, Star)> tuples, int minNeighbours)
        {
            return tuples.Count(x => Math.Abs(x.Item1.X - x.Item2.X) + Math.Abs(x.Item1.Y - x.Item2.Y) == 1) > minNeighbours;
        }

        public static void Print(List<Star> stars, string path)
        {
            int minX = stars.Min(x => x.X);
            int minY = stars.Min(x => x.Y);
            int maxX = stars.Max(x => x.X);
            int maxY = stars.Max(x => x.Y);

            using (Bitmap bmp = new Bitmap(maxX - minX + 1, maxY - minY + 1))
            using (Graphics image = Graphics.FromImage(bmp))
            {
                foreach (Star star in stars)
                    image.FillRectangle(Brushes.Black, star.X - minX, star.Y - minY, 1, 1);

                bmp.Save(path, ImageFormat.Png);
            }
        }
    }
}
