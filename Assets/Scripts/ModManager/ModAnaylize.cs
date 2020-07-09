using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using SyntaxAnaylize;

namespace TaisEngine.ModManager
{
    internal class ModAnaylize
    {
        internal static MethodInfo ReflParseValueType;
        internal static MethodInfo ReflParse;

        static ModAnaylize()
        {
            Type type = typeof(ModAnaylize);
            ReflParseValueType = type.GetMethod(nameof(ModAnaylize.ParseValueType), BindingFlags.Static | BindingFlags.NonPublic);
            ReflParse = type.GetMethod(nameof(ModAnaylize.Parse), BindingFlags.Static | BindingFlags.NonPublic);
        }

        internal static T Parse<T>(Value value) where T : new()
        {
            var multiItem = value as MultiItem;
            if (multiItem == null)
            {
                throw new Exception();
            }

            T rslt = new T();

            var type = typeof(T);
            var fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            foreach (var field in fields)
            {
                var modPropery = (ModProperty)Attribute.GetCustomAttribute(field, typeof(ModProperty));
                if (modPropery == null)
                {
                    continue;
                }

                if (modPropery is ModPropertyList)
                {
                    dynamic list = AnaylizeModPropertyList(multiItem, field, modPropery);

                    field.SetValue(rslt, list);
                    continue;
                }

                var modValue = multiItem.TryFind(modPropery.key);
                if (modValue == null)
                {
                    continue;
                }

                if (field.FieldType.IsValueType && Nullable.GetUnderlyingType(field.FieldType) == null)
                {
                    throw new Exception();
                }

                object obj = AnaylizeSubValue(field.FieldType, modValue);

                field.SetValue(rslt, obj);
            }

            return rslt;
        }

        private static dynamic AnaylizeSubValue(Type type, Value modValue)
        {

            if (type.IsValueType || type == typeof(string))
            {
                return AnaylizeModValueType(modValue, type);
            }
            else
            {
                return AnaylizeModClassType(modValue, type);
            }

        }

        private static dynamic AnaylizeModClassType(Value modValue, Type type)
        {

            if (type == typeof(Expr_OperationGroup))
            {
                return new Expr_OperationGroup(modValue);
            }
            if (type == typeof(Expr_MultiValue))
            {
                return new Expr_MultiValue(modValue as MultiValue);
            }
            if (type == typeof(Expr_Condition))
            {
                return Expr_Condition.Parse(modValue);
            }
            if (type == typeof(Expr_ModifierGroup))
            {
                return new Expr_ModifierGroup(modValue as MultiItem);
            }
            if (type == typeof(Expr_EffectGroup))
            {
                return new Expr_EffectGroup(modValue);
            }

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Dictionary<,>))
            {
                return AnaylizeModDictionary(modValue as MultiItem, type);
            }

            var miConstructed = ReflParse.MakeGenericMethod(type);

            object[] args = { modValue };
            return miConstructed.Invoke(null, args);
        }

        private static object AnaylizeModDictionary(MultiItem multiItem, Type type)
        {
            Type[] listParameters = type.GetGenericArguments();
            if(listParameters[0] != typeof(string))
            {
                throw new Exception();
            }

            dynamic dict = Activator.CreateInstance(type);

            foreach (var subValue in multiItem.elems)
            {
                var item = (Item)subValue;
                if (item == null)
                {
                    throw new Exception();
                }

                dict.Add(item.key, AnaylizeSubValue(listParameters[1], item.value));
            }

            return dict;
        }

        private static dynamic AnaylizeModValueType(Value modValue, Type type)
        {

            var miConstructed = ReflParseValueType.MakeGenericMethod(type);

            object[] args = { modValue };
            return miConstructed.Invoke(null, args);
        }

        private static dynamic AnaylizeModPropertyList(MultiItem multiItem, FieldInfo field, ModProperty modPropery)
        {
            dynamic list = Activator.CreateInstance(field.FieldType);

            Type[] listParameters = field.FieldType.GetGenericArguments();

            var miConstructed = ReflParse.MakeGenericMethod(listParameters[0]);
            foreach (var subValue in multiItem.TryFinds(modPropery.key))
            {
                object[] args = { subValue };
                list.Add((dynamic)miConstructed.Invoke(null, args));
            }

            return list;
        }

        internal static T ParseValueType<T>(Value modvalue) 
        {
            var singleValue = modvalue as SingleValue;
            if (singleValue == null)
            {
                throw new Exception();
            }

            var factor = new Factor<T>(singleValue, Visitor.VType.READ);
            if(factor.staticReadValue == null)
            {
                throw new Exception();
            }

            
            return factor.Read();
        }
    }

    [AttributeUsage(AttributeTargets.Field)]
    internal class ModProperty : Attribute
    {
        internal ModProperty()
        {

        }

        internal ModProperty(string key)
        {
            this.key = key;
        }


        internal ModProperty(string key, object ext)
        {
            this.key = key;
        }

        internal string key;
        internal Visitor.VType vType;
        internal object ext;
    }

    [AttributeUsage(AttributeTargets.Field)]
    internal class ModPropertyList : ModProperty
    {
        internal ModPropertyList(string key) : base(key)
        {
        }
    }
}
