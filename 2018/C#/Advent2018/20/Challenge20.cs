using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2018._20
{
    public static class Challenge20
    {
        public static void Run()
        {
            // Load input
            string input = File.ReadAllLines(Path.Combine("20", "input")).Single();

            // Trim first and last character
            input = input.Substring(1, input.Length - 2);

            // Create map
            Dictionary<Point, int> map = CreateMap(input);

            // Task 1
            // Calculate number of doors to most distanced room
            int doors = map.Select(x => x.Value).Max();

            Console.WriteLine(doors);

            // Task 2
            // Calculate number of rooms with more than 1000 doors
            int rooms = map.Count(x => x.Value >= 1000);

            Console.WriteLine(rooms);
        }

        private static Dictionary<Point, int> CreateMap(string input)
        {
            Point current = new Point(0, 0);
            int distance = 0;

            Dictionary<Point, int> roomToDistance = new Dictionary<Point, int>()
            {
                { current, 0 }
            };

            Dictionary<char, (int, int)> shift = new Dictionary<char, (int, int)>()
            {
                { 'N', (0, -1) },
                { 'W', (-1, 0) },
                { 'S', (0, 1) },
                { 'E', (1, 0) },
            };

            // Tracking
            Stack<(Point, int)> positionStack = new Stack<(Point, int)>();

            for (int i = 0; i < input.Length;++i)
            {
                switch (input[i])
                {
                    case 'N':
                    case 'W':
                    case 'S':
                    case 'E':
                        (int dx, int dy) = shift[input[i]];
                        current = new Point(current.X + dx, current.Y + dy);
                        ++distance;

                        if (!roomToDistance.ContainsKey(current))
                            roomToDistance[current] = distance;
                        else
                            roomToDistance[current] = Math.Min(roomToDistance[current], distance);
                        break;
                    case '(':
                        positionStack.Push((current, distance));
                        break;
                    case ')':
                        (current, distance) = positionStack.Pop();
                        break;
                    case '|':
                        (current, distance) = positionStack.Peek();
                        break;
                }
            }

            return roomToDistance;
        }
    }
}
