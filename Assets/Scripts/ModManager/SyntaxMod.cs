using System;
using System.Collections.Generic;
using System.IO;
using SyntaxAnaylize;

namespace TaisEngine.ModManager
{
    internal class SyntaxMod
    {
        internal class Element
        {
            internal string name;
            internal MultiItem multiItem;
        }

        internal SyntaxMod(string path)
        {
            dict = new Dictionary<string, List<Element>>();

            var subpath = $"{path}/init_select/";

            List<Element> elements = new List<Element>();
            foreach (var file in Directory.EnumerateFiles(subpath, "*.txt"))
            {
                Element element = new Element()
                {
                    name = Path.GetFileNameWithoutExtension(file),
                    multiItem = Syntax.Anaylize(File.ReadAllText(file))
                };

                elements.Add(element);
            }

            dict.Add("init_select", elements);
        }

        internal List<Element> GetElements(string name)
        {
            List<Element> rslt;

            if(dict.TryGetValue(name, out rslt))
            {
                return rslt;
            }

            return null;
        }

        Dictionary<string, List<Element>> dict;
    }
}