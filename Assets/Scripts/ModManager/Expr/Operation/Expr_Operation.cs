using System;
using System.Collections.Generic;
using SyntaxAnaylize;

namespace TaisEngine.ModManager
{
    internal abstract class Expr_Operation
    {
        protected Item opRaw;

        internal static Expr_Operation Parse(Item item)
        {
            switch (item.key)
            {
                case "set.value":
                    return new Expr_SetValue(item);
                case "set.buffer_valid":
                    return new Expr_BufferValid(item);
                case "set.buffer_invalid":
                    return new Expr_BufferInvalid(item);
                default:
                    throw new Expr_Exception($"not support operation {item.key}", item);
            }
        }

        internal Expr_Operation(Item item)
        {
            this.opRaw = item;
        }

        internal abstract void Do();

        internal virtual void Check()
        {

        }
    }

    internal class Expr_OperationGroup
    {
        internal List<Expr_Operation> operations = new List<Expr_Operation>();

        public Expr_OperationGroup()
        {
        }

        public Expr_OperationGroup(MultiItem opRaw)
        {
            foreach(var elem in opRaw.elems)
            {
                operations.Add(Expr_Operation.Parse(elem));
            }
        }

        internal void Run()
        {
            foreach (var op in operations)
            {
                op.Do();
            }
        }

        internal void Check()
        {
            foreach(var op in operations)
            {
                op.Check();
            }
        }
    }
}
