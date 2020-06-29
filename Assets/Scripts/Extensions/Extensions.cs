using System.Collections.Generic;
using System.Linq;

namespace TaisEngine.Extensions
{
    public static class Extensions
    {
        public static TaisEngine.Run.Depart FindByColor(this List<TaisEngine.Run.Depart> departs, string color)
        {
            return departs.SingleOrDefault(x => x.def.color == color);
        }
    }
}