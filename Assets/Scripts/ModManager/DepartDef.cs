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
    internal class DepartDef
    {
        internal class Element
        {
            internal string name;

            internal string color;

            internal Dictionary<string, int> popInitDict;

            public Element(SyntaxMod.MultiItem modElem)
            {
                this.name = Path.GetFileNameWithoutExtension(modElem.filePath).ToUpper();

                var colorObj = Expr<object[]>.staticParseMod(modElem, "color");
                if(colorObj.Count() !=3)
                {
                    throw new Exception();
                }

                color = string.Join(",", colorObj);

                popInitDict = Expr<Dictionary<string, string>>.staticParseMod(modElem, "pop_init").ToDictionary(k => k.Key, v => int.Parse(v.Value));
            }

            internal void Check()
            {

            }
        }

        static internal Element Find(string name)
        {
            foreach (var mod in Mod.listMod.Where(x => x.content != null))
            {
                var finded = mod.content.departDef.lists.Find(x => x.name == name);
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
                foreach(var elem in mod.content.departDef.lists)
                {
                    yield return elem;
                }
            }
        }

        internal List<Element> lists = new List<Element>();

        internal DepartDef(List<SyntaxMod.MultiItem> modElements)
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

        internal void Check()
        {
            foreach(var curr in lists)
            {
                curr.Check();
            }
        }
    }
}
