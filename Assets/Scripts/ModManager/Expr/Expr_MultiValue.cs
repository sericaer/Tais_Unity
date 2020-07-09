using System;
using System.Collections.Generic;
using System.Linq;
using SyntaxAnaylize;

namespace TaisEngine.ModManager
{
    internal class Expr_MultiValue : Expr<object[]>
    {
        internal Expr_MultiValue(Value value) : base(value)
        {
        }

        internal static Expr_MultiValue Parse(SyntaxMod.ModItem mod, string name, object[] defValue)
        {
            try
            {
                return Parse(mod.multiItem, name, defValue);
            }
            catch (Exception e)
            {
                throw new Exception($"parse file faild! {mod.filePath}", e);
            }
        }

        internal static Expr_MultiValue Parse(MultiItem multiItem, string name, object[] defValue)
        {
            var modValue = multiItem.TryFind(name);
            if (modValue == null)
            {
                if (defValue != null)
                {
                    return new Expr_MultiValue(defValue);
                }

                throw new Expr_Exception($"can not find {name}", multiItem);
            }

            switch (modValue)
            {
                case MultiValue multi:
                    return new Expr_MultiValue(multi);
                default:
                    throw new Expr_Exception($"EvalExpr_Condition not support {modValue} ", modValue);
            }
        }

        internal override object[] ResultImp()
        {
            return factors.Select(x => x.Read()).ToArray();
        }

        internal Expr_MultiValue(params object[] defaultValue) : base(null)
        {
            this.defaultValue = defaultValue;
        }

        internal Expr_MultiValue(MultiValue multi) : base(multi)
        {
            factors = multi.elems.Select(x => new Factor<object>(x, Visitor.VType.READ)).ToList();
        }

        List<Factor<object>> factors;
    }
}
