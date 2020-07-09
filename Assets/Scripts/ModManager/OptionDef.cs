using System;
using System.Collections.Generic;
using System.Linq;
using ModVisitor;
using SyntaxAnaylize;

namespace TaisEngine.ModManager
{
    internal class OptionDef
    {
        [ModProperty("desc")]
        public Expr_MultiValue desc;

        [ModProperty("selected")]
        public Expr_OperationGroup selected;

        [ModProperty("next")]
        public Dictionary<string, Expr_Condition> next;

        //public OptionDef(string name, Value raw)
        //{
        //    if(!(raw is MultiItem))
        //    {
        //        throw new Exception($"option not support {raw}");
        //    }

        //    this.name = name;

        //    var opRaw = raw as MultiItem;

        //    object[] defValue = { name };
        //    this.desc = Expr_MultiValue.Parse(opRaw, "desc", defValue);

        //    this.selected = new Expr_OperationGroup(opRaw, "selected");
        //    this.next = new NextSelect(opRaw, "next");

        //}

        //internal static List<OptionDef> ParseList(SyntaxMod.MultiItem mod, string key, string parent)
        //{
        //    try
        //    {
        //        var rslt = new List<OptionDef>();

        //        var rawElems = mod.multiItem.elems.Where(x => x.key == key).ToArray();
        //        for (int i = 0; i < rawElems.Count(); i++)
        //        {
        //            rslt.Add(new OptionDef($"{parent}_OPTION_{i + 1}_DESC", rawElems[i].value));
        //        }

        //        return rslt;
        //    }
        //    catch(Exception e)
        //    {
        //        throw new Exception($"parse file faild! {mod.filePath}", e);
        //    }
        //}

        internal void Check()
        {
            if(selected == null)
            {
                throw new Exception($"OptionDef must have element selected");
            }

            selected.Check();
        }

        internal string getNextEvent()
        {
            if(next == null)
            {
                return null;
            }

            foreach (var elem in next)
            {
                if(elem.Value.Result())
                {
                    return elem.Key;
                }
            }

            return null;
        }
    }

    public class NextSelect
    {
        //private MultiItem opRaw;

        //public NextSelect(MultiItem opRaw, string key)
        //{
        //    this.opRaw = opRaw;
        //}

        internal string Get()
        {
            return "";
        }
    }
}
