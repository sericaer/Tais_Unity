using SyntaxAnaylize;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TaisEngine.ModManager
{
    class InitSelectDef
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

        internal InitSelectDef(string path)
        {
            lists = new List<Element>();

            foreach (var file in Directory.EnumerateFiles(path, "*.txt"))
            {
                MultiItem modItems = Syntax.Anaylize(File.ReadAllText(file));

                var elem = new Element(Path.GetFileNameWithoutExtension(file), modItems);

                lists.Add(elem);
            }
        }

    }

    internal class OptionDef
    {
        public string name;
        public Eval<string> desc;
        public EvalSelected selected;

        private Value opRaw;

        public OptionDef(string name, Value opRaw)
        {
            this.name = name;
            this.desc = Eval<string>.Parse("desc", opRaw as MultiItem, $"{name}_DESC");
            this.selected = EvalSelected.Parse("selected", opRaw as MultiItem);

            this.opRaw = opRaw;
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

        public OperationSetValue(MultiValue multiValue)
        {
            var setValueParam = multiValue;
            if (setValueParam.elems.Count() != 2)
            {
                throw new Exception();
            }

            var dest = setValueParam.elems[0];
            var src = setValueParam.elems[1];

            var setter = new Setter(dest.value);
            var getter = new Getter(src.value);

            
        }

        internal override void Do()
        {
            setter.set(getter.value());
        }
    }

    internal class Getter
    {
        private string raw;

        public Getter(string value)
        {
            this.raw = value;
        }

        internal object value()
        {
            throw new NotImplementedException();
        }
    }

    internal class Setter
    {
        private string raw;

        public Setter(string value)
        {
            raw = value;

            int start = 0;
            while(start < raw.Length)
            {
                var matched = Regex.Match(raw.Substring(start), @"^[A-Za-z]+\.*");
                if(!matched.Success)
                {
                    throw new Exception();
                }

                var currProperty = GetType().GetProperty(matched.Value);

                ;

                start += rslt.Length;
            }
            
            if (rslt.Success)
            {
                endIndex = charIndex + rslt.Length;
                return ELEM_TYPE.STRING;
            }
        }

        internal void set(object value)
        {
            throw new NotImplementedException();
        }
    }
}
