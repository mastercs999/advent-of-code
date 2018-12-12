using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2015._18
{
    public static class Challenge18
    {
        public static void Run()
        {
            // Load lights
            bool[,] lights = LoadLights(Path.Combine("18", "input"));

            // Task 1
            bool[,] result = lights;
            for (int i = 0; i < 100; ++i)
                result = NextStep(result, NextState);

            int isOnCount = result.Cast<bool>().Count(x => x);

            Console.WriteLine(isOnCount);

            // Task 2
            result = lights;
            for (int i = 0; i < 100; ++i)
                result = NextStep(result, NextState2);

            isOnCount = result.Cast<bool>().Count(x => x);

            Console.WriteLine(isOnCount);
        }

        public static bool[,] LoadLights(string path)
        {
            string[] lines = File.ReadAllLines(path);

            bool[,] lights = new bool[lines.Length, lines[0].Length];

            for (int y = 0; y < lines.Length; ++y)
                for (int x = 0; x < lines[y].Length; ++x)
                    lights[y, x] = lines[y][x] == '#';

            return lights;
        }

        public static bool[,] NextStep(bool[,] lights, Func<bool[,], int, int, bool> nextStateFunc)
        {
            bool[,] result = new bool[lights.GetLength(0), lights.GetLength(1)];

            for (int x = 0; x < lights.GetLength(1); ++x)
                for (int y = 0; y < lights.GetLength(0); ++y)
                    result[y, x] = nextStateFunc(lights, x, y);

            return result;
        }

        public static bool NextState(bool[,] lights, int x, int y)
        {
            int onCount = OnCount(lights, x, y);

            // On
            if (lights[y, x])
                return onCount == 2 || onCount == 3;
            else // Off
                return onCount == 3;
        }

        public static bool NextState2(bool[,] lights, int x, int y)
        {
            if ((x == 0 || x == lights.GetLength(1) - 1) &&
                (y == 0 || y == lights.GetLength(0) - 1))
                return true;

            return NextState(lights, x, y);
        }

        public static int OnCount(bool[,] lights, int x, int y)
        {
            int sum = 0;

            for (int i = Math.Max(0, x - 1); i <= Math.Min(lights.GetLength(1) - 1, x + 1); ++i)
                for (int j = Math.Max(0, y - 1); j <= Math.Min(lights.GetLength(0) - 1, y + 1); ++j)
                    if (x != i || y != j)
                        sum += lights[j, i] ? 1 : 0;

            return sum;
        }
    }
}
