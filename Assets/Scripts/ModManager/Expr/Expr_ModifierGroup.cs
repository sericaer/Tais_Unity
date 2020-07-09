using System;
using System.Collections.Generic;
using System.Linq;
using SyntaxAnaylize;

namespace TaisEngine.ModManager
{
    internal class Expr_ModifierGroup : Expr<double>
    {
        internal static Expr_ModifierGroup Parse(SyntaxMod.ModItem modElem, string name, double? defValue)
        {
            try
            {
                var modValue = modElem.multiItem.TryFind<MultiItem>(name);
                if (modValue == null)
                {
                    if (defValue != null)
                    {
                        return new Expr_ModifierGroup(defValue.Value);
                    }

                    throw new Expr_Exception($"can not find {name}", modElem.multiItem);
                }

                return new Expr_ModifierGroup(modValue);
            }
            catch(Exception e)
            {
                throw new Exception($"parse file faild! {modElem.filePath}", e);
            }
        }

        internal Expr_ModifierGroup(double defValue) : base(null)
        {
            defaultValue = defValue;
        }

        internal Expr_ModifierGroup(MultiItem multiItem) : base(multiItem)
        {
            var baseItem = multiItem.TryFind<SingleValue>("base");
            if (baseItem != null)
            {
                baseValue = new Factor<double>(baseItem, Visitor.VType.READ);
            }

            modifiers = Expr_Modifier.ParseArray(multiItem, "modifier");
         }

        internal override double ResultImp()
        {
            double rslt = 0.0;
            if(baseValue != null)
            {
                rslt += baseValue.Read();
            }

            rslt += modifiers.Where(x => x.valid).Sum(x => x.value.Read());
            return rslt;
        }

        internal Factor<double> baseValue;
        internal Expr_Modifier[] modifiers;
    }

    internal class Expr_Modifier
    {
        internal Factor<double> value;
        internal Expr<bool> condition;

        internal Expr_Modifier(Value modValue)
        {
            if(!(modValue is MultiItem))
            {
                throw new Expr_Exception("modifier must be MultiItem", modValue);
            }

            var multiItem = modValue as MultiItem;
            var valueItem = multiItem.TryFind("value");
            if(valueItem == null)
            {
                throw new Expr_Exception("modifier must have 'value' item", modValue);
            }
            if(!(valueItem is SingleValue))
            {
                throw new Expr_Exception("modifier value item must be Single", modValue);
            }

            value = new Factor<double>(valueItem as SingleValue, Visitor.VType.READ);

            var condRaw = multiItem.TryFind("cond");
            if (condRaw != null)
            {
                condition = Expr_Condition.Parse(condRaw);
            }
            
        }

        internal bool valid
        {
            get
            {
                return condition.Result();
            }
        }

        internal static Expr_Modifier[] ParseArray(MultiItem multiItem, string name)
        {
            var rslt = new List<Expr_Modifier>();

            var rawElems = multiItem.elems.Where(x => x.key == name).ToArray();
            for (int i = 0; i < rawElems.Count(); i++)
            {
                rslt.Add(new Expr_Modifier(rawElems[i].value));
            }

            return rslt.ToArray();
        }
    }
}
