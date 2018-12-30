using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2015._21
{
    public static class Challenge21
    {
        private static readonly Equipment[] Weapons = new Equipment[]
        {
            new Equipment(8, 4, 0),
            new Equipment(10, 5, 0),
            new Equipment(25, 6, 0),
            new Equipment(40, 7, 0),
            new Equipment(74, 8, 0),
        };

        private static readonly Equipment[] Armor = new Equipment[]
        {
            new Equipment(13, 0, 1),
            new Equipment(31, 0, 2),
            new Equipment(53, 0, 3),
            new Equipment(75, 0, 4),
            new Equipment(102, 0, 5),
        };

        private static readonly Equipment[] Rings = new Equipment[]
        {
            new Equipment(25, 1, 0),
            new Equipment(50, 2, 0),
            new Equipment(100, 3, 0),
            new Equipment(20, 0, 1),
            new Equipment(40, 0, 2),
            new Equipment(80, 0, 3),
        };

        public static void Run()
        {
            // Load boss characters
            Character boss = LoadBoss(Path.Combine("21", "input"));

            // Task 1
            int minCost = TryFights(boss, (playerWins, player) => playerWins ? player.EquipmentCost : int.MaxValue).Min();

            Console.WriteLine(minCost);

            // Task 2
            int maxCost = TryFights(boss, (playerWins, player) => !playerWins ? player.EquipmentCost : int.MinValue).Max();

            Console.WriteLine(maxCost);
        }

        private static Character LoadBoss(string path)
        {
            int[] numbers = File.ReadAllLines(path).Select(line => int.Parse(line.Split(':').Last())).ToArray();

            return new Character()
            {
                HitPoints = numbers[0],
                Equipment = new Equipment[] {
                    new Equipment()
                    {
                        Damage = numbers[1],
                        Armor = numbers[2]
                    }
                }
            };
        }

        private static IEnumerable<T> TryFights<T>(Character boss, Func<bool, Character, T> afterFightFunc)
        {
            foreach (Equipment[] equipment in AllEquipment())
            {
                // Create characters to fight
                Character player = new Character()
                {
                    HitPoints = 100,
                    Equipment = equipment
                };
                Character bossCopy = new Character()
                {
                    HitPoints = boss.HitPoints,
                    Equipment = boss.Equipment
                };

                // Simulate fight
                bool playerWins = FightWin(player, bossCopy);

                yield return afterFightFunc(playerWins, player);
            }
        }

        private static IEnumerable<Equipment[]> AllEquipment()
        {
            foreach (Equipment weapon in Weapons)
                foreach (Equipment armor in Armor.Concat(new Equipment[] { new Equipment() }))
                    foreach (Equipment leftRing in Rings.Concat(new Equipment[] { new Equipment() }))
                        foreach (Equipment rightRing in Rings.Concat(new Equipment[] { new Equipment() }))
                        {
                            // No same rings
                            if (leftRing == rightRing)
                                continue;

                            yield return new Equipment[] { weapon, armor, leftRing, rightRing };
                        }
        }

        private static bool FightWin(Character player, Character boss)
        {
            Character attacker = player;
            Character victim = boss;

            // Do fight until somebody wins
            while (player.HitPoints > 0 && boss.HitPoints > 0)
            {
                // Do hit
                victim.TakeHit(attacker);

                // Move to other player
                attacker = attacker == player ? boss : player;
                victim = victim == player ? boss : player;
            }

            return boss.HitPoints <= 0;
        }
    }
}
