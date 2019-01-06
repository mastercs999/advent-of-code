using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2016._02
{
    public static class Challenge02
    {
        public static void Run()
        {
            // Load directions
            string[] directions = File.ReadAllLines(Path.Combine("02", "input"));

            // Task 1
            // Crack the code
            int code1 = int.Parse(String.Join("", CrackCode1(directions)));

            Console.WriteLine(code1);

            // Task 2
            string code2 = new string(CrackCode2(directions).ToArray());

            Console.WriteLine(code2);
        }

        private static IEnumerable<int> CrackCode1(string[] directions)
        {
            int currentNumber = 5;

            // Function that cares of not crossing edges of the keyboard
            int safeAdd(int number, int add) => number % 3 == 1 && add == -1 ||
                                                number % 3 == 0 && add == 1 ||
                                                number <= 3 && add == -3 ||
                                                number >= 7 && add == 3 ? number : number + add;

            // Functions that converts direction to move in number
            int calculateShift(char direction)
            {
                switch (direction)
                {
                    case 'U': return -3;
                    case 'D': return 3;
                    case 'L': return -1;
                    case 'R': return 1;
                    default: throw new InvalidOperationException();
                }
            }

            foreach (string numberDirections in directions)
            {
                foreach (char direction in numberDirections)
                    currentNumber = safeAdd(currentNumber, calculateShift(direction));

                yield return currentNumber;
            }
        }

        private static IEnumerable<char> CrackCode2(string[] directions)
        {
            char[,] keyboard = new char[,]
            {
                { '-', '-', '1', '-', '-' },
                { '-', '2', '3', '4', '-' },
                { '5', '6', '7', '8', '9' },
                { '-', 'A', 'B', 'C', '-' },
                { '-', '-', 'D', '-', '-' },
            };

            // Function converts direction to shifts in X and Y axis
            (int, int) decodeShift(char c)
            {
                switch (c)
                {
                    case 'U': return (0, -1);
                    case 'D': return (0, 1);
                    case 'L': return (-1, 0);
                    case 'R': return (1, 0);
                    default: throw new InvalidOperationException();
                }
            }

            // Functions say whether the move can or cannot be made
            bool cantMove(Point position, int dx, int dy) => position.X == 0 && dx < 0 ||
                                                             position.X == keyboard.GetLength(1) - 1 && dx > 0 ||
                                                             position.Y == 0 && dy < 0 ||
                                                             position.Y == keyboard.GetLength(0) - 1 && dy > 0 ||
                                                             keyboard[position.X + dx, position.Y + dy] == '-';
            bool canMove(Point position, int dx, int dy) => !cantMove(position, dx, dy);

            // We start at 5
            Point currentPosition = new Point(0, 2);

            // Go through lines
            foreach (string charDirections in directions)
            {
                foreach (char direction in charDirections)
                {
                    (int dx, int dy) = decodeShift(direction);

                    if (canMove(currentPosition, dx, dy))
                    {
                        currentPosition.X += dx;
                        currentPosition.Y += dy;
                    }
                }

                yield return keyboard[currentPosition.Y, currentPosition.X];
            }
        }
    }
}
