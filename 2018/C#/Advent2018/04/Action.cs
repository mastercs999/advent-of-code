using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2018._04
{
    public class Action
    {
        public DateTime DateTime { get; set; }
        public ActionType ActionType { get; set; }
        public int GuardNumber { get; set; }

        public Action(string input)
        {
            string[] parts = input.Split(']');

            // Parse date
            DateTime = DateTime.ParseExact(parts[0], "[yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);

            // Parse action
            if (parts[1].Contains("#"))
            {
                ActionType = ActionType.BeginShift;
                GuardNumber = int.Parse(parts[1].Split(' ', '#')[3]);
            }
            else if (parts[1].Contains("asleep"))
                ActionType = ActionType.Asleep;
            else
                ActionType = ActionType.WakeUp;
        }

        public override string ToString()
        {
            return $"{DateTime} {ActionType} {GuardNumber}";
        }
    }
}
