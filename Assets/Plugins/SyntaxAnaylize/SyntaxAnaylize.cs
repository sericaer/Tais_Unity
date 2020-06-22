﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SyntaxAnaylize
{
    public class Syntax
    {
        static int line;

        public static MultiItem Anaylize(string filename, string script)
        {

            try
            {
                MultiItem rslt = new MultiItem();

                int charIndex = 0;

                var trim = Regex.Replace(script, @"[\s]+$", "");

                line = 1;

                while (charIndex < trim.Length)
                {
                    int offset;
                    rslt.AddRange(Anaylize(trim.Substring(charIndex), out offset).Select(x => x as Item));
                    charIndex += offset;
                }

                return rslt;
            }
            catch (Exception e)
            {
                throw new Exception($"exception in {filename}:{line}", e);
            }
        }

        internal static IEnumerable<Value> Anaylize(string script, out int offset)
        {
            int charIndex = 0;

            List<Item> multiItem = null;
            List<SingleValue> multiValue = null;

            string key = "";

            while (true)
            {
                int endIndex;
                ELEM_TYPE elemType = Match(script, charIndex, out endIndex);
                switch (elemType)
                {
                    case ELEM_TYPE.CR:
                        line++;
                        break;
                    case ELEM_TYPE.SPACE:
                        break;
                    case ELEM_TYPE.STRING:
                        if (key == "")
                        {
                            key = script.Substring(charIndex, endIndex - charIndex);
                        }
                        else
                        {
                            var value = script.Substring(charIndex, endIndex - charIndex);

                            if (multiItem != null)
                            {
                                multiItem.Add(new Item(key, value));
                            }
                            if (multiValue != null)
                            {
                                multiValue.Add(new SingleValue(key));
                                multiValue.Add(new SingleValue(value));
                            }
                            key = "";
                        }
                        break;
                    case ELEM_TYPE.ASSIGN:
                        if (key == "")
                        {
                            throw new Exception();
                        }
                        if (multiValue != null)
                        {
                            throw new Exception();
                        }
                        if (multiItem == null)
                        {
                            multiItem = new List<Item>();
                        }
                        break;
                    case ELEM_TYPE.BRACE_OPEN:
                        {
                            int offset_sub;
                            IEnumerable<Value> sub = Anaylize(script.Substring(endIndex), out offset_sub);
                            charIndex = endIndex + offset_sub;

                            multiItem.Add(new Item(key, sub.Select(x => x as Value)));

                            key = "";

                            continue;
                        }
                    case ELEM_TYPE.BRACE_CLOSE:
                    case ELEM_TYPE.PARSE_END:
                        {
                            offset = endIndex;

                            if (multiItem != null && key == "")
                            {
                                return multiItem;
                            }

                            if (multiValue == null && key != "")
                            {
                                multiValue = new List<SingleValue>();
                                multiValue.Add(new SingleValue(key));

                                return multiValue;
                            }

                            if(multiValue != null && key == "")
                            {
                                return multiValue;
                            }

                            throw new Exception();
                        }
                    case ELEM_TYPE.COMMA:
                        {
                            if (key == "")
                            {
                                throw new Exception();
                            }
                            if (multiItem != null)
                            {
                                throw new Exception();
                            }
                            if (multiValue == null)
                            {
                                multiValue = new List<SingleValue>();
                            }
                        }
                        break;
                    default:
                        break;
                }

                charIndex = endIndex;
            }
            
        }

        private static ELEM_TYPE Match(string script, int charIndex, out int endIndex)
        {
            var curr = script.Substring(charIndex);
            if(curr == "")
            {
                endIndex = charIndex;
                return ELEM_TYPE.PARSE_END;
            }

            if (curr[0] == '\n')
            {
                endIndex = charIndex + 1;
                return ELEM_TYPE.CR;
            }

            var rslt = Regex.Match(curr, @"^[ \f\r\t\v]+");
            if (rslt.Success)
            {
                endIndex = charIndex + rslt.Length;
                return ELEM_TYPE.SPACE;
            }

            rslt = Regex.Match(curr, @"^=");
            if (rslt.Success)
            {
                endIndex = charIndex + rslt.Length;
                return ELEM_TYPE.ASSIGN;
            }

            rslt = Regex.Match(curr, @"^{");
            if (rslt.Success)
            {
                endIndex = charIndex + rslt.Length;
                return ELEM_TYPE.BRACE_OPEN;
            }

            rslt = Regex.Match(curr, @"^,");
            if (rslt.Success)
            {
                endIndex = charIndex + rslt.Length;
                return ELEM_TYPE.COMMA;
            }

            rslt = Regex.Match(curr, @"^}");
            if (rslt.Success)
            {
                endIndex = charIndex + rslt.Length;
                return ELEM_TYPE.BRACE_CLOSE;
            }

            //数字表达式
            rslt = Regex.Match(curr, @"^[\+\-]?[0-9]+\.?[0-9]+([ \f\r\t\v]*[\+\-\*/]?[ \f\r\t\v]*[0-9]+\.?[0-9]+[ \f\r\t\v]*)*");
            if (rslt.Success)
            {
                endIndex = charIndex + rslt.Length;
                return ELEM_TYPE.STRING;
            }

            //标识符开始表达式
            rslt = Regex.Match(curr, @"^[A-Za-z]+[A-Za-z0-9_]*(\.?[A-Za-z]+[A-Za-z0-9_])*([ \f\r\t\v]*[\+\-\*/]?[ \f\r\t\v]*[\+\-]?[0-9]+(\.?[0-9]+)?[ \f\r\t\v]*)*");
            if (rslt.Success)
            {
                endIndex = charIndex + rslt.Length;
                return ELEM_TYPE.STRING;
            }

            //数字开始表达式
            rslt = Regex.Match(curr, @"^[\+\-]?[0-9]+(\.?[0-9]+)?([ \f\r\t\v]*[\+\-\*/]?[ \f\r\t\v]*[A-Za-z]+[A-Za-z0-9_]*(\.?[A-Za-z]+[A-Za-z0-9_])+[ \f\r\t\v]*)*");
            if (rslt.Success)
            {
                endIndex = charIndex + rslt.Length;
                return ELEM_TYPE.STRING;
            }

            throw new Exception();
        }

        enum ELEM_TYPE
        {
            CR,
            ASSIGN,
            BRACE_OPEN,
            BRACE_CLOSE,
            STRING,
            SPACE,
            COMMA,
            PARSE_END
        }
    }

    public class Value
    {

    }

    public class Item : Value
    {
        public readonly string key;
        public readonly Value value;

        public Item(string key, string value)
        {
            this.key = key;
            this.value = new SingleValue(value);
        }

        public Item(string key, IEnumerable<Value> value)
        {
            this.key = key;

            if (value.All(x => x is Item))
            {
                this.value = new MultiItem(value.Select(x => x as Item));
            }
            else if (value.All(x => x is SingleValue))
            {
                this.value = new MultiValue(value.Select(x => x as SingleValue));
            }
            else
            {
                throw new Exception();
            }
        }

        public override string ToString()
        {

            return $"{key} = {value}";
        }
    }

    public class MultiItem : Value
    {
       public readonly List<Item> elems;

        public MultiItem()
        {
            this.elems = new List<Item>();
        }

        public MultiItem(IEnumerable<Item> value)
        {
            this.elems = value.ToList();
        }

        public object Get(string name, Type fieldType)
        {
            var item = elems.SingleOrDefault(x => x.key == name);
            if(item == null)
            {
                throw new Exception($"can not find '{name}'");
            }

            if(fieldType == typeof(bool))
            {
                return bool.Parse((item.value as SingleValue).ToString());
            }

            throw new Exception();
        }

        public override string ToString()
        {
            return "\n{\n    " + string.Join("\n", elems.Select(x => x.ToString())).Replace("\n", "\n    ") + "\n}";
        }

        internal void AddRange(IEnumerable<Item> list)
        {
            elems.AddRange(list);
        }
    }

    public class SingleValue : Value
    {
        public SingleValue(string value)
        {
            this.value = value;
        }

        public override string ToString()
        {
            return value;
        }

        public readonly string value;
    }

    public class MultiValue : Value
    {
        public MultiValue(IEnumerable<SingleValue> value)
        {
            this.elems = value.ToList();
        }

        public readonly List<SingleValue> elems;

        public override string ToString()
        {
            return "{" + string.Join(", ", elems.Select(x => x.ToString())) + "}";
        }
    }
}