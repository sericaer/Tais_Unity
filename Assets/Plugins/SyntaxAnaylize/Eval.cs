using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ModVisitor;

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
            if(modValue is MultiItem)
            {
                if(((MultiItem)modValue).elems.Count() != 1)
                {
                    throw new Exception("eval modValue should have 1 elem, curr is " + ((MultiItem)modValue).elems.Count());
                }

                return EvalExpr<T>.Parse(((MultiItem)modValue).elems[0] as Item);
            }

            throw new NotImplementedException();
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
        public static EvalExpr<T> Parse(Item modItem)
        {
            if (modItem.key == "is.equal")
            {
                if(typeof(T) != typeof(bool))
                {
                    throw new Exception("'is.equal' expect 'bool' type, but curr is " + typeof(T).Name);
                }

                return new EvalExpr_Equal<T>(modItem.value);
            }

            throw new NotImplementedException();
        }
    }

    public class EvalExpr_Equal<T> : EvalExpr<T>
    {
        private Value value;

        public EvalExpr_Equal(Value value)
        {
            this.value = value;
            if(!(value is MultiValue))
            {
                throw new Exception("'EvalExpr_Equal' value expect 'MultiValue' but curr is " + value.GetType().Name);
            }

            var elements = ((MultiValue)value).elems;
            if(elements.Count() != 2)
            {
                throw new Exception("'EvalExpr_Equal' value expect have 2 elements but curr is " + elements.Count());
            }

            left = new Getter(elements[0].value);
            right = new Getter(elements[1].value);

            _Result = () =>
            {
                var l = left.get();
                var r = right.get();
                if(l.GetType() != r.GetType())
                {
                    throw new Exception($"EvalExpr_Equal left type is {l.GetType().Name}, but right type is {r.GetType().Name}");
                }

                object rslt = (l == r);
                return (T)rslt;
            };
        }

        private Getter left;
        private Getter right;
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
