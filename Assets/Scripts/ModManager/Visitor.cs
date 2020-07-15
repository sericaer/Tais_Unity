using System;
using System.Reflection;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

namespace TaisEngine.ModManager
{
    internal class Visitor
    {
        //internal static Dictionary<string, FieldInfo> dictCommon;
        //internal static Dictionary<string, FieldInfo> dictDepart;
        //internal static Dictionary<string, FieldInfo> dictInit;

        //internal static object commonObj;
        //internal static object departObj;
        internal class Element
        {
            internal object obj;
            internal Dictionary<string, FieldInfo[] > dict;

            internal Element(Type type, string rootKey)
            {
                FieldInfo[] root = { };
                dict = AnaylizeVisitPropery(type, root, rootKey);
            }

            private Dictionary<string, FieldInfo[] > AnaylizeVisitPropery(Type type, FieldInfo[] parent, string rootKey)
            {
                var rslt = new Dictionary<string, FieldInfo[] >();

                var fields = type.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance);

                foreach (var field in fields)
                {
                    var visitPropery = (VisitPropery)Attribute.GetCustomAttribute(field, typeof(VisitPropery));
                    if (visitPropery == null)
                    {
                        continue;
                    }

                    var list = new List<FieldInfo>();
                    list.AddRange(parent);
                    list.Add(field);

                    if (visitPropery.key != null)
                    {
                        //if(!visitPropery.key.StartsWith(rootKey))
                        //{
                        //    throw new Exception($"{visitPropery.key} must start with {rootKey}");
                        //}

                        rslt.Add(visitPropery.key, list.ToArray());
                    }
                    else
                    {
                        var subDict = AnaylizeVisitPropery(field.FieldType, list.ToArray(), rootKey);
                        foreach(var set in subDict)
                        {
                            rslt.Add(set.Key, set.Value);
                        }
                    }
                }

                return rslt;
            }

            internal object GetValue(string raw)
            {
                object rslt = obj;
                foreach (var field in dict[raw])
                {
                    rslt = field.GetValue(rslt);
                }

                return rslt;
            }

            internal void SetValue<T>(string raw, T value)
            {
                var rslt = obj;
                for (int i=0; i<dict[raw].Count()-1; i++)
                {
                    rslt = dict[raw][i].GetValue(rslt);
                }

                dict[raw].Last().SetValue(rslt, value);
            }

            internal Type GetFieldType(string raw)
            {
                if(!dict.ContainsKey(raw))
                {
                    throw new Exception($"can not find '{raw}' in data visitor");
                }

                return dict[raw].Last().FieldType;
            }
        }

        internal static Dictionary<string, Element> dictRoot = new Dictionary<string, Element>();

        internal enum VType
        {
            READ = 0x01,
            WRITE = 0x10
        }

        internal static void InitReflect(string rootKey, Type type)
        {
            dictRoot.Add(rootKey, new Element(type, rootKey));
        }

        internal static void SetObj(string key, object obj)
        {
            dictRoot[key].obj = obj;
        }

        internal static void TryParse(string key, ref object staticReadValue, Visitor.VType vType)
        {
            if(vType == VType.READ)
            {
                var elem = GetElement(key);
                if (elem != null)
                {
                    return;
                }

                if (TryParseDigitCalc(key, ref staticReadValue))
                {
                    return;
                }
                
                if (key == "true")
                {
                    staticReadValue = true;
                    return;
                }
                if (key == "false")
                {
                    staticReadValue = false;
                    return;
                }

                if(key.Contains('.'))
                {
                    throw new Exception($"can not find '{key}' in data visitor");
                }

                staticReadValue = key;
                return;

            }

            if (vType == VType.WRITE)
            {
                if (GetElement(key) == null)
                {
                    throw new Exception($"'{key}' not support write");
                }
            }
        }

        internal static Type GetValueType(string raw)
        {
            var elem = GetElement(raw);
            return elem.GetFieldType(raw);
        }

        internal static void Write<T>(string raw, T value)
        {
            var elem = GetElement(raw);
            elem.SetValue(raw, value);
        }

        internal static T Read<T>(string raw)
        {
            var elem = GetElement(raw);
            return (T)elem.GetValue(raw);
        }

        private static Element GetElement(string key)
        {
            var rslt = Regex.Match(key, @"^[A-Za-z]?[A-Za-z0-9_]*\.");
            if (!rslt.Success)
            {
                return null;
            }

            var rootkey = rslt.Value.TrimEnd('.');
            return dictRoot.ContainsKey(rootkey)? dictRoot[rootkey] : dictRoot["common"];
        }

        private static bool TryParseDigitCalc(string script, ref object obj)
        {
            var convert = script.Replace(" ", "");

            var rslt = Regex.Match(convert, @"^[\+\-]?[0-9]+(\.?[0-9]+)*([\+\-\*/]?[0-9]+(\.?[0-9]+)*)*");
            if (!rslt.Success)
            {
                return false;
            }
            if (rslt.Length != convert.Length)
            {
                return false;
            }

            double value = 0.0;

            int start = 0;
            while (start < convert.Length)
            {
                if (start == 0)
                {
                    var matched_head = Regex.Match(convert.Substring(start), @"^[\+\-]?[0-9]+(\.?[0-9]+)*");
                    if (!matched_head.Success)
                    {
                        return false;
                    }

                    value = double.Parse(matched_head.Value);
                    start += matched_head.Length;
                    continue;
                }

                var matched = Regex.Match(convert.Substring(start), @"^[\+\-\*/][0-9]+(\.?[0-9]+)*");
                if (!matched.Success)
                {
                    return false;
                }

                if (matched.Value.StartsWith("+"))
                {
                    value += double.Parse(matched.Value.Replace("+", ""));
                }
                if (matched.Value.StartsWith("-"))
                {
                    value -= double.Parse(matched.Value.Replace("-", ""));
                }
                if (matched.Value.StartsWith("*"))
                {
                    value *= double.Parse(matched.Value.Replace("*", ""));
                }
                if (matched.Value.StartsWith("/"))
                {
                    value /= double.Parse(matched.Value.Replace("/", ""));
                }
            }

            obj = value;
            return true;
        }
    }

    [AttributeUsage(AttributeTargets.Field)]
    internal class VisitPropery : System.Attribute
    {
        internal VisitPropery()
        {

        }

        internal VisitPropery(string key)
        {
            this.key = key;
        }


        internal VisitPropery(string key, object ext)
        {
            this.key = key;
        }

        internal string key;
        internal Visitor.VType vType;
        internal object ext;
    }
}
