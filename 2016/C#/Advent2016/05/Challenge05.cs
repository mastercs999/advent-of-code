using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Advent2016._05
{
    public static class Challenge05
    {
        public static void Run()
        {
            // This is the input
            string input = "wtnhxymk";

            // Task 1
            string code1 = CrackCode1(input);

            Console.WriteLine(code1);

            // Task 1
            string code2 = CrackCode2(input);

            Console.WriteLine(code2);
        }

        private static string CrackCode1(string input)
        {
            string code = "";

            int index = 0;
            while (code.Length < 8)
            {
                string hash = CreateMD5(input + index);
                if (hash.StartsWith("00000"))
                    code += hash[5];

                ++index;
            }

            return code;
        }
        private static string CrackCode2(string input)
        {
            char[] code = new char[8];

            ulong index = 0;
            while (code.Any(x => x == 0))
            {
                string hash = CreateMD5(input + index);
                if (hash.StartsWith("00000") && hash[5] >= '0' && hash[5] <= '7')
                {
                    int position = hash[5] - '0';
                    char c = hash[6];

                    if (code[position] == 0)
                        code[position] = c;
                }

                ++index;
            }

            return new string(code);
        }

        private static string CreateMD5(string input)
        {
            // Use input string to calculate MD5 hash
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                    sb.Append(hashBytes[i].ToString("X2"));

                return sb.ToString();
            }
        }
    }
}
