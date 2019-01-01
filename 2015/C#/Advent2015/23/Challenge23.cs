using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2015._23
{
    public static class Challenge23
    {
        private static readonly Dictionary<string, Func<int[], int, int, int>> NameToInstruction = new Dictionary<string, Func<int[], int, int, int>>()
        {
            { "hlf", (regs, arg1, arg2) => { regs[arg1] /= 2; return 1; } },
            { "tpl", (regs, arg1, arg2) => { regs[arg1] *= 3; return 1; } },
            { "inc", (regs, arg1, arg2) => { regs[arg1] += 1; return 1; } },
            { "jmp", (regs, arg1, arg2) => arg1 },
            { "jie", (regs, arg1, arg2) => regs[arg1] % 2 == 0 ? arg2 : 1},
            { "jio", (regs, arg1, arg2) => regs[arg1] == 1 ? arg2 : 1},
        };

        public static void Run()
        {
            // Load program
            List<Instruction> program = LoadInstructions(Path.Combine("23", "input")).ToList();

            // Task 1
            int[] registers = new int[2];
            RunProgram(program, registers);

            int regB = registers[1];

            Console.WriteLine(regB);

            // Task 1
            registers = new int[2] { 1, 0 };
            RunProgram(program, registers);

            regB = registers[1];

            Console.WriteLine(regB);
        }

        private static IEnumerable<Instruction> LoadInstructions(string path)
        {
            return File.ReadLines(path).Select(x =>
            {
                string[] parts = x.Split(new char[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);

                return new Instruction()
                {
                    Name = parts[0],
                    Arg1 = parts[1] == "a" || parts[1] == "b" ? parts[1][0] - 'a' : int.Parse(parts[1]),
                    Arg2 = parts.Length > 2 ? int.Parse(parts[2]) : 0
                };
            });
        }

        private static void RunProgram(List<Instruction> program, int[] registers)
        {
            int ptr = 0;

            while (ptr < program.Count)
            {
                Instruction instruction = program[ptr];

                ptr += NameToInstruction[instruction.Name](registers, instruction.Arg1, instruction.Arg2);
            }
        }
    }
}
