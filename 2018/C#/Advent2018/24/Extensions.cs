using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2018._24
{
    public static class Extensions
    {
        public static T ToEnum<T>(this string text)
        {
            foreach (T enumValue in Enum.GetValues(typeof(T)).Cast<T>())
                if (text.ToLower() == enumValue.ToString().ToLower())
                    return enumValue;

            throw new Exception($"Enum value for {text} not found.");
        }
    }
}
