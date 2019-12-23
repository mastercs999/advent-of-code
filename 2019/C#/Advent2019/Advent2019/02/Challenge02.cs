using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Advent2019._02
{
    public static class Challenge02
    {
        public static void Run()
        {
            // Load and parse input
            int[] tape = File.ReadAllText(Path.Combine("02", "input")).Split(',').Select(int.Parse).ToArray();
           
            // Task 1
            int[] workingTape = tape.ToArray();

            // Make replacement
            workingTape[1] = 12;
            workingTape[2] = 2;

            // Execute the program
            Execute(workingTape);

            Console.WriteLine(workingTape[0]);

            // Task 2
            for (int noun = 0; noun < 100; ++ noun)
            for (int verb = 0; verb < 100; ++verb)
                {
                    // Copy of original tape
                    workingTape = tape.ToArray();

                    // Set up paramters
                    workingTape[1] = noun;
                    workingTape[2] = verb;

                    // Run
                    Execute(workingTape);

                    // Is it correct?
                    if (workingTape[0] == 19690720)
                    {
                        int output = 100 * noun + verb;

                        Console.WriteLine(output);
                        return;
                    }
                }
        }

        public static void Execute(int[] tape)
        {
            int position = 0;

            while (tape[position] != 99)
            {
                switch (tape[position])
                {
                    case 1:
                        tape[tape[position + 3]] = tape[tape[position + 1]] + tape[tape[position + 2]];
                        break;
                    case 2:
                        tape[tape[position + 3]] = tape[tape[position + 1]] * tape[tape[position + 2]];
                        break;
                }

                position += 4;
            }
        }
    }
}
