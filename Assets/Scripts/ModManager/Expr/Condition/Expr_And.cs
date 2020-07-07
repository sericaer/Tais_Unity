using System;
using System.Linq;

using System.Collections.Generic;
using SyntaxAnaylize;
using TaisEngine.Extensions;

namespace TaisEngine.ModManager
{
    internal class Expr_And : Expr_Condition
    {
        internal static string key = "and";

        internal Expr_And(Value value) : base(value)
        {
            if(!(value is MultiItem))
            {
                throw new Expr_Exception($"{key} only support MultiItem", value);
            }

            var multiItem = value as MultiItem;
            if (multiItem.elems.Count < 2)
            {
                throw new Expr_Exception($"{key} must support > 2 element", value);
            }

            foreach(var elem in multiItem.elems)
            {
                list.Add(Expr_Condition.Parse(elem));
            }

            //left = new Factor<object>(multiValue.elems[0], Visitor.VType.READ);
            //right = new Factor<object>(multiValue.elems[1], Visitor.VType.READ);


            //if(left.GetValueType() == right.GetValueType())
            //{
            //    return;
            //}

            //if(left.GetValueType().IsNumericType() && right.GetValueType().IsNumericType())
            //{
            //    return;
            //}

            //throw new Expr_Exception($"left type {left.GetValueType()} not same right {right.GetValueType()}", value);
        }

        internal override bool ResultImp()
        {
            return list.All(x => x.ResultImp());
        }

        List<Expr_Condition> list = new List<Expr_Condition>();
    }
}
