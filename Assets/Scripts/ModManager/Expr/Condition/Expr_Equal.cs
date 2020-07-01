﻿using System;
using System.Collections.Generic;
using SyntaxAnaylize;

namespace TaisEngine.ModManager
{
    internal class Expr_Equal : Expr_Condition
    {
        internal static string key = "is.equal";

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

            left = new Factor<object>(multiValue.elems[0], Visitor.VType.READ);
            right = new Factor<object>(multiValue.elems[1], Visitor.VType.READ);


            if(left.GetValueType() == right.GetValueType())
            {
                return;
            }

            if(IsNumericType(left.GetValueType()) && IsNumericType(right.GetValueType()))
            {
                return;
            }

            throw new Expr_Exception($"left type {left.GetValueType()} not same right {right.GetValueType()}", value);
        }

        internal override bool ResultImp()
        {
            dynamic leftValue = left.Read();
            dynamic rightValue = right.Read();

            return leftValue == rightValue;
        }

        Factor<object> left;
        Factor<object> right;

        private static HashSet<Type> NumericTypes = new HashSet<Type>
        {
            typeof(int),
            typeof(uint),
            typeof(double),
            typeof(decimal)
        };

        internal static bool IsNumericType(Type type)
        {
            return NumericTypes.Contains(type) ||
                   NumericTypes.Contains(Nullable.GetUnderlyingType(type));
        }
    }
}
