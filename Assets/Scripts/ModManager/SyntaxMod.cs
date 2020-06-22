using System;
using System.Collections.Generic;
using System.IO;
using SyntaxAnaylize;

namespace TaisEngine.ModManager
{
    internal class SyntaxMod
    {
        internal string path;

        internal class Element
        {
            internal string name;
            internal MultiItem multiItem;
        }

        internal SyntaxMod(string path)
        {
            this.path = path;

            dict = new Dictionary<string, List<Element>>();

            foreach(var dir in Mod.modStructDict.Keys)
            {
                SyntaxDir(dir);
            }
        }

        private void SyntaxDir(string dir)
        {
            var subpath = $"{path}/{dir}/";

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

            dict.Add(dir, elements);
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