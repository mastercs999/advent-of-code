using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Advent2018._24
{
    public static class Challenge24
    {
        public static void Run()
        {
            // Load groups
            List<Group> groups = LoadGroups(Path.Combine("24", "input")).ToList();

            // Do the war
            MakeWar(groups);

            // Task 1
            int units = groups.Sum(x => x.Units);
            
            Console.WriteLine(units);

            // Task 2
            int boost = 0;
            while (groups.Any(x => x.ArmyType == ArmyType.Infection))
            {
                // Load groups again
                groups = LoadGroups(Path.Combine("24", "input")).ToList();

                // Apply boost
                ++boost;
                foreach (Group immunity in groups.Where(x => x.ArmyType == ArmyType.Immunity))
                    immunity.Damage += boost;

                // Do the war
                MakeWar(groups);
            }

            units = groups.Sum(x => x.Units);

            Console.WriteLine(units);
        }

        public static IEnumerable<Group> LoadGroups(string path)
        {
            Regex regex = new Regex(@"(?<units>\d+) units each with (?<hitpoints>\d+) hit points( \((((weak to (?<weakness>\w+(, \w+)*))|(immune to (?<immunity>\w+(, \w+)*)))(; )?)+\))? with an attack that does (?<damage>\d+) (?<attacktype>\w+) damage at initiative (?<initiative>\d+)");

            ArmyType currentArmy = ArmyType.Immunity;
            foreach (string line in File.ReadLines(path))
            {
                Match match = regex.Match(line);
                if (match.Success)
                    yield return new Group()
                    {
                        ArmyType = currentArmy,
                        Units = int.Parse(match.Groups["units"].Value),
                        UnitHitPoints = int.Parse(match.Groups["hitpoints"].Value),
                        Damage = int.Parse(match.Groups["damage"].Value),
                        AttackType = match.Groups["attacktype"].Value.ToEnum<AttackType>(),
                        Weaknesses = match.Groups["weakness"].Success ? match.Groups["weakness"].Value.Split(',').Select(y => y.Trim().ToEnum<AttackType>()).ToHashSet() : new HashSet<AttackType>(),
                        Immunity = match.Groups["immunity"].Success ? match.Groups["immunity"].Value.Split(',').Select(y => y.Trim().ToEnum<AttackType>()).ToHashSet() : new HashSet<AttackType>(),
                        Initiative = int.Parse(match.Groups["initiative"].Value),
                    };
                else if (line == "Immune System:")
                    currentArmy = ArmyType.Immunity;
                else if (line == "Infection:")
                    currentArmy = ArmyType.Infection;
            }
        }

        private static void MakeWar(List<Group> groups)
        {
            // Make fight
            while (groups.Any(x => x.ArmyType == ArmyType.Immunity) && groups.Any(x => x.ArmyType == ArmyType.Infection))
            {
                // Choose targets
                Dictionary<Group, Group> attackerToTarget = TargetSelection(groups);

                // Make attacks
                int totalUnitsBefore = groups.Sum(x => x.Units);
                HashSet<Group> victims = AttackPhase(groups, attackerToTarget).ToHashSet();
                int totalUnitsAfter = groups.Sum(x => x.Units);

                // No units killed - its a tie
                if (totalUnitsBefore == totalUnitsAfter)
                    break;

                // Remove victims from groups
                groups.RemoveAll(x => victims.Contains(x));
            }
        }

        private static Dictionary<Group, Group> TargetSelection(List<Group> groups)
        {
            Dictionary<Group, Group> attackerToTarget = new Dictionary<Group, Group>();

            foreach (Group attacker in groups.OrderByDescending(x => x.EffectivePower).ThenByDescending(x => x.Initiative))
            {
                // Find the weakest target
                Group target = groups.Where(x => x.ArmyType != attacker.ArmyType && !attackerToTarget.ContainsValue(x) && x.ComputeDamage(attacker) > 0).OrderByDescending(x => x.ComputeDamage(attacker)).ThenByDescending(x => x.EffectivePower).ThenByDescending(x => x.Initiative).FirstOrDefault();
                if (target != null)
                    attackerToTarget.Add(attacker, target);
            }

            return attackerToTarget;
        }

        private static IEnumerable<Group> AttackPhase(List<Group> groups, Dictionary<Group, Group> attackerToTarget)
        {
            foreach (KeyValuePair<Group, Group> combat in attackerToTarget.OrderByDescending(x => x.Key.Initiative))
                if (!combat.Key.IsDead)
                    combat.Value.TakeDamage(combat.Key);

            return groups.Where(x => x.IsDead);
        }
    }
}
