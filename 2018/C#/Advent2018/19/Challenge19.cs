using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Advent2018._19
{
    public static class Challenge19
    {
        private static readonly Dictionary<string, Action<ulong[], ulong, ulong, ulong>> Instructions = new Dictionary<string, Action<ulong[], ulong, ulong, ulong>>()
        {
            { "addr", (reg, a, b, c) => reg[c] = reg[a] + reg[b] },
            { "addi", (reg, a, b, c) => reg[c] = reg[a] + b },
            { "mulr", (reg, a, b, c) => reg[c] = reg[a] * reg[b] },
            { "muli", (reg, a, b, c) => reg[c] = reg[a] * b },
            { "banr", (reg, a, b, c) => reg[c] = reg[a] & reg[b] },
            { "bani", (reg, a, b, c) => reg[c] = reg[a] & b },
            { "borr", (reg, a, b, c) => reg[c] = reg[a] | reg[b] },
            { "bori", (reg, a, b, c) => reg[c] = reg[a] | b },
            { "setr", (reg, a, b, c) => reg[c] = reg[a] },
            { "seti", (reg, a, b, c) => reg[c] = a },
            { "gtir", (reg, a, b, c) => reg[c] = a > reg[b] ? 1 : 0u },
            { "gtri", (reg, a, b, c) => reg[c] = reg[a] > b ? 1 : 0u },
            { "gtrr", (reg, a, b, c) => reg[c] = reg[a] > reg[b] ? 1 : 0u },
            { "eqir", (reg, a, b, c) => reg[c] = a == reg[b] ? 1 : 0u },
            { "eqri", (reg, a, b, c) => reg[c] = reg[a] == b ? 1 : 0u },
            { "eqrr", (reg, a, b, c) => reg[c] = reg[a] == reg[b] ? 1 : 0u },
        };

        public static void Run()
        {
            // Load input
            int ip = int.Parse(File.ReadLines(Path.Combine("19", "input")).First().Split(' ')[1]);
            List<Instruction> program = LoadProgram(Path.Combine("19", "input")).ToList();

            // Task 1
            ulong[] registers = ExecuteProgram(program, ip, new ulong[6], null);

            ulong register0 = registers[0];

            Console.WriteLine(register0);

            // Task 2
            registers = new ulong[6] { 1, 0, 0, 0, 0, 0 };
            ExecuteProgram(program, ip, registers, (ipValue, regs) =>
            {
                if (ipValue == 4)
                {
                    if (regs[2] % regs[1] == 0)
                        regs[5] = 1;

                    regs[4] = regs[2];
                }
            });

            register0 = registers[0];

            Console.WriteLine(register0);
        }

        private static IEnumerable<Instruction> LoadProgram(string path)
        {
            string[] lines = File.ReadAllLines(path).Skip(1).ToArray();
            int from = lines.ToList().FindLastIndex(x => x.StartsWith("After")) + 1;

            return lines.Skip(from).Where(x => !String.IsNullOrWhiteSpace(x)).Select(x =>
            {
                Match match = Regex.Match(x, @"(\w+) (\d+) (\d+) (\d+)");

                return new Instruction()
                {
                    Name = match.Groups[1].Value,
                    A = ulong.Parse(match.Groups[2].Value),
                    B = ulong.Parse(match.Groups[3].Value),
                    C = ulong.Parse(match.Groups[4].Value),
                };
            });

        }
        
        private static ulong[] ExecuteProgram(List<Instruction> program, int ip, ulong[] registers, Action<ulong, ulong[]> afterInstructionExec)
        {
            // Load IP
            ulong ipValue = registers[ip];

            // Run program
            while ((int)ipValue < program.Count)
            {
                // Load instruction
                Instruction instruction = program[(int)ipValue];

                // Execute instruction
                Instructions[instruction.Name](registers, instruction.A, instruction.B, instruction.C);

                // Run injected code after instruction execution
                afterInstructionExec?.Invoke(ipValue, registers);

                // Get next instruction pointer
                ipValue = ++registers[ip];
            }

            return registers;
        }
    }
}
