using System;
using System.Collections.Generic;
using System.IO;
using SyntaxAnaylize;

namespace TaisEngine.ModManager
{
    internal class SyntaxMod
    {
        internal string path;

        internal SyntaxMod(string path)
        {
            this.path = path;

            dict = new Dictionary<string, List<SyntaxModElement>>();

            foreach(var dir in Mod.modStructDict.Keys)
            {
                SyntaxDir(dir);
            }
        }

        private void SyntaxDir(string dir)
        {
            var subpath = $"{path}/{dir}/";

            List<SyntaxModElement> elements = new List<SyntaxModElement>();
            if(Directory.Exists(subpath))
            {
                foreach (var file in Directory.EnumerateFiles(subpath, "*.txt"))
                {
                    SyntaxModElement element = new SyntaxModElement()
                    {
                        filePath = file,
                        multiItem = Syntax.Anaylize(file, File.ReadAllText(file))
                    };

                    elements.Add(element);
                }
            }

            dict.Add(dir, elements);
        }

        internal List<SyntaxModElement> GetElements(string name)
        {
            List<SyntaxModElement> rslt;

            if(dict.TryGetValue(name, out rslt))
            {
                return rslt;
            }

            return null;
        }

        Dictionary<string, List<SyntaxModElement>> dict;
    }

    internal class SyntaxModElement
    {
        internal string filePath;
        internal SyntaxAnaylize.MultiItem multiItem;
    }
}