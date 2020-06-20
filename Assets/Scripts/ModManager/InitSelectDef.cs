using SyntaxAnaylize;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
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

            public Element(string name, MultiItem modItems)
            {
                this.name = name ;
                this.is_first = Eval<bool>.Parse("is_first", modItems, false);
                this.desc = Eval<string>.Parse("desc", modItems, $"{name}_DESC");
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

    public class Eval<T>
    {
        public static Eval<T> Parse(string key, MultiItem modItems, T defaultValue)
        {
            var modItem = modItems.elems.SingleOrDefault(x=>x.key == key);
            if (modItem == null)
            {
                var ret = new Eval<T>();
                ret.staticValue = defaultValue;
                ret._Result = () =>
                {
                    return ret.staticValue;
                };

                return ret;
            }

            return Parse(modItem.value);
        }

        public static Eval<T> Parse(Value modValue)
        {
            if (modValue is SingleValue)
            {
                return new EvalFactor<T>(modValue as SingleValue);
            }

            throw new Exception();
        }

        public T Result()
        {
            return _Result == null ? staticValue : _Result();
        }
        
        public T staticValue;

        protected Func<T> _Result;
    }

    public class EvalFactor<T> : Eval<T>
    {
        public EvalFactor(SingleValue modValue)
        {
            this.modValue = modValue;
            this._Result = Converter.GetFunc<T>(modValue.value);

        }

        internal SingleValue modValue;
    }

    public abstract class EvalExpr<T> : Eval<T>
    {
        internal List<Eval<object>> evals;
    }

    //public abstract class EvalExpr_And : EvalExpr<bool>
    //{
    //    public override bool Result()
    //    {
    //        return !evals.Any(x => !((bool)x.Result()));
    //    }
    //}

    public class Converter
    {
        public static void Convert(string raw, out bool ret)
        {
            if(raw == "true")
            {
                ret = true;
            }
            else if(raw == "false")
            {
                ret = false;
            }
            else
            {
                throw new Exception($"{raw} must be 'true' or 'false'");
            }
        }


        public static Func<T> GetFunc<T>(string value)
        {
            Type[] paramTypes = { typeof(string), typeof(T).MakeByRefType() };

            var method = typeof(Converter).GetMethod("Convert", BindingFlags.Static| BindingFlags.Public, null, paramTypes, null);
            if(method == null)
            {
                throw new Exception();
            }

            return () =>
            {
                var args = new object[] { value, null };
                method.Invoke(null, args);
                return (T)args[1];
            };
        }
    }

}
