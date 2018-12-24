using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2018._24
{
    public class Group
    {
        public ArmyType ArmyType { get; set; }
        public int Units { get; set; }
        public int UnitHitPoints { get; set; }
        public int Damage { get; set; }
        public AttackType AttackType { get; set; }
        public HashSet<AttackType> Weaknesses { get; set; }
        public HashSet<AttackType> Immunity { get; set; }
        public int Initiative { get; set; }

        public int EffectivePower => Units * Damage;
        public bool IsDead => Units <= 0;

        public void TakeDamage(AttackType attackType, int damage)
        {
            // Compute damage
            damage = ComputeDamage(attackType, damage);

            // Kill some units
            Units -= damage / UnitHitPoints;
        }
        public void TakeDamage(Group attacker) => TakeDamage(attacker.AttackType, attacker.EffectivePower);

        public int ComputeDamage(AttackType attackType, int damage)
        {
            if (Immunity.Contains(attackType))
                return 0;
            else if (Weaknesses.Contains(attackType))
                return damage * 2;
            else
                return damage;
        }
        public int ComputeDamage(Group attacker) => ComputeDamage(attacker.AttackType, attacker.EffectivePower);
    }
}
