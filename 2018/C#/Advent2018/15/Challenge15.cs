using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2018._15
{
    public static class Challenge15
    {
        public static void Run()
        {
            // Load map
            Node[,] map = LoadMap(Path.Combine("15", "input"));

            // Task 1
            (int rounds, List<Unit> units) = GameOn(map);

            int result = rounds * units.Sum(x => x.HitPoints);

            Console.WriteLine(result);

            // Task 2
            // Run until no elf dies
            int attackPower = 3;
            while (true)
            {
                // Load map
                map = LoadMap(Path.Combine("15", "input"));

                // What is elves count
                List<Unit> elves = map.Cast<Node>().Where(x => x.NodeType == NodeType.Elf).Cast<Unit>().ToList();

                // Set attack power
                foreach (Unit elf in map.Cast<Node>().Where(x => x.NodeType == NodeType.Elf).Cast<Unit>())
                    elf.AttackPower = attackPower;

                // Run the game
                (rounds, units) = GameOn(map);

                // No elf died?
                if (elves.Count == units.Where(x => x.NodeType == NodeType.Elf).Count())
                {
                    result = rounds * units.Sum(x => x.HitPoints);

                    Console.WriteLine(result);

                    break;
                }

                ++attackPower;
            }
        }

        private static Node[,] LoadMap(string path)
        {
            // Load file content
            string[] lines = File.ReadAllLines(path);

            // Init map
            Node[,] map = new Node[lines.Length, lines[0].Length];

            // Parse map
            for (int x = 0; x < map.GetLength(1); ++x)
                for (int y = 0; y < map.GetLength(0); ++y)
                    map[y, x] = ParseNode(lines[y][x], x, y);

            return map;
        }
        private static Node ParseNode(char c, int x, int y)
        {
            Point point = new Point(x, y);

            switch (c)
            {
                case '#':
                    return new Node()
                    {
                        Location = point,
                        NodeType = NodeType.Wall
                    };
                case '.':
                    return new Node()
                    {
                        Location = point,
                        NodeType = NodeType.Empty
                    };
                case 'G':
                    return new Unit()
                    {
                        Location = point,
                        NodeType = NodeType.Goblin,
                    };
                case 'E':
                    return new Unit()
                    {
                        Location = point,
                        NodeType = NodeType.Elf
                    };
                default:
                    throw new InvalidOperationException();
            }
        }
        private static void PrintMap(string path, Node[,] map)
        {
            using (StreamWriter sw = new StreamWriter(path))
                for (int y = 0; y < map.GetLength(0); ++y)
                {
                    string title = " ";

                    for (int x = 0; x < map.GetLength(1); ++x)
                    {
                        NodeType type = map[y, x].NodeType;
                        char c = type == NodeType.Elf ? 'E' : type == NodeType.Goblin ? 'G' : type == NodeType.Empty ? '.' : '#';
                        sw.Write(c);

                        if (map[y, x].NodeType == NodeType.Elf || map[y, x].NodeType == NodeType.Goblin)
                            title += c + "(" + (map[y, x] as Unit).HitPoints + ") ";
                    }
                    sw.Write(title);
                    sw.WriteLine();
                }
        }

        private static Dictionary<Point, PathPoint> FindShortestPaths(Node[,] map, Node startNode)
        {
            // Dictionary with found paths
            Dictionary<Point, PathPoint> foundPaths = new Dictionary<Point, PathPoint>();

            // We remember what we went through
            HashSet<Point> examinedPoints = new HashSet<Point>();

            // Starting point
            Queue<PathPoint> toProcess = new Queue<PathPoint>();
            toProcess.Enqueue(new PathPoint()
            {
                Node = startNode,
                From = null,
                Distance = 0,
            });

            // Main search loop
            while (toProcess.Any())
            {
                PathPoint currentPoint = toProcess.Dequeue();

                // Add to examined
                examinedPoints.Add(currentPoint.Node.Location);

                // Examine all neighbours
                foreach (Node neighbour in currentPoint.Node.Neighbours(map).Where(x => x.NodeType != NodeType.Wall))
                {
                    PathPoint pathPoint = new PathPoint()
                    {
                        Node = neighbour,
                        Distance = currentPoint.Distance + 1,
                        From = currentPoint
                    };

                    // Add or replace
                    if (foundPaths.TryGetValue(neighbour.Location, out PathPoint existingPathPoint))
                    {
                        if (existingPathPoint.Distance > pathPoint.Distance)
                        {
                            foundPaths[neighbour.Location] = pathPoint;
                            toProcess.Enqueue(pathPoint);
                        }
                    }
                    else
                        foundPaths.Add(neighbour.Location, pathPoint);

                    // Add to process
                    if (!examinedPoints.Contains(neighbour.Location) && !toProcess.Any(x => x.Node.Location == neighbour.Location) && neighbour.NodeType == NodeType.Empty)
                        toProcess.Enqueue(pathPoint);
                }
            }

            return foundPaths;
        }
        private static IEnumerable<Node> BuildPath(Dictionary<Point, PathPoint> shortestPaths, Node startNode, Node targetNode)
        {
            IEnumerable<Node> buildPathReversed()
            {
                Node currentNode = targetNode;

                while (currentNode != startNode)
                {
                    yield return currentNode;

                    currentNode = shortestPaths[currentNode.Location].From.Node;
                }

                yield return currentNode;
            }

            return buildPathReversed().Reverse();
        }

        private static void Move(Node[,] map, Unit unit, Point newPosition)
        {
            // Old position is empty now
            map[unit.Location.Y, unit.Location.X] = new Node()
            {
                Location = unit.Location,
                NodeType = NodeType.Empty
            };

            // Unit is now on new position
            map[newPosition.Y, newPosition.X] = unit;
            unit.Location = newPosition;
        }
        private static void Fight(Node[,] map, Unit attacker, Unit victim)
        {
            victim.HitPoints -= attacker.AttackPower;
        }

        private static (int, List<Unit>) GameOn(Node[,] map)
        {
            // Get all units
            List<Unit> units = map.Cast<Node>().Where(x => x.NodeType == NodeType.Elf || x.NodeType == NodeType.Goblin).Cast<Unit>().ToList();

            // Now simulate the game
            int rounds = 0;
            while (true)
            {
                HashSet<Unit> deadUnits = new HashSet<Unit>();

                foreach (Unit unit in units.OrderBy(x => x.Location.Y).ThenBy(x => x.Location.X))
                {
                    // Skip dead units
                    if (deadUnits.Contains(unit))
                        continue;

                    // No victim? So end!
                    if (units.Except(deadUnits).All(x => x.NodeType == NodeType.Elf) || units.Except(deadUnits).All(x => x.NodeType == NodeType.Goblin))
                        return (rounds, units.Except(deadUnits).ToList());

                    // Oponent type
                    NodeType oponentType = unit.NodeType == NodeType.Elf ? NodeType.Goblin : NodeType.Elf;

                    // Find whether is any victim nearby and fight with him
                    Unit findVictim() => unit.Neighbours(map).Where(x => x.NodeType == oponentType).Cast<Unit>().OrderBy(x => x.HitPoints).ThenBy(x => x.Location.Y).ThenBy(x => x.Location.X).FirstOrDefault() as Unit;
                    Unit victim = findVictim();

                    // No victim nearby? Move first
                    if (victim == null)
                    {
                        // Find shortest path in current configuration
                        Dictionary<Point, PathPoint> shortestPaths = FindShortestPaths(map, unit);

                        // Find the nearest target
                        victim = units.Except(deadUnits).Where(x => x.NodeType == oponentType && shortestPaths.ContainsKey(x.Location)).OrderBy(x => shortestPaths[x.Location].Distance).ThenBy(x => shortestPaths[x.Location].Node.Location.Y).ThenBy(x => shortestPaths[x.Location].Node.Location.X).FirstOrDefault();
                        if (victim == null)
                            continue;

                        // Find new position - second step in the path
                        Point newPosition = BuildPath(shortestPaths, unit, victim).ElementAt(1).Location;

                        // Move the unit to new position
                        Move(map, unit, newPosition);
                    }

                    //  Try to attack
                    victim = findVictim();
                    if (victim != null)
                    {
                        // Fight
                        Fight(map, unit, victim);

                        // Remove dead unit
                        if (victim.HitPoints <= 0)
                        {
                            map[victim.Location.Y, victim.Location.X] = new Node()
                            {
                                Location = victim.Location,
                                NodeType = NodeType.Empty
                            };

                            deadUnits.Add(victim);
                        }
                    }
                }
                
                foreach (Unit dead in deadUnits)
                    units.Remove(dead);

                ++rounds;
            }
        }
    }
}
