using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TaisEngine.Extensions
{
    public static class Extensions
    {
        public static TaisEngine.Run.Depart FindByColor(this List<TaisEngine.Run.Depart> departs, string color)
        {
            return departs.SingleOrDefault(x => x.def.color == color);
        }

        private static HashSet<Type> NumericTypes = new HashSet<Type>
        {
            typeof(int),
            typeof(uint),
            typeof(double),
            typeof(decimal)
        };

        internal static bool IsNumericType(this Type type)
        {
            return NumericTypes.Contains(type) || NumericTypes.Contains(Nullable.GetUnderlyingType(type));
        }
    }
}