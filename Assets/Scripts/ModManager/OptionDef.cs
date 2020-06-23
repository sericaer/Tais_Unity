using System;
using System.Collections.Generic;
using System.Linq;
using ModVisitor;
using SyntaxAnaylize;

namespace TaisEngine.ModManager
{
    internal class OptionDef
    {
        public string name;
        public EvalExpr_MultiValue desc;

        public OptionSelected selected;
        public NextSelect next;

        private Value raw;

        public OptionDef(string name, MultiItem opRaw)
        {
            this.name = name;

            object[] defValue = { name };
            this.desc = EvalExpr_MultiValue.Parse(opRaw, "desc", defValue);

            this.selected = new OptionSelected(opRaw, "selected");
            this.next = new NextSelect(opRaw, "next");

            this.raw = opRaw;
        }
    }

    public class NextSelect
    {
        private MultiItem opRaw;

        public NextSelect(MultiItem opRaw, string key)
        {
            this.opRaw = opRaw;
        }

        internal string Get()
        {
            return "";
        }
    }

    public class OptionSelected
    {
        public MultiItem raw;
        internal List<Operation> operations = new List<Operation>();

        public OptionSelected(MultiItem opRaw, string key)
        {
            raw = opRaw.Find<MultiItem>(key);

            foreach(var elem in raw.elems)
            {
                operations.Add(Operation.Parse(elem));
            }
        }

        internal void Run()
        {
            foreach (var op in operations)
            {
                op.Do();
            }
        }
    }


    internal abstract class Operation
    {
        internal static Operation Parse(Item item)
        {
            if(item.key == "set.value")
            {
                if(!(item.value is MultiValue))
                {
                    throw new Exception("not multi value");
                }

                return new OperationSetValue(item.value as MultiValue);
            }

            throw new Exception();
        }

        internal abstract void Do();
    }

    internal class OperationSetValue : Operation
    {
        private MultiValue raw;
        private EvalExpr_SINGLE<object> left;
        private EvalExpr_SINGLE<object> right;

        public OperationSetValue(MultiValue multiValue)
        {
            raw = multiValue;
            if (raw.elems.Count() != 2)
            {
                throw new Exception();
            }

            var dest = raw.elems[0];
            var src = raw.elems[1];

            left = new EvalExpr_SINGLE<object>(dest, EvalExpr_SINGLE<object>.OPType.WRITE);
            right = new EvalExpr_SINGLE<object>(src, EvalExpr_SINGLE<object>.OPType.READ);
        }

        internal override void Do()
        {
            left.setter.set(right.getter.get());
        }
    }
}
