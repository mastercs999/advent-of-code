using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2018._22
{
    public static class Challenge22
    {
        private static readonly Dictionary<RegionType, Tool[]> RegionToTools = new Dictionary<RegionType, Tool[]>()
        {
            { RegionType.Rocky, new Tool[] { Tool.Torch, Tool.ClimbingGear } },
            { RegionType.Wet, new Tool[] { Tool.None, Tool.ClimbingGear } },
            { RegionType.Narrow, new Tool[] { Tool.None, Tool.Torch } },
        };

        public static void Run()
        {
            // Load input
            (int depth, Point target) = LoadInput(Path.Combine("22", "input"));

            // Build risk level map
            RegionType[,] riskLevelMap = BuildRiskLevelMap(depth, target);

            // Task 1
            int totalRiskLevel = riskLevelMap.ToQueryable((x, y, risk) => (x: x, y: y, risk: risk)).Where(x => x.x <= target.X && x.y <= target.Y).Sum(x => (int)x.risk);

            Console.WriteLine(totalRiskLevel);

            // Task 2
            Dictionary<Point, PathPoint> routes = FindShortestPaths(riskLevelMap, new Point(0, 0));
            PathPoint lastPoint = routes[target];

            int changeDistance = lastPoint.ToolToOrigin.Select(x => x.Value).Where(x => x.Tool != Tool.Torch).OrderBy(x => x.Distance).Select(x => x.Distance).Concat(new int[] { int.MaxValue - 7 }).Min() + 7;
            int noChangeDistance = lastPoint.ToolToOrigin.TryGetValue(Tool.Torch, out Origin origin) ? origin.Distance : int.MaxValue;
            int distance = Math.Min(changeDistance, noChangeDistance);

            Console.WriteLine(distance);
        }

        public static (int, Point) LoadInput(string path)
        {
            string[] lines = File.ReadAllLines(path);

            // Parse depth
            int depth = int.Parse(lines[0].Split(' ')[1]);

            // Parse target location
            string[] parts = lines[1].Split(' ', ',');
            int x = int.Parse(parts[1]);
            int y = int.Parse(parts[2]);

            return (depth, new Point(x, y));
        }
        private static void PrintMap(RegionType[,] riskLevelMap, string path)
        {
            using (StreamWriter sw = new StreamWriter(path))
                for (int y = 0; y < riskLevelMap.GetLength(0); ++y)
                {
                    for (int x = 0; x < riskLevelMap.GetLength(1); ++x)
                    {
                        RegionType val = riskLevelMap[y, x];
                        char c;
                        if (val == RegionType.Rocky)
                            c = '.';
                        else if (val == RegionType.Wet)
                            c = '=';
                        else
                            c = '|';

                        sw.Write(c);
                    }
                    sw.WriteLine();
                }
        }

        public static RegionType[,] BuildRiskLevelMap(int depth, Point target)
        {
            int xMax = target.X + 100;
            int yMax = target.Y + 100;

            int[,] geologicalMap = new int[yMax, xMax];
            int[,] erosionMap = new int[yMax, xMax];
            RegionType[,] riskLevelMap = new RegionType[yMax, xMax];

            for (int y = 0; y < yMax; ++y)
                for (int x = 0; x < xMax; ++x)
                {
                    geologicalMap[y, x] = CalculateGeologicalIndex(x, y, erosionMap, target);
                    erosionMap[y, x] = CalculateErosionIndex(x, y, geologicalMap, depth);
                    riskLevelMap[y, x] = CalculateRiskLevel(x, y, erosionMap, target);
                }

            return riskLevelMap;
        }
        private static int CalculateGeologicalIndex(int x, int y, int[,] erosionMap, Point target)
        {
            if (x == 0 && y == 0 || x == target.X && y == target.Y)
                return 0;
            else if (y == 0)
                return x * 16807;
            else if (x == 0)
                return y * 48271;
            else
                return erosionMap[y, x - 1] * erosionMap[y - 1, x];
        }
        private static int CalculateErosionIndex(int x, int y, int[,] geologicalMap, int depth)
        {
            return (geologicalMap[y, x] + depth) % 20183;
        }
        private static RegionType CalculateRiskLevel(int x, int y, int[,] erosionMap, Point target)
        {
            if (target.X == x && target.Y == y)
                return RegionType.Rocky;
            else
                return (RegionType)(erosionMap[y, x] % 3);
        }

        private static Dictionary<Point, PathPoint> FindShortestPaths(RegionType[,] riskLevelMap, Point start)
        {
            // Dictionary with found paths
            Dictionary<Point, PathPoint> routes = riskLevelMap.ToQueryable((x, y, item) => new PathPoint()
            {
                Location = new Point(x, y),
                RegionType = riskLevelMap[y, x],
                ToolToOrigin = RegionToTools[riskLevelMap[y, x]].ToDictionary(z => z, z => new Origin(null, z, int.MaxValue - 10))
            }).ToDictionary(x => x.Location);

            // Set starting point
            routes[start].ToolToOrigin[Tool.Torch] = new Origin(null, Tool.Torch, 0);

            // Create process query
            Queue<PathPoint> toProcess = new Queue<PathPoint>();
            toProcess.Enqueue(routes[start]);

            // Faster search in toProcess
            HashSet<PathPoint> toProcessHash = new HashSet<PathPoint>() { toProcess.Single() };

            // Fast search for neighbours
            Dictionary<PathPoint, PathPoint[]> neigbours = routes.ToDictionary(x => x.Value, x => x.Value.Neighbours(routes).ToArray());

            // Main search loop
            while (toProcess.Any())
            {
                PathPoint currentPoint = toProcess.Dequeue();
                toProcessHash.Remove(currentPoint);

                // Examine all neighbours
                foreach (PathPoint neighbour in neigbours[currentPoint])
                {
                    // What if we didn't change the tool
                    foreach (Tool tool in currentPoint.ToolToOrigin.Keys.Intersect(RegionToTools[neighbour.RegionType]))
                    {
                        int newDistance = currentPoint.ToolToOrigin[tool].Distance + 1;
                        if (neighbour.ToolToOrigin[tool].Distance > newDistance)
                        {
                            neighbour.ToolToOrigin[tool] = new Origin(currentPoint, tool, newDistance);
                            if (!toProcessHash.Contains(neighbour))
                            {
                                toProcess.Enqueue(neighbour);
                                toProcessHash.Add(neighbour);
                            }
                        }
                    }

                    // Or we have to change the tool
                    foreach (Tool oldTool in currentPoint.ToolToOrigin.Keys.Except(RegionToTools[neighbour.RegionType]))
                        foreach (Tool newTool in RegionToTools[neighbour.RegionType])
                        {
                            int newDistance = currentPoint.ToolToOrigin[oldTool].Distance + 1 + 7;
                            if (neighbour.ToolToOrigin[newTool].Distance > newDistance)
                            {
                                neighbour.ToolToOrigin[newTool] = new Origin(currentPoint, newTool, newDistance);
                                if (!toProcessHash.Contains(neighbour))
                                {
                                    toProcess.Enqueue(neighbour);
                                    toProcessHash.Add(neighbour);
                                }
                            }
                        }
                }
            }

            return routes;
        }
        private static Tool NextTool(RegionType from, RegionType to)
        {
            return RegionToTools[from].Intersect(RegionToTools[to]).Single();
        }
    }
}
