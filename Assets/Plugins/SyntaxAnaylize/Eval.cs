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
        public T Result()
        {
            return _Result == null ? defaultValue : _Result();
        }

        public T defaultValue;
        public Value modValue;

        protected Func<T> _Result;
    }

    public class EvalExpr_ModifierGroup : Eval<double>
    {
        public static EvalExpr_ModifierGroup Parse(MultiItem modItems, string key, double? defVale)
        {
            var find = modItems.TryFind<MultiItem>(key);
            if(find == null)
            {
                if(defVale == null)
                {
                    throw new Exception($"can not find {key} in {modItems}");
                }

                return new EvalExpr_ModifierGroup() { defaultValue = defVale.Value };
            }

            return new EvalExpr_ModifierGroup(find);
        }


        internal EvalExpr_SINGLE<double> baseValue;
        internal List<EvalExpr_Modifier> modifiers = new List<EvalExpr_Modifier>();

        public EvalExpr_ModifierGroup(MultiItem modItem)
        {
            var modBaseValue = modItem.Find<SingleValue>("base_value");

            baseValue = new EvalExpr_SINGLE<double>(modBaseValue, EvalExpr_SINGLE<double>.OPType.READ);
            foreach(var elem in modItem.elems.Where(x=>x.key == "modifier"))
            {
                modifiers.Add(new EvalExpr_Modifier(elem.value as MultiItem));
            }

            _Result = () =>
            {
                return baseValue.Result() + modifiers.Select(x => x.Result()).Sum();
            };
        }

        public EvalExpr_ModifierGroup()
        {
        }
    }

    public class EvalExpr_Modifier : Eval<double>
    {
        internal EvalExpr_SINGLE<double> value;
        internal EvalExpr_Condition condition;

        public EvalExpr_Modifier(MultiItem modItem)
        {
            var currValue = modItem.Find<SingleValue>("value");
            value = new EvalExpr_SINGLE<double>(currValue, EvalExpr_SINGLE<double>.OPType.READ);

            var modCondition = modItem.Find<MultiItem>("condition");
            if(modCondition.elems.Count() != 1)
            {
                throw new Exception();
            }

            condition = EvalExpr_Condition.Parse(modCondition.elems[0]);
        }
    }

    public class EvalExpr_MultiValue : Eval<object[]>
    {
        public static EvalExpr_MultiValue Parse(MultiItem multiItem, string name, object[] defValue)
        {
            var multiValue = multiItem.TryFind<MultiValue>(name);
            if (multiValue == null)
            {
                if (defValue != null)
                {
                    return new EvalExpr_MultiValue(defValue);
                }
                throw new Exception();
            }

            return new EvalExpr_MultiValue(multiValue);
        }

        public EvalExpr_MultiValue(object[] defValue) 
        {
            defaultValue = defValue;
        }

        public EvalExpr_MultiValue(MultiValue multiValue)
        {
            expr_singleList = multiValue.elems.Select(x => new EvalExpr_SINGLE<object>(x, EvalExpr_SINGLE<object>.OPType.READ)).ToList();
            _Result = () =>
            {
                return expr_singleList.Select(x => x.Result()).ToArray();
            };
        }

        List<EvalExpr_SINGLE<object>> expr_singleList;
    }

    public class EvalExpr_Condition : Eval<bool>
    { 
        public static EvalExpr_Condition Parse(MultiItem multiItem, string name, bool? defValue)
        {
            var itemValue = multiItem.TryFind<MultiItem>(name);
            if(itemValue == null)
            {
                if(defValue != null)
                {
                    return new EvalExpr_Condition() { defaultValue = defValue.Value };
                }
                throw new Exception();
            }

            return Parse(itemValue);
        }

        public static EvalExpr_Condition Parse(Value modValue)
        {
            if(modValue is Item)
            {
                var item = modValue as Item;
                if (item.key == "is.equal")
                {
                    return new EvalExpr_EUQAL(item.value as MultiValue);
                }

                throw new Exception("not support " + item.key);
            }
            else if(modValue is SingleValue)
            {
                var single = modValue as SingleValue;
                return new EvalExpr_Condition() { defaultValue = bool.Parse(single.value) };
            }

            throw new Exception("not support type" + modValue.GetType().Name);
        }
    }

    public class EvalExpr_EUQAL : EvalExpr_Condition
    {
        public EvalExpr_EUQAL(MultiValue multiItem)
        {
            this.modValue = multiItem;

            var elements = multiItem.elems;
            if (elements.Count() != 2)
            {
                throw new Exception("'EvalExpr_Equal' value expect have 2 elements but curr is " + elements.Count());
            }

            left = new EvalExpr_SINGLE<object>(elements[0], EvalExpr_SINGLE<object>.OPType.READ);
            right = new EvalExpr_SINGLE<object>(elements[1], EvalExpr_SINGLE<object>.OPType.READ);

            _Result = () =>
            {
                var l = left.Result();
                var r = right.Result();
                if (l.GetType() != r.GetType())
                {
                    throw new Exception($"EvalExpr_Equal left type is {l.GetType().Name}, but right type is {r.GetType().Name}");
                }

                return  (l.Equals(r));
            };
        }

        private EvalExpr_SINGLE<object> left;
        private EvalExpr_SINGLE<object> right;
    }

    public class EvalExpr_SINGLE<T> : Eval<T>
    {
        //public static EvalExpr_SINGLE<T> ParseRead(string name, MultiItem multiItem)
        //{
        //    var itemValue = multiItem.Find<SingleValue>(name);
        //    return new EvalExpr_SINGLE<T>(itemValue, OPType.READ);
        //}

        //public static EvalExpr_SINGLE<T> ParseRead(string name, MultiItem multiItem, T defaultValue)
        //{
        //    var itemValue = multiItem.TryFind<SingleValue>(name);
        //    if(itemValue == null)
        //    {
        //        return new EvalExpr_SINGLE<T>(defaultValue);
        //    }

        //    return new EvalExpr_SINGLE<T>(itemValue, OPType.READ);
        //}

        public EvalExpr_SINGLE(SingleValue value, OPType optype)
        {
            this.modValue = value;
            if(((int)optype & (int)OPType.READ) != 0)
            {
                getter = new Getter(value.value);

                _Result = () =>
                {
                    return (T)getter.get();
                };
            }
            if(((int)optype & (int)OPType.WRITE) != 0)
            {
                setter = new Setter(value.value);
            }
        }

        public enum OPType
        {
            READ = 0x01,
            WRITE = 0x10,
            READ_WRITE = (int)READ | (int)WRITE
        }

        public Getter getter;
        public Setter setter;
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
