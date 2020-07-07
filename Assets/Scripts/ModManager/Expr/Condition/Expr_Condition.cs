using System;
using SyntaxAnaylize;

namespace TaisEngine.ModManager
{
    internal abstract class Expr_Condition : Expr<bool>
    {
        internal Expr_Condition(Value value) : base(value)
        {
        }

        //internal static Expr<bool> Parse(SyntaxMod.MultiItem mod, string name, bool? defValue)
        //{
        //    try
        //    {
        //        return Parse(mod.multiItem, name, defValue);
        //    }
        //    catch (Exception e)
        //    {
        //        throw new Exception($"parse file faild! {mod.filePath}", e);
        //    }
        //}

        internal static Expr_Condition Parse(Value modValue)
        {
            switch (modValue)
            {
                case Item item:
                    return ParseItem(item);
                case MultiItem multi:
                    return ParseItem(multi.elems[0]);
                case SingleValue single:
                    return ParseSingle(single);
                default:
                    throw new Exception($"Expr_Condition not support {modValue} ");
            }
        }

        //internal static Expr<bool> Parse(MultiItem multiItem, string name, bool? defValue)
        //{
        //    var modValue = multiItem.TryFind(name);
        //    if (modValue == null)
        //    {
        //        if (defValue != null)
        //        {
        //            return new ExprDefault<bool>(defValue.Value);
        //        }

        //        throw new Exception();
        //    }

        //    switch (modValue)
        //    {
        //        case MultiItem multi:
        //            return ParseMulti(multi);
        //        case SingleValue single:
        //            return ParseSingle(single);
        //        default:
        //            throw new Exception($"EvalExpr_Condition not support {modValue} ");
        //    }
        //}

        private static Expr_Condition ParseSingle(SingleValue single)
        {
            return new Expr_SingleCondition(single);
        }

        private static Expr_Condition ParseItem(Item item)
        {
            switch (item.key)
            {
                case "is.equal":
                    return new Expr_Equal(item.value);
                case "and":
                    return new Expr_And(item.value);
                case "not":
                    return new Expr_Not(item.value);
                case "is.buffer_valid":
                    return new Expr_IsBufferValid(item.value);
                default:
                    throw new Exception();
            }
        }
    }
}
