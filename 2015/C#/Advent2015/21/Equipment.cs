using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2015._21
{
    public class Equipment
    {
        public int Cost { get; set; }
        public int Damage { get; set; }
        public int Armor { get; set; }

        public Equipment()
        {

        }

        public Equipment(int cost, int damage, int armor)
        {
            Cost = cost;
            Damage = damage;
            Armor = armor;
        }
    }
}
