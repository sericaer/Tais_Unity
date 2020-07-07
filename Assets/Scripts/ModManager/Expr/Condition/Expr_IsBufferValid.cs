using System;
using System.Linq;

using System.Collections.Generic;
using SyntaxAnaylize;
using TaisEngine.Extensions;
using TaisEngine.Run;

namespace TaisEngine.ModManager
{
    internal class Expr_IsBufferValid : Expr_Condition
    {
        internal static string key = "is.buffer_vaild";

        internal Expr_IsBufferValid(Value value) : base(value)
        {
            if (!(value is MultiValue))
            {
                throw new Exception($"'set.value' not support {value}");
            }

            var multiValue = value as MultiValue;
            if (multiValue.elems.Count != 2)
            {
                throw new Expr_Exception("'SET' operation only support 2 element", multiValue);
            }

            var dest = multiValue.elems[0];
            var src = multiValue.elems[1];

            left = new Factor<object>(dest, Visitor.VType.WRITE);
            right = new Factor<object>(src, Visitor.VType.READ);

            if (left.GetValueType() != typeof(BufferManager))
            {
                throw new Expr_Exception("1srt param in set.buffer_valid must be buffers", value);
            }
        }

        internal override bool ResultImp()
        {
            var buffMgr = left.Read() as BufferManager;
            return buffMgr.IsValid(right.Read() as string);
        }

        private Factor<object> left;
        private Factor<object> right;
    }
}
