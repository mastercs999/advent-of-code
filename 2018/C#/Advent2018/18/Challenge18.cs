using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2018._18
{
    public static class Challenge18
    {
        public static void Run()
        {
            // Load area
            Acre[,] area = LoadArea(Path.Combine("18", "input"));

            // Task 1
            for (int i = 0; i < 10; ++i)
                area = Next(area);

            int resource = CalculateResource(area);

            Console.WriteLine(resource);

            // Task 2
            // Detect cycle
            Dictionary<ulong, List<Acre>> snapshots = new Dictionary<ulong, List<Acre>>();
            for (ulong i = 10; i < 1000000000; ++i)
            {
                area = Next(area);

                List<Acre> snapshot = area.Cast<Acre>().ToList();

                // Cycle detected
                ulong? index = snapshots.Where(x => x.Value.SequenceEqual(snapshot)).Select(x => (ulong?)x.Key).FirstOrDefault();
                if (index.HasValue)
                {
                    // Skip complete periods
                    ulong period = i - index.Value;
                    ulong rest = 1000000000 - i - 1;

                    for (i = 0; i < rest % period; ++i)
                        area = Next(area);

                    // Calculate result
                    resource = CalculateResource(area);

                    Console.WriteLine(resource);
                    break;
                }

                // Save snapshot
                snapshots[i] = snapshot;
            }

        }

        private static Acre[,] LoadArea(string path)
        {
            string[] lines = File.ReadAllLines(path);

            Acre[,] area = new Acre[lines.Length, lines[0].Length];

            for (int x = 0; x < area.GetLength(1); ++x)
                for (int y = 0; y < area.GetLength(0); ++y)
                    area[y, x] = Decode(lines[y][x]);

            return area;
        }
        private static Acre Decode(char c)
        {
            return c == '.' ? Acre.Open : c == '|' ? Acre.Tree : Acre.Lumberyard;
        }

        private static Acre[,] Next(Acre[,] oldArea)
        {
            Acre[,] newArea = new Acre[oldArea.GetLength(1), oldArea.GetLength(0)];

            for (int y = 0; y < oldArea.GetLength(0); ++y)
                for (int x = 0; x < oldArea.GetLength(1); ++x)
                    newArea[y, x] = NextAcre(oldArea, x, y);

            return newArea;
        }
        private static Acre NextAcre(Acre[,] area, int x, int y)
        {
            List<Acre> neigbours = area.Neighbours(x, y).ToList();

            switch (area[y, x])
            {
                case Acre.Open: return neigbours.Count(n => n == Acre.Tree) >= 3 ? Acre.Tree : area[y, x];
                case Acre.Tree: return neigbours.Count(n => n == Acre.Lumberyard) >= 3 ? Acre.Lumberyard : area[y, x];
                case Acre.Lumberyard: return neigbours.Any(n => n == Acre.Lumberyard) && neigbours.Any(n => n == Acre.Tree) ? Acre.Lumberyard : Acre.Open;
            }

            throw new InvalidOperationException();
        }

        private static int CalculateResource(Acre[,] area) => area.Cast<Acre>().Count(x => x == Acre.Lumberyard) * area.Cast<Acre>().Count(x => x == Acre.Tree);
    }
}
