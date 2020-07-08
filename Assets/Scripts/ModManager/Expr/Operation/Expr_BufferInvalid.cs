using System;
using SyntaxAnaylize;
using TaisEngine.Run;

namespace TaisEngine.ModManager
{
    internal class Expr_BufferInvalid : Expr_Operation
    {
        private Factor<object> left;
        private Factor<object> right;

        public Expr_BufferInvalid(Item item) : base(item)
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

            if (left.GetValueType() != typeof(BufferManager))
            {
                throw new Expr_Exception("1srt param in set.buffer_valid must be buffers", item);
            }
        }

        internal override void Check()
        {
            switch(left.raw)
            {
                case "depart.buffer":
                    if(BufferGroup.BufferDefDepart.Find(right.raw) == null)
                    {
                        throw new Expr_Exception($"'{right.raw}' not in '{left.raw}'", base.opRaw);
                    }
                    break;
                default:
                    throw new Exception();
            }


        }

        internal override void Do()
        {
            var buffMgr = left.Read() as BufferManager;
            buffMgr.SetInvalid(right.Read() as string);
        }
    }
}
