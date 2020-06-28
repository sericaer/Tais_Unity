using System;
using SyntaxAnaylize;

namespace TaisEngine.ModManager
{
    internal class Expr_Equal : Expr_Condition
    {
        internal static string key;

        internal Expr_Equal(Value value) : base(value)
        {
            if(!(value is MultiValue))
            {
                throw new Expr_Exception($"{key} only support MultiValue", value);
            }

            var multiValue = value as MultiValue;
            if (multiValue.elems.Count != 2)
            {
                throw new Expr_Exception($"{key} only support 2 element", value);
            }

            left = new Factor<object>(multiValue.elems[0], Visitor.Type.READ);
            right = new Factor<object>(multiValue.elems[1], Visitor.Type.READ);
        }

        internal override bool ResultImp()
        {
            dynamic leftValue = left.Read();
            dynamic rightValue = right.Read();

            return leftValue == rightValue;
        }

        Factor<object> left;
        Factor<object> right;
    }
}
