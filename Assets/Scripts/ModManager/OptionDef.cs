using System;
using System.Collections.Generic;
using System.Linq;
using ModVisitor;
using SyntaxAnaylize;

namespace TaisEngine.ModManager
{
    internal class OptionDef
    {
        public string name;

        public Expr<object[]> desc;

        public Expr_OperationGroup selected;

        public NextSelect next;

        public OptionDef(string name, Value raw)
        {
            if(!(raw is MultiItem))
            {
                throw new Exception($"option not support {raw}");
            }

            this.name = name;

            var opRaw = raw as MultiItem;

            object[] defValue = { name };
            this.desc = Expr_MultiValue.Parse(opRaw, "desc", defValue);

            this.selected = new Expr_OperationGroup(opRaw, "selected");
            this.next = new NextSelect(opRaw, "next");

        }

        internal static List<OptionDef> ParseList(SyntaxMod.Element mod, string key, string parent)
        {
            try
            {
                var rslt = new List<OptionDef>();

                var rawElems = mod.multiItem.elems.Where(x => x.key == key).ToArray();
                for (int i = 0; i < rawElems.Count(); i++)
                {
                    rslt.Add(new OptionDef($"{parent}_OPTION_{i + 1}_DESC", rawElems[i].value));
                }

                return rslt;
            }
            catch(Exception e)
            {
                throw new Exception($"parse file faild! {mod.filePath}", e);
            }
        }

        internal void Check()
        {
            selected.Check();
        }
    }

    public class NextSelect
    {
        private MultiItem opRaw;

        public NextSelect(MultiItem opRaw, string key)
        {
            this.opRaw = opRaw;
        }

        internal string Get()
        {
            return "";
        }
    }
}
