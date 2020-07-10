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
    internal class InitSelectDef : BaseDefMulti<InitSelectDef>
    {
        [ModProperty("name")]
        public string name;

        [ModProperty("is_first")]
        public bool? is_first;

        [ModProperty("desc")]
        public Expr_MultiValue desc;

        [ModPropertyList("option")]
        public List<OptionDef> options;

        internal static void AnaylzeMod(Mod mod, SyntaxModElement modElemnts)
        {
            mod.content.initSelectDefs.Add(InitSelectDef.Parse(mod.info.name, modElemnts));
        }

        internal override string GetName()
        {
            return name;
        }

        internal override void SetDefault()
        {
            if (is_first == null)
            {
                is_first = false;
            }

            if(desc == null)
            {
                desc = new Expr_MultiValue(name + "_DESC");
            }

            for(int i=0; i<options.Count(); i++)
            {
                var op = options[i];

                if (op.desc == null)
                {
                    op.desc = new Expr_MultiValue($"{name}_OPTION_{i+1}_DESC");
                }
            }

            //options.ForEach(x => x.CheckDefault());
        }
    }
}
