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
            var fields = type.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            foreach (var field in fields)
            {
                var modPropery = (ModProperty)Attribute.GetCustomAttribute(field, typeof(ModProperty));
                if (modPropery == null)
                {
                    continue;
                }

                var modValue = multiItem.TryFind(modPropery.key);
                if(modValue == null)
                {
                    continue;
                }

                if (field.FieldType.IsValueType)
                {
                    if(Nullable.GetUnderlyingType(field.FieldType) == null)
                    {
                        throw new Exception();
                    }

                    var miConstructed = ReflParseValueType.MakeGenericMethod(field.FieldType);

                    object[] args = { modValue };
                    field.SetValue(rslt, miConstructed.Invoke(null, args));
                }
                else
                {
                    var miConstructed = ReflParse.MakeGenericMethod(field.FieldType);

                    object[] args = { modValue };
                    field.SetValue(rslt, miConstructed.Invoke(null, args));
                }
            }

            return rslt;
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
    internal class ModProperty : System.Attribute
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
}
