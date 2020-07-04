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
        static Dictionary<string, List<(T def, string mod)>> dict = new Dictionary<string, List<(T, string)>>();

        static internal T Find(string name)
        {
            if (!dict.ContainsKey(name))
            {
                throw new Exception("can not find INIT_SELECT:" + name);
            }

            return dict[name].First().def;
        }

        static internal IEnumerable<T> Enumerate()
        {
            return dict.Values.Select(x => x.First().def);
        }

        static internal List<T> ParseList(string modName, IEnumerable<MultiItem> enumerable)
        {
            var rslt = new List<T>();

            foreach (var multi in enumerable)
            {
                dynamic elem = ModAnaylize.Parse<T>(multi);
                elem.SetDefault();

                if (!dict.ContainsKey(elem.name))
                {
                    dict[elem.name] = new List<(T def, string mod)>();
                }

                dict[elem.name].Add(((T)elem, modName));
            }

            return rslt;
        }
    }
}