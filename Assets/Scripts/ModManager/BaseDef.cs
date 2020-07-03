using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SyntaxAnaylize;

namespace TaisEngine.ModManager
{
    internal class BaseDef<T> where T:new()
    {
        static Dictionary<string, List<T>> dict = new Dictionary<string, List<T>>();

        static internal T Find(string name)
        {
            foreach (var mod in Mod.listMod.Where(x => x.content != null))
            {
                var finded = mod.content.initSelectDef.lists.Find(x => x.name == name);
                if (finded != null)
                {
                    return finded;
                }
            }

            throw new Exception("can not find INIT_SELECT:" + name);
        }

        static internal IEnumerable<T> Enumerate()
        {
            foreach (var mod in Mod.listMod.Where(x => x.content != null))
            {
                foreach (var elem in mod.content.initSelectDef.lists)
                {
                    yield return elem;
                }
            }
        }

        internal List<T> list = new List<T>();

        public BaseDef(string modName, IEnumerable<MultiItem> enumerable)
        {
            foreach(var multi in enumerable)
            {
                var elem = ModAnaylize.Parse<T>(multi);
                list.Add(elem);
            }

            dict.Add(modName, list);
        }
    }
}