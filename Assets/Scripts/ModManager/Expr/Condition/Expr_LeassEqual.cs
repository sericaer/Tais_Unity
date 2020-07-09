using System;
using System.Collections.Generic;
using SyntaxAnaylize;
using TaisEngine.Extensions;

namespace TaisEngine.ModManager
{
    internal class Expr_LessEqual : Expr_Condition
    {
        internal static string key = "is.less_equal";

        internal Expr_LessEqual(Value value) : base(value)
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

            left = new Factor<object>(multiValue.elems[0], Visitor.VType.READ);
            right = new Factor<object>(multiValue.elems[1], Visitor.VType.READ);


            if(left.GetValueType() == right.GetValueType())
            {
                return;
            }

            if(left.GetValueType().IsNumericType() && right.GetValueType().IsNumericType())
            {
                return;
            }

            throw new Expr_Exception($"left type {left.GetValueType()} not same right {right.GetValueType()}", value);
        }

        internal override bool ResultImp()
        {
            dynamic leftValue = left.Read();
            dynamic rightValue = right.Read();

            return leftValue <= rightValue;
        }

        Factor<object> left;
        Factor<object> right;
    }
}
