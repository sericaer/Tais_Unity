using System;
using SyntaxAnaylize;

namespace TaisEngine.ModManager
{
    internal class Expr_SetValue : Expr_Operation
    {
        private Factor<object> left;
        private Factor<object> right;

        public Expr_SetValue(Item item) : base(item)
        {
            if (!(item.value is MultiValue))
            {
                throw new Exception($"'set.value' not support {item.value}");
            }

            var multiValue = item.value as MultiValue;
            if (multiValue.elems.Count != 2)
            {
                throw new Expr_Exception("'SET' operation only support 2 element", multiValue);
            }

            var dest = multiValue.elems[0];
            var src = multiValue.elems[1];

            left = new Factor<object>(dest, Visitor.VType.WRITE);
            right = new Factor<object>(src, Visitor.VType.READ);
        }

        internal override void Do()
        {
            left.Write(right.Read());
        }
    }
}
