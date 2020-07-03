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
    internal class PopDef
    {
        internal class Element
        {
            internal string name;

            internal bool is_tax;

            public Element(SyntaxMod.Element modElem)
            {
                this.name = Path.GetFileNameWithoutExtension(modElem.filePath).ToUpper();
                this.is_tax = Expr<bool>.staticParseMod(modElem, "is_tax");

                //var colorObj = Expr_MultiValue.Parse(modElem, "color", null).Result();
                //if(colorObj.Count() !=3)
                //{
                //    throw new Exception();
                //}

                //color = string.Join(",", colorObj);
            }

            internal void Check()
            {

            }
        }

        static internal Element Find(string name)
        {
            foreach (var mod in Mod.listMod.Where(x => x.content != null))
            {
                var finded = mod.content.popDef.lists.Find(x => x.name == name);
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
                foreach(var elem in mod.content.popDef.lists)
                {
                    yield return elem;
                }
            }
        }

        internal List<Element> lists = new List<Element>();

        internal PopDef(List<SyntaxMod.Element> modElements)
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
