using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SyntaxAnaylize
{
    public class Eval<T>
    {
        public static Eval<T> Parse(string key, MultiItem modItems, T defaultValue)
        {
            var modItem = modItems.elems.SingleOrDefault(x => x.key == key);
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
            if (raw == "true")
            {
                ret = true;
            }
            else if (raw == "false")
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

            var method = typeof(Converter).GetMethod("Convert", BindingFlags.Static | BindingFlags.Public, null, paramTypes, null);
            if (method == null)
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
