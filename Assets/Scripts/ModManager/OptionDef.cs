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
        public NextDef next;

        internal void Check()
        {
            if(selected == null)
            {
                throw new Exception($"OptionDef must have element selected");
            }

            selected.Check();

            next?.Check();
        }

        internal string getNextEvent()
        {
            if(next == null)
            {
                return null;
            }

            return next.Get();
        }
    }

    public class NextDef
    {
        internal Dictionary<string, Expr_Condition> group;
        internal string single = null;

        public NextDef(Value value)
        {
            group = new Dictionary<string, Expr_Condition>();

            switch (value)
            {
                case MultiItem multi:
                    foreach(var elem in multi.elems)
                    {
                        group.Add(elem.key, Expr_Condition.Parse(elem.value));
                    }
                    break;
                case SingleValue single:
                    this.single = single.value;
                    break;
                default:
                    throw new Expr_Exception("operation only support 'MultiItem', 'SingleValue'", value);
            }
        }

        internal void Check()
        {

        }

        internal string Get()
        {
            if(single != null)
            {
                return single;
            }

            foreach(var elem in group)
            {
                if(elem.Value.Result())
                {
                    return elem.Key;
                }
            }

            return null;
        }
    }
}
