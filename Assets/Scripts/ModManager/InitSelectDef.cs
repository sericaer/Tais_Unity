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

            public Expr_Condition is_first;
            public Expr_MultiValue desc;

            public List<OptionDef> options;

            public Element(SyntaxMod.Element modElem)
            {
                this.name = Path.GetFileNameWithoutExtension(modElem.filePath).ToUpper();

                this.is_first = Expr_Condition.Parse(modElem, "is_first", false);

                object[] defDesc = { this.name + "_DESC" };
                this.desc = Expr_MultiValue.Parse(modElem, "desc", defDesc);

                this.options = OptionDef.ParseList(modElem, "option"); 
                //new List<OptionDef>();
                //var rawOptions = modElem.multiItem.elems.Where(x => x.key == "option").ToArray();
                //for (int i=0; i< rawOptions.Count(); i++)
                //{
                //    this.options.Add(new OptionDef($"{name}_OPTION_{i+1}_DESC", rawOptions[i].value as MultiItem));
                //}
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
                var elem = new Element(modElem);
                lists.Add(elem);
            }
        }
    }
}
