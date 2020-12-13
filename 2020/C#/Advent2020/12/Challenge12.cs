using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2020._12
{
    public static class Challenge12
    {
        public static void Run()
        {
            // Load instruction
            List<Instruction> instructions = LoadInstructions(Path.Combine("12", "input.txt")).ToList();

            // Task 1
            Position position = new Position()
            {
                Direction = 'E'
            };
            foreach (Instruction instruction in instructions)
                position = instruction.Execute(position);
            int distance = Math.Abs(position.X) + Math.Abs(position.Y);
            Console.WriteLine(distance);

            // Task 2
            Position waypointPosition = new Position()
            {
                X = 10,
                Y = 1
            };
            Position shipPosition = new Position();
            foreach (Instruction instruction in instructions)
                (waypointPosition, shipPosition) = instruction.Execute2(waypointPosition, shipPosition);
            int distance2 = Math.Abs(shipPosition.X) + Math.Abs(shipPosition.Y);
            Console.WriteLine(distance2);
        }

        private static IEnumerable<Instruction> LoadInstructions(string filePath)
        {
            return File.ReadLines(filePath).Select(x => new Instruction()
            {
                Direction = x[0],
                Value = int.Parse(x[1..])
            });
        }
    }
}
