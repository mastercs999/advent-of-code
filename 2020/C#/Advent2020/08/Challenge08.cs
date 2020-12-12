using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2020._08
{
    public static class Challenge08
    {
        private static readonly Dictionary<string, Action<int[], int>> instructions = new ()
        {
            { "nop", (x, _) => ++x[0] },
            { "acc", (x, y) => { x[1] += y; ++x[0]; } },
            { "jmp", (x, y) => x[0] += y },
        };

        public static void Run()
        {
            // Load instructions
            List<Instruction> program = LoadInstructions(Path.Combine("08", "input.txt")).ToList();

            // Task 1
            int failAcc = Execute(program).acc;
            Console.WriteLine(failAcc);

            // Task 2
            static Instruction Switch(Instruction instr) => instr.Name == "nop" ? instr with { Name = "jmp" } : instr.Name == "jmp" ? instr with { Name = "nop" } : instr;
            for (int i = 0; i < program.Count; ++i)
            {
                // Switch instruction
                Instruction originalInstruction = program[i];
                Instruction newInstruction = Switch(program[i]);
                if (originalInstruction == newInstruction)
                    continue;

                // Run program
                program[i] = newInstruction;
                (int acc, bool valid) = Execute(program);
                if (valid)
                {
                    Console.WriteLine(acc);
                    return;
                }

                // Switch back
                program[i] = originalInstruction;
            }
        }

        private static (int acc, bool valid) Execute(List<Instruction> program)
        {
            // Start with empty registers
            HashSet<int> seen = new HashSet<int>();
            int[] registers = new int[2];

            // Interpret
            while (!seen.Contains(registers[0]) && registers[0] < program.Count)
            {
                // Remember position
                seen.Add(registers[0]);

                // Execute instruction
                Instruction next = program[registers[0]];
                instructions[next.Name].Invoke(registers, next.Value);
            }

            return (registers[1], registers[0] >= program.Count);
        }

        private static IEnumerable<Instruction> LoadInstructions(string filePath)
        {
            return File.ReadLines(filePath).Select(x => new Instruction()
            {
                Name = x[0..3],
                Value = int.Parse(x[3..])
            });
        }
    }
}
