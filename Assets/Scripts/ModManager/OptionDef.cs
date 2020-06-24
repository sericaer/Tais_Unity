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

        public Expr_MultiValue desc;

        public OptionSelected selected;

        public NextSelect next;

        public OptionDef(string name, Value raw)
        {
            if(!(raw is MultiValue))
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
        public MultiItem raw;
        internal List<Operation> operations = new List<Operation>();

        public OptionSelected(MultiItem opRaw, string key)
        {
            raw = opRaw.TryFind<MultiItem>(key);
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
        internal static Operation Parse(Item item)
        {
            switch(item.key)
            {
                case "SET":
                    if (!(item.value is MultiValue))
                    {
                        throw new Exception($"'SET' not support {item.value}");
                    }

                    return new OperationSetValue(item.value as MultiValue);
                default:
                    throw new Expr_Exception($"not support operation {item.key}", item);
            }
        }

        internal abstract void Do();
    }

    internal class OperationSetValue : Operation
    {
        private Factor<object> left;
        private Factor<object> right;

        public OperationSetValue(MultiValue multiValue)
        {
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
