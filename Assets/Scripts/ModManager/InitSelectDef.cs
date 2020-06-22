using SyntaxAnaylize;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ModVisitor;

namespace TaisEngine.ModManager
{
    internal class InitSelectDef
    {
        internal class Element
        {
            public string name;

            public Eval<bool> is_first;
            public Eval<string> desc;

            public List<OptionDef> options;

            public Element(string name, MultiItem modItems)
            {
                this.name = name.ToUpper();
                this.is_first = Eval<bool>.Parse("is_first", modItems, false);
                this.desc = Eval<string>.Parse("desc", modItems, $"{name}_DESC");

                this.options = new List<OptionDef>();
                var rawOptions = modItems.elems.Where(x => x.key == "option").ToArray();
                for (int i=0; i< rawOptions.Count(); i++)
                {
                    this.options.Add(new OptionDef($"{name}_OPTION_{i+1}", rawOptions[i].value));
                }
            }
        }

        static internal Element Find(string name)
        {
            foreach (var mod in Mod.listMod.Where(x => x.content != null))
            {
                var finded = mod.content.initSelectDef.lists.Find(x => x.name == name);
                if(finded != null)
                {
                    return finded;
                }
            }

            throw new Exception("can not find INIT_SELECT:" + name);
        }

        static internal IEnumerable<Element> Enumerate()
        {
            foreach(var mod in Mod.listMod.Where(x=>x.content != null))
            {
                foreach(var elem in mod.content.initSelectDef.lists)
                {
                    yield return elem;
                }
            }
        }

        internal List<Element> lists = new List<Element>();

        public InitSelectDef(List<SyntaxMod.Element> modElements)
        {
            if(modElements == null)
            {
                return;
            }

            foreach(var modElem in modElements)
            {
                var elem = new Element(modElem.name, modElem.multiItem);
                lists.Add(elem);
            }
        }
    }


    public class EvalSelected
    {
        internal static EvalSelected Parse(string key, MultiItem multiItem)
        {
            var rawSelected = multiItem.elems.SingleOrDefault(x => x.key == key);
            if (rawSelected == null)
            {
                throw new Exception();
            }

            return new EvalSelected(rawSelected.value as MultiItem);
        }

        internal EvalSelected(MultiItem rawSelected)
        {
            this.rawSelected = rawSelected;

            var setValue = rawSelected.elems.SingleOrDefault(x => x.key == "set.value");
            if(setValue != null)
            {
                operations.Add(new OperationSetValue(setValue.value as MultiValue));
            }
        }

        internal void Run()
        {
            foreach(var op in operations)
            {
                op.Do();
            }
        }

        List<Operation> operations = new List<Operation>();

        Action _Run;
        private Value rawSelected;
    }

    internal abstract class Operation
    {
        internal abstract void Do();
    }

    internal class OperationSetValue : Operation
    {
        private MultiValue multiValue;
        private Setter setter;
        private Getter getter;

        public OperationSetValue(MultiValue multiValue)
        {
            var setValueParam = multiValue;
            if (setValueParam.elems.Count() != 2)
            {
                throw new Exception();
            }

            var dest = setValueParam.elems[0];
            var src = setValueParam.elems[1];

            setter = new Setter(dest.value);
            getter = new Getter(src.value);
        }

        internal override void Do()
        {
            setter.set(getter.get());
        }
    }
}
