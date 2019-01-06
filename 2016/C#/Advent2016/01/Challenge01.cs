using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2016._01
{
    public static class Challenge01
    {
        public static void Run()
        {
            // Load directions
            List<Direction> directions = LoadDirections(Path.Combine("01", "input")).ToList();

            // Set starting point
            Point start = new Point(0, 0);

            // Follow the path
            List<Point> path = FollowPath(start, directions).ToList();

            // Task 1
            // Find the target point
            Point target = path.Last();

            // Calculate distance
            int distance = Math.Abs(target.X) + Math.Abs(target.Y);

            Console.WriteLine(distance);

            // Task 2
            Point duplicated = path.GroupBy(x => x).Where(x => x.Count() > 1).Select(x => x.Key).OrderBy(x => path.IndexOf(x)).First();

            // Calculate distance
            distance = Math.Abs(duplicated.X) + Math.Abs(duplicated.Y);

            Console.WriteLine(distance);
        }

        public static IEnumerable<Direction> LoadDirections(string path)
        {
            return File.ReadAllText(path).Split(',').Select(x => x.Trim()).Select(x => new Direction()
            {
                Shift = x[0] == 'R' ? 1 : -1,
                Length = int.Parse(x.Substring(1))
            });
        }

        public static IEnumerable<Point> FollowPath(Point start, List<Direction> directions)
        {
            // There is array with shifts
            (int, int)[] shifts = new (int, int)[] { (0, -1), (1, 0), (0, 1), (-1, 0) };

            // This function returns correct index to shifts array. This shifts array works like a circular array.
            int cycleIndex(int index) => index < 0 ? shifts.Length - Math.Abs(index) % shifts.Length :
                                         index >= shifts.Length ? index % shifts.Length :
                                         index;

            // We start facing north from the start
            int currentDirection = 0;
            Point currentPosition = start;

            // Yield the start
            yield return start;

            // Go through directions
            foreach (Direction direction in directions)
            {
                // Change the directions
                currentDirection = cycleIndex(currentDirection + direction.Shift);

                // Get vector of direction
                (int dx, int dy) = shifts[currentDirection];

                // Do the move
                for (int i = 0; i < direction.Length; ++i)
                {
                    // Do the move
                    currentPosition.X += dx;
                    currentPosition.Y += dy;

                    yield return currentPosition;
                }
            }
        }
    }
}
