using Priority_Queue;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2015._22
{
    public static class Challenge22
    {
        private static SpellType[] PlayersSpells = new SpellType[]
        {
            SpellType.MagicMissile,
            SpellType.Drain,
            SpellType.Shield,
            SpellType.Poison,
            SpellType.Recharge,
        };

        private static CharacterFactory CharacterFactory;
        private static SpellFactory SpellFactory;

        public static void Run()
        {
            // Load boss' properties
            (int hitPoints, int damage) = LoadBoss(Path.Combine("22", "input"));

            // Create factories
            CharacterFactory = new CharacterFactory(hitPoints, 50, 500);
            SpellFactory = new SpellFactory(damage);

            // Task 1
            int leastMana = LeastManaSpend(0);

            Console.WriteLine(leastMana);

            // Task 1
            int leastMana2 = LeastManaSpend(-1);

            Console.WriteLine(leastMana2);
        }

        private static (int, int) LoadBoss(string path)
        {
            int[] numbers = File.ReadAllLines(path).Select(line => int.Parse(line.Split(':').Last())).ToArray();

            return (numbers[0], numbers[1]);
        }

        private static int LeastManaSpend(int hitPointsPenalty)
        {
            // Holds seqeunces, where index in a sequence determines round number and its value is casted spell
            SimplePriorityQueue<List<SpellType>> paths = new SimplePriorityQueue<List<SpellType>>();

            // Push onto stack empty path
            paths.Enqueue(new List<SpellType>(), 0);

            while (true)
            {
                // Pick a path
                List<SpellType> path = paths.First;
                int spent = (int)Math.Round(paths.GetPriority(path));
                paths.Dequeue();

                // Create characters
                Character player = CharacterFactory.CreatePlayer();
                Character boss = CharacterFactory.CreateBoss();

                // Do the fight
                bool win = Fight(player, boss, hitPointsPenalty, (round, attacker, victim, isBoss) =>
                {
                    // Boss has one type of spell only
                    if (isBoss)
                        return SpellFactory.CreateSpell(SpellType.BossDamage, attacker, victim);

                    // Divide round number as the player goes every second round
                    round /= 2;

                    // Either send spell in the path 
                    if (round < path.Count)
                        return SpellFactory.CreateSpell(path[round], attacker, victim);

                    // Otherwise push onto stack new path where we can move in this round
                    foreach (Spell spell in PlayersSpells.Select(x => SpellFactory.CreateSpell(x, attacker, victim)).Where(x => attacker.CanCastSpell(x)))
                        paths.Enqueue(path.Concat(new SpellType[] { spell.SpellType }).ToList(), spent + spell.Cost);

                    // End this fight as losing one
                    return null;
                });

                // We finally won
                if (win)
                    return spent;
            }
        }

        private static bool Fight(Character player, Character boss, int hitPointsPenalty, Func<int, Character, Character, bool, Spell> pickSpellFunc)
        {
            Character attacker = player;
            Character victim = boss;

            // Calculate rounds
            int round = 0;

            // Do fight until somebody wins
            while (player.IsAlive && boss.IsAlive)
            {
                ++round;

                // Perform health penalty
                if (hitPointsPenalty != 0 && attacker == player)
                {
                    --attacker.HitPoints;
                    if (attacker.IsDead)
                        break;
                }

                // Run all spells
                attacker.ExecuteSpells();
                victim.ExecuteSpells();

                // Did sombeody die?
                if (attacker.IsDead || victim.IsDead)
                    break;

                // Pick a spell. If no spell can be chosen, attacker is instantly dead.
                Spell spellToCast = pickSpellFunc(round, attacker, victim, attacker == boss);
                if (spellToCast == null)
                {
                    attacker.HitPoints = 0;
                    break;
                }

                // Deduct mana
                attacker.Mana -= spellToCast.Cost;

                // Execute immediately if the spell should be cast instantly, otherwise save it for later use
                if (spellToCast.IsInstant)
                    spellToCast.Execute();
                else
                    attacker.ActiveSpells.Add(spellToCast);

                // Move to other player
                attacker = attacker == player ? boss : player;
                victim = victim == player ? boss : player;
            }

            return player.IsAlive;
        }
    }
}
