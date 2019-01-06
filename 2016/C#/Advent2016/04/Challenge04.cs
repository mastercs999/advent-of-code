using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2016._04
{
    public static class Challenge04
    {
        public static void Run()
        {
            // Load rooms
            List<Room> rooms = LoadRooms(Path.Combine("04", "input")).ToList();

            // Task 1
            int sum = rooms.Where(x => x.IsValid()).Sum(x => x.Id);

            Console.WriteLine(sum);

            // Task 2
            int id = rooms.Single(x => x.DecodeName() == "northpole object storage").Id;

            Console.WriteLine(id);
        }

        public static IEnumerable<Room> LoadRooms(string path)
        {
            return File.ReadLines(path).Select(x =>
            {
                string[] parts = x.Split(new char[] { '-', '[', ']' }, StringSplitOptions.RemoveEmptyEntries).ToArray();

                return new Room()
                {
                    Name = String.Join("-", parts.Take(parts.Length - 2)),
                    Id = int.Parse(parts[parts.Length - 2]),
                    Checksum = parts.Last()
                };
            });
        }
    }
}
