using System.Linq;

namespace TaisEngine.Extensions
{
    public static class Extensions
    {
        public static Depart FindByColor(this Depart[] departs, string color)
        {
            return departs.SingleOrDefault(x => x.def.color == color);
        }
    }
}