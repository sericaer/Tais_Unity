using System;
using SyntaxAnaylize;

namespace TaisEngine.ModManager
{
    internal class Expr_SingleCondition : Expr_Condition
    {
        internal override bool Result()
        {
            return factor.Read();
        }

        internal Expr_SingleCondition(SingleValue modValue) : base(modValue)
        {
            factor = new Factor<bool>(modValue, Visitor.Type.READ);
        }

        Factor<bool> factor;
    }
}
