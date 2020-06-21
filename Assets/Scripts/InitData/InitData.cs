using ModVisitor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaisEngine.Run;

namespace TaisEngine.Init
{
    public class InitData
    {
        internal static InitData inst;

        internal static void Generate()
        {
            inst = new InitData();
        }

        [ModVisit]
        public Taishou taishou;

        public InitData()
        {
            taishou = new Taishou();
        }
    }

    public class Taishou
    {
        [ModVisit]
        public string background;

        [ModVisit]
        public string name;

        [ModVisit]
        public int age;

        public static (int min, int max) ageRange = (25, 55);
    }
}
