using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace TaisEngine.ModManager
{
    internal class Visitor
    {
        internal static Dictionary<string, (Func<object> reader, Action<object> write, Type dataType)> dictMethod;

        static Visitor()
        {
            dictMethod = new Dictionary<string, (Func<object> reader, Action<object> write, Type dataType)>();
        }

        internal enum VType
        {
            READ = 0x01,
            WRITE = 0x10
        }

        internal static void Add(string key, Func<object> reader, Action<object> write, Type dataType)
        {
            dictMethod.Add(key, (reader, write, dataType));
        }

        internal static void TryParse(string key, ref object staticReadValue, Visitor.VType vType)
        {
            if(vType == VType.READ)
            {
                if (dictMethod.ContainsKey(key))
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

                if (!key.StartsWith("gm.") && !key.StartsWith("init."))
                {
                    staticReadValue = key;
                    return;
                }

                throw new Exception($"can not find {key} in data visitor");
            }

            if (vType == VType.WRITE)
            {
                if (!dictMethod.ContainsKey(key) || dictMethod[key].write == null)
                {
                    throw new Exception($"{key} not support write");
                }
            }
        }

        internal static Type GetValueType(string raw)
        {
            return dictMethod[raw].dataType;
        }

        internal static void Write<T>(string raw, T obj)
        {
            dictMethod[raw].write(obj);
        }

        internal static T Read<T>(string raw)
        {
            return (T)dictMethod[raw].reader();
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

    //[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    //internal class VisitPropery : System.Attribute
    //{
    //    internal VisitPropery(string key, Visitor.VType vType)
    //    {

    //    }
    //}
}
