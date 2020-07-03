using SyntaxAnaylize;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ModVisitor;

namespace TaisEngine.ModManager
{
    internal class InitSelectDef : BaseDef<InitSelectDef.Element>
    {
        public InitSelectDef(string modName, List<MultiItem> modItems) : base(modName, modItems)
        {
        }

        internal class Element
        {
            [ModProperty("name")]
            public string name;

            [ModProperty("is_first")]
            public bool? is_first;

            [ModProperty("desc")]
            public Expr_MultiValue desc;

            [ModPropertyList("option")]
            public List<OptionDef> options;
        }
    }
}
