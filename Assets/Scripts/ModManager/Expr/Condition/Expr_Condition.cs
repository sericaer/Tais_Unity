using System;
using SyntaxAnaylize;

namespace TaisEngine.ModManager
{
    internal class Expr_Condition : Expr<bool>
    {
        internal Expr_Condition(Value value) : base(value)
        {
        }

        internal static Expr_Condition Parse(SyntaxMod.Element mod, string name, bool? defValue)
        {
            try
            {
                var modValue = mod.multiItem.TryFind(name);
                if (modValue == null)
                {
                    if (defValue != null)
                    {
                        return new Expr_Condition(null) { defaultValue = defValue.Value };
                    }

                    throw new Exception();
                }

                switch (modValue)
                {
                    case MultiItem multi:
                        return ParseMulti(multi);
                    case SingleValue single:
                        return ParseSingle(single);
                    default:
                        throw new Exception($"EvalExpr_Condition not support {modValue} ");
                }
            }
            catch (Exception e)
            {
                throw new Exception($"parse file faild! {mod.filePath}", e);
            }

        }

        private static Expr_Condition ParseSingle(SingleValue single)
        {
            return new Expr_SingleCondition(single);
        }

        private static Expr_Condition ParseMulti(MultiItem items)
        {
            if (items.elems.Count != 1)
            {
                throw new Exception($"EvalExpr_Condition should only 1 element, but curr is {items.elems.Count}");
            }

            switch (items.elems[0].key)
            {
                case "EQUAL":
                    return new Expr_Equal(items.elems[0].value);
                default:
                    throw new Exception();
            }
        }
    }
}
