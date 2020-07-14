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
    internal abstract class BaseDefMulti<T> : ModAbstractNamed where T: ModAbstractNamed, new()
    {
        static Dictionary<string, List<(T def, string mod)>> dict = new Dictionary<string, List<(T, string)>>();

        static internal T Find(string name)
        {
            var rslt = TryFind(name);
            if (rslt == null)
            {
                throw new Exception("can not find:" + name);
            }

            return rslt;
        }

        static internal T TryFind(string name)
        {
            if (!dict.ContainsKey(name))
            {
                return default(T);
            }

            return dict[name].First().def;
        }

        static internal IEnumerable<T> Enumerate()
        {
            return dict.Values.Select(x => x.First().def);
        }

        static internal T Parse(string modName, SyntaxModElement syntaxModElem)
        {
            try
            {
                var rslt = ModAnaylize.Parse<T>(syntaxModElem.multiItem);
                rslt.SetDefault();

                if (!dict.ContainsKey(rslt.GetName()))
                {
                    dict[rslt.GetName()] = new List<(T def, string mod)>();
                }

                dict[rslt.GetName()].Add(((T)rslt, modName));

                return rslt;
            }
            catch (Exception e)
            {
                throw new Exception($"parse error in {syntaxModElem.filePath}", e);
            }
        }
    }

    internal abstract class BaseDef<T> : ModAbstract where T : BaseDef<T>, new()
    {
        static List<(T def, string mod)> list = new List<(T def, string mod)>();

        static internal T Get()
        {
            var rslt = TryGet();
            if (rslt.Equals(default(T)))
            {
                throw new Exception($"{typeof(T)} can is empty !");
            }

            return rslt;
        }

        static internal T TryGet()
        {
            if (list.Count() == 0)
            {
                return default(T);
            }

            return list.First().def;
        }

        static internal T Parse(string modName, SyntaxModElement syntaxModElem)
        {
            try
            {
                var rslt = ModAnaylize.Parse<T>(syntaxModElem.multiItem);
                rslt.SetDefault();

                list.Add(((T)rslt, modName));

                return rslt;
            }
            catch (Exception e)
            {
                throw new Exception($"parse error in {syntaxModElem.filePath}", e);
            }
        }
    }

    internal abstract class ModAbstract
    {
        internal abstract void SetDefault();
    }

    internal abstract class ModAbstractNamed : ModAbstract
    {
        internal abstract string GetName();
    }
}