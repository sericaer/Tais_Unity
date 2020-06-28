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

        public Expr<object[]> desc;

        public OptionSelected selected;

        public NextSelect next;

        public OptionDef(string name, Value raw)
        {
            if(!(raw is MultiItem))
            {
                throw new Exception($"option not support {raw}");
            }

            this.name = name;

            var opRaw = raw as MultiItem;

            object[] defValue = { name };
            this.desc = Expr_MultiValue.Parse(opRaw, "desc", defValue);

            this.selected = new OptionSelected(opRaw, "selected");
            this.next = new NextSelect(opRaw, "next");

        }

        internal static List<OptionDef> ParseList(SyntaxMod.Element mod, string key, string parent)
        {
            try
            {
                var rslt = new List<OptionDef>();

                var rawElems = mod.multiItem.elems.Where(x => x.key == key).ToArray();
                for (int i = 0; i < rawElems.Count(); i++)
                {
                    rslt.Add(new OptionDef($"{parent}_OPTION_{i + 1}_DESC", rawElems[i].value));
                }

                return rslt;
            }
            catch(Exception e)
            {
                throw new Exception($"parse file faild! {mod.filePath}", e);
            }
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
        internal List<Operation> operations = new List<Operation>();

        public OptionSelected(MultiItem opRaw, string key)
        {
            var raw = opRaw.TryFind<MultiItem>(key);
            if(raw == null)
            {
                return;
            }

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
        protected Item opRaw;

        internal static Operation Parse(Item item)
        {
            switch (item.key)
            {
                case "set.value":
                    return new OperationSetValue(item);
                default:
                    throw new Expr_Exception($"not support operation {item.key}", item);
            }
        }

        internal Operation(Item item)
        {
            this.opRaw = item;
        }

        internal abstract void Do();
    }

    internal class OperationSetValue : Operation
    {
        private Factor<object> left;
        private Factor<object> right;

        public OperationSetValue(Item item) : base(item)
        {
            if (!(item.value is MultiValue))
            {
                throw new Exception($"'set.value' not support {item.value}");
            }

            var multiValue = item.value as MultiValue;
            if (multiValue.elems.Count() != 2)
            {
                throw new Expr_Exception("'SET' operation only support 2 element", multiValue);
            }

            var dest = multiValue.elems[0];
            var src = multiValue.elems[1];

            left = new Factor<object>(dest, Visitor.Type.WRITE);
            right = new Factor<object>(src, Visitor.Type.READ);
        }

        internal override void Do()
        {
            left.Write(right.Read());
        }
    }
}
