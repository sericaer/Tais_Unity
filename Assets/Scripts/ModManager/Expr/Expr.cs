using System;
using SyntaxAnaylize;

namespace TaisEngine.ModManager
{
    internal abstract class Expr<T>
    {
        internal Expr(Value value)
        {
            modValue = value;
        }

        internal T Result()
        {
            if(defaultValue != null)
            {
                return (T)defaultValue;
            }

            return ResultImp();
        }

        internal abstract T ResultImp();

        internal object defaultValue;
        internal Value modValue;
    }

    internal class ExprDefault<T> : Expr<T>
    {
        internal ExprDefault(T defValue) : base(null)
        {
            defaultValue = defValue;
        }

        internal override T ResultImp()
        {
            throw new Exception();
        }
    }

    //public class EvalExpr_ModifierGroup : Eval<double>
    //{
    //    public static EvalExpr_ModifierGroup Parse(MultiItem modItems, string key, double? defVale)
    //    {
    //        var find = modItems.TryFind<MultiItem>(key);
    //        if(find == null)
    //        {
    //            if(defVale == null)
    //            {
    //                throw new Exception($"can not find {key} in {modItems}");
    //            }

    //            return new EvalExpr_ModifierGroup() { defaultValue = defVale.Value };
    //        }

    //        return new EvalExpr_ModifierGroup(find);
    //    }


    //    internal EvalExpr_SINGLE<double> baseValue;
    //    internal List<EvalExpr_Modifier> modifiers = new List<EvalExpr_Modifier>();

    //    public EvalExpr_ModifierGroup(MultiItem modItem)
    //    {
    //        var modBaseValue = modItem.Find<SingleValue>("base_value");

    //        baseValue = new EvalExpr_SINGLE<double>(modBaseValue, EvalExpr_SINGLE<double>.OPType.READ);
    //        foreach(var elem in modItem.elems.Where(x=>x.key == "modifier"))
    //        {
    //            modifiers.Add(new EvalExpr_Modifier(elem.value as MultiItem));
    //        }

    //        _Result = () =>
    //        {
    //            return baseValue.Result() + modifiers.Select(x => x.Result()).Sum();
    //        };
    //    }

    //    public EvalExpr_ModifierGroup()
    //    {
    //    }
    //}

    //public class EvalExpr_Modifier : Eval<double>
    //{
    //    internal EvalExpr_SINGLE<double> value;
    //    internal EvalExpr_Condition condition;

    //    public EvalExpr_Modifier(MultiItem modItem)
    //    {
    //        var currValue = modItem.Find<SingleValue>("value");
    //        value = new EvalExpr_SINGLE<double>(currValue, EvalExpr_SINGLE<double>.OPType.READ);

    //        var modCondition = modItem.Find<MultiItem>("condition");
    //        if(modCondition.elems.Count() != 1)
    //        {
    //            throw new Exception();
    //        }

    //        condition = EvalExpr_Condition.Parse(modCondition.elems[0]);
    //    }
    //}

    //public class EvalExpr_MultiValue : Eval<object[]>
    //{
    //    public static EvalExpr_MultiValue Parse(MultiItem multiItem, string name, object[] defValue)
    //    {
    //        var multiValue = multiItem.TryFind<MultiValue>(name);
    //        if (multiValue == null)
    //        {
    //            if (defValue != null)
    //            {
    //                return new EvalExpr_MultiValue(defValue);
    //            }
    //            throw new Exception();
    //        }

    //        return new EvalExpr_MultiValue(multiValue);
    //    }

    //    public EvalExpr_MultiValue(object[] defValue) 
    //    {
    //        defaultValue = defValue;
    //    }

    //    public EvalExpr_MultiValue(MultiValue multiValue)
    //    {
    //        expr_singleList = multiValue.elems.Select(x => new EvalExpr_SINGLE<object>(x, EvalExpr_SINGLE<object>.OPType.READ)).ToList();
    //        _Result = () =>
    //        {
    //            return expr_singleList.Select(x => x.Result()).ToArray();
    //        };
    //    }

    //    List<EvalExpr_SINGLE<object>> expr_singleList;
    //}

    //public class EvalExpr_Condition : Eval<bool>
    //{ 
    //    public static EvalExpr_Condition Parse(MultiItem multiItem, string name, bool? defValue)
    //    {
    //        var modValue = multiItem.TryFind(name);
    //        if(modValue == null)
    //        {
    //            if(defValue != null)
    //            {
    //                return new EvalExpr_Condition() { defaultValue = defValue.Value };
    //            }
    //            throw new Exception();
    //        }

    //        if (modValue is MultiItem)
    //        {
    //            var items = modValue as MultiItem;
    //            if (items.elems.Count() != 1)
    //            {
    //                throw new Exception($"EvalExpr_Condition should only 1 element, but curr is {items.elems.Count()}");
    //            }

    //            return Parse(items.elems[0]);
    //        }
    //        if (modValue is SingleValue)
    //        {
    //            return Parse(modValue as SingleValue);
    //        }

    //        throw new Exception($"EvalExpr_Condition not support {modValue} ");
    //    }

    //    public static EvalExpr_Condition Parse(SingleValue modValue)
    //    {
    //        return new EvalExpr_Condition_Single(modValue);
    //    }

    //    public static EvalExpr_Condition Parse(Item modValue)
    //    {
    //        if(modValue.key == "is.equal")
    //        {
    //            return new EvalExpr_EUQAL(modValue.value as MultiValue);
    //        }
    //        throw new Exception();
    //    }
    //}

    //public class EvalExpr_Condition_Single : EvalExpr_Condition
    //{
    //    public EvalExpr_Condition_Single(SingleValue modValue)
    //    {
    //        singleExpr = new EvalExpr_SINGLE<bool>(modValue, EvalExpr_SINGLE<bool>.OPType.READ);

    //        _Result = () =>
    //         {
    //             return (bool)singleExpr.getter.get();
    //         };
    //    }

    //    EvalExpr_SINGLE<bool> singleExpr;
    //}

    //public class EvalExpr_EUQAL : EvalExpr_Condition
    //{
    //    public EvalExpr_EUQAL(MultiValue multiItem)
    //    {
    //        this.modValue = multiItem;

    //        var elements = multiItem.elems;
    //        if (elements.Count() != 2)
    //        {
    //            throw new Exception("'EvalExpr_Equal' value expect have 2 elements but curr is " + elements.Count());
    //        }

    //        left = new EvalExpr_SINGLE<object>(elements[0], EvalExpr_SINGLE<object>.OPType.READ);
    //        right = new EvalExpr_SINGLE<object>(elements[1], EvalExpr_SINGLE<object>.OPType.READ);

    //        _Result = () =>
    //        {
    //            dynamic l = left.Result();
    //            dynamic r = right.Result();

    //            bool rslt = l == r;
    //            return rslt;
    //        };
    //    }

    //    private EvalExpr_SINGLE<object> left;
    //    private EvalExpr_SINGLE<object> right;
    //}

    //public class EvalExpr_SINGLE<T> : Eval<T>
    //{
    //    //public static EvalExpr_SINGLE<T> ParseRead(string name, MultiItem multiItem)
    //    //{
    //    //    var itemValue = multiItem.Find<SingleValue>(name);
    //    //    return new EvalExpr_SINGLE<T>(itemValue, OPType.READ);
    //    //}

    //    //public static EvalExpr_SINGLE<T> ParseRead(string name, MultiItem multiItem, T defaultValue)
    //    //{
    //    //    var itemValue = multiItem.TryFind<SingleValue>(name);
    //    //    if(itemValue == null)
    //    //    {
    //    //        return new EvalExpr_SINGLE<T>(defaultValue);
    //    //    }

    //    //    return new EvalExpr_SINGLE<T>(itemValue, OPType.READ);
    //    //}

    //    public EvalExpr_SINGLE(SingleValue value, OPType optype)
    //    {
    //        this.modValue = value;
    //        if(((int)optype & (int)OPType.READ) != 0)
    //        {
    //            getter = new Getter(value.value);

    //            _Result = () =>
    //            {
    //                return (T)getter.get();
    //            };
    //        }
    //        if(((int)optype & (int)OPType.WRITE) != 0)
    //        {
    //            setter = new Setter(value.value);
    //        }
    //    }

    //    public enum OPType
    //    {
    //        READ = 0x01,
    //        WRITE = 0x10,
    //        READ_WRITE = (int)READ | (int)WRITE
    //    }

    //    public Getter getter;
    //    public Setter setter;
    //}

    ////public abstract class EvalExpr_And : EvalExpr<bool>
    ////{
    ////    public override bool Result()
    ////    {
    ////        return !evals.Any(x => !((bool)x.Result()));
    ////    }
    ////}

    //public class Converter
    //{
    //    public static void Convert(string raw, out bool ret)
    //    {
    //        if (raw == "true")
    //        {
    //            ret = true;
    //        }
    //        else if (raw == "false")
    //        {
    //            ret = false;
    //        }
    //        else
    //        {
    //            throw new Exception($"{raw} must be 'true' or 'false'");
    //        }
    //    }


    //    public static Func<T> GetFunc<T>(string value)
    //    {
    //        Type[] paramTypes = { typeof(string), typeof(T).MakeByRefType() };

    //        var method = typeof(Converter).GetMethod("Convert", BindingFlags.Static | BindingFlags.Public, null, paramTypes, null);
    //        if (method == null)
    //        {
    //            throw new Exception();
    //        }

    //        return () =>
    //        {
    //            var args = new object[] { value, null };
    //            method.Invoke(null, args);
    //            return (T)args[1];
    //        };
    //    }
    //}
}
