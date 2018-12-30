using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2015._21
{
    public class Character
    {
        public int HitPoints { get; set; }
        public Equipment[] Equipment { get; set; }

        public int Damage => Equipment.Sum(x => x.Damage);
        public int Armor => Equipment.Sum(x => x.Armor);
        public int EquipmentCost => Equipment.Sum(x => x.Cost);

        public void TakeHit(Character attacker)
        {
            HitPoints -= Math.Max(1, attacker.Damage - Armor);
        }
    }
}
