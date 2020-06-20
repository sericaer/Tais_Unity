using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SyntaxAnaylize
{
    internal class Syntax
    {
        internal static MultiItem Anaylize(string script)
        {
            MultiItem rslt = new MultiItem();

            string key = "";

            int charIndex = 0;

            var trim = Regex.Replace(script, @"[\s]+$", "");

            while (charIndex < trim.Length)
            {
                int offset;
                rslt.AddRange(Anaylize(trim.Substring(charIndex), out offset).Select(x=> x as Item));
                charIndex += offset;
            }

            return rslt;
        }

        internal static IEnumerable<Value> Anaylize(string script, out int offset)
        {
            int charIndex = 0;

            List<Item> multiItem = null;
            List<SingleValue> multiValue = null;

            string key = "";

            while(true)
            {
                int endIndex;
                ELEM_TYPE elemType = Match(script, charIndex, out endIndex);
                switch (elemType)
                {
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

                            if(multiItem != null)
                            {
                                multiItem.Add(new Item(key, value));
                            }
                            if(multiValue != null)
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

                            if (multiValue != null)
                            {
                                if (key != "")
                                {
                                    multiValue.Add(new SingleValue(key));
                                }

                                return multiValue;
                            }

                            if(multiItem != null)
                            {
                                if (key != "")
                                {
                                    throw new Exception();
                                }

                                return multiItem;
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

            var rslt = Regex.Match(curr, @"^[\s]+");
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

            rslt = Regex.Match(curr, @"^[\+\-]*\w+[\.\+\-\*/]*\w+");
            if (rslt.Success)
            {
                endIndex = charIndex + rslt.Length;
                return ELEM_TYPE.STRING;
            }

            throw new Exception();
        }

        enum ELEM_TYPE
        {
            ASSIGN,
            BRACE_OPEN,
            BRACE_CLOSE,
            STRING,
            SPACE,
            COMMA,
            PARSE_END
        }
    }

    class Value
    {

    }

    class Item : Value
    {
        string key;
        Value value;

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

    class MultiItem : Value
    {
        List<Item> elems;

        public MultiItem()
        {
            this.elems = new List<Item>();
        }

        public MultiItem(IEnumerable<Item> value)
        {
            this.elems = value.ToList();
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

    class SingleValue : Value
    {
        public SingleValue(string value)
        {
            this.value = value;
        }

        public override string ToString()
        {
            return value;
        }

        string value;
    }

    class MultiValue : Value
    {
        public MultiValue(IEnumerable<SingleValue> value)
        {
            this.elems = value.ToList();
        }

        List<SingleValue> elems;

        public override string ToString()
        {
            return "{" + string.Join(", ", elems.Select(x => x.ToString())) + "}";
        }
    }
}