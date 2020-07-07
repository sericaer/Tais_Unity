using System;
using System.Linq;

using System.Collections.Generic;
using SyntaxAnaylize;
using TaisEngine.Extensions;

namespace TaisEngine.ModManager
{
    internal class Expr_Not : Expr_Condition
    {
        internal static string key = "not";

        internal Expr_Not(Value value) : base(value)
        {
            if(!(value is MultiItem))
            {
                throw new Expr_Exception($"{key} only support MultiItem", value);
            }

            var multiItem = value as MultiItem;
            if (multiItem.elems.Count != 1)
            {
                throw new Expr_Exception($"{key} must support 1 element", value);
            }

            sub = Expr_Condition.Parse(multiItem.elems[0]);
        }

        internal override bool ResultImp()
        {
            return !sub.Result();
        }

        Expr_Condition sub;
    }
}
