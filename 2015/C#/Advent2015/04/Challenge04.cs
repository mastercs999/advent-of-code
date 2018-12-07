using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Advent2015._04
{
    public static class Challenge04
    {
        public static void Run()
        {
            string input = "ckczppom";

            // Task 1
            ulong mine1 = DoMine(input, 5);

            Console.WriteLine(mine1);

            // Task 2
            ulong mine2 = DoMine(input, 6);

            Console.WriteLine(mine2);
        }

        public static ulong DoMine(string input, int zerosCount)
        {
            string prefix = new string('0', zerosCount);

            string output = "";
            ulong number = 0;
            while (!output.StartsWith(prefix))
                output = CreateMD5(input + number++);

            return number - 1;
        }

        public static string CreateMD5(string input)
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
