using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Advent2016._08
{
    public static class Challenge08
    {
        public static void Run()
        {
            // Load instructions
            List<Action<bool[,]>> instructions = LoadInstructions(Path.Combine("08", "input")).ToList();

            // Task 1
            bool[,] screen = new bool[6, 50];

            foreach (Action<bool[,]> instruction in instructions)
                instruction(screen);

            int onCount = screen.Cast<bool>().Count(x => x);

            Console.WriteLine(onCount);

            // Task 2
            PrintScreen(screen);
        }

        private static IEnumerable<Action<bool[,]>> LoadInstructions(string path)
        {
            Regex regex = new Regex(@"((?<rect>rect (\d+)x(\d+))|(?<rowR>rotate row y=(\d+) by (\d+))|(?<colR>rotate column x=(\d+) by (\d+)))");

            return File.ReadLines(path).Select(x =>
            {
                Match match = regex.Match(x);

                return (Action<bool[,]>)((screen) =>
                {
                    if (match.Groups["rect"].Success)
                    {
                        int width = int.Parse(match.Groups[2].Value);
                        int height = int.Parse(match.Groups[3].Value);

                        LightOn(screen, width, height);
                    }
                    else if (match.Groups["rowR"].Success)
                    {
                        int row = int.Parse(match.Groups[4].Value);
                        int count = int.Parse(match.Groups[5].Value);

                        RotateRow(screen, row, count);
                    }
                    else if (match.Groups["colR"].Success)
                    {
                        int column = int.Parse(match.Groups[6].Value);
                        int count = int.Parse(match.Groups[7].Value);

                        RotateColumn(screen, column, count);
                    }
                    else
                        throw new InvalidOperationException();
                });
            });
        }

        private static void LightOn(bool[,] screen, int width, int height)
        {
            for (int i = 0; i < height; ++i)
                for (int j = 0; j < width; ++j)
                    screen[i, j] = true;
        }

        private static void RotateRow(bool[,] screen, int row, int count)
        {
            int columns = screen.GetLength(1);

            // We don't neeed to rotate twice
            count %= columns;

            for (int i = 0; i < count; ++i)
            {
                bool last = screen[row, columns - 1];

                for (int j = columns - 1; j > 0; --j)
                    screen[row, j] = screen[row, j - 1];

                screen[row, 0] = last;

            }
        }

        private static void RotateColumn(bool[,] screen, int column, int count)
        {
            int rows = screen.GetLength(0);

            // We don't neeed to rotate twice
            count %= rows;

            for (int i = 0; i < count; ++i)
            {
                bool last = screen[rows - 1, column];

                for (int j = rows - 1; j > 0; --j)
                    screen[j, column] = screen[j - 1, column];

                screen[0, column] = last;
            }
        }

        private static void PrintScreen(bool[,] screen)
        {
            for (int i = 0; i < screen.GetLength(0); ++i)
            {
                for (int j = 0; j < screen.GetLength(1); ++j)
                    Console.Write(screen[i, j] ? '#' : '.');
                Console.WriteLine();
            }
        }
    }
}
