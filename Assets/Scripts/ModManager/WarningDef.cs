using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SyntaxAnaylize;

namespace TaisEngine.ModManager
{
    class WarningDef : BaseDefMulti<WarningDef>
    {
        [ModProperty("name")]
        internal string name;

        [ModProperty("trigger")]
        public Expr_Condition trigger;

        [ModProperty("desc")]
        public Expr_MultiValue desc;

        internal override string GetName()
        {
            return name;
        }

        internal override void SetDefault()
        {

        }

        internal static void AnaylizeMod(Mod mod, SyntaxModElement modElemnts)
        {
            mod.content.warningDefs.Add(WarningDef.Parse(mod.info.name, modElemnts));
        }
    }
}
