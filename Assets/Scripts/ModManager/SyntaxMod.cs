using System;
using System.Collections.Generic;
using System.IO;
using SyntaxAnaylize;

namespace TaisEngine.ModManager
{
    internal class SyntaxMod
    {
        internal string path;

        internal class MultiItem
        {
            internal string filePath;
            internal SyntaxAnaylize.MultiItem multiItem;
        }

        internal SyntaxMod(string path)
        {
            this.path = path;

            dict = new Dictionary<string, List<MultiItem>>();

            foreach(var dir in Mod.modStructDict.Keys)
            {
                SyntaxDir(dir);
            }
        }

        private void SyntaxDir(string dir)
        {
            var subpath = $"{path}/{dir}/";

            List<MultiItem> elements = new List<MultiItem>();
            if(Directory.Exists(subpath))
            {
                foreach (var file in Directory.EnumerateFiles(subpath, "*.txt"))
                {
                    MultiItem element = new MultiItem()
                    {
                        filePath = file,
                        multiItem = Syntax.Anaylize(file, File.ReadAllText(file))
                    };

                    elements.Add(element);
                }
            }

            dict.Add(dir, elements);
        }

        internal List<MultiItem> GetElements(string name)
        {
            List<MultiItem> rslt;

            if(dict.TryGetValue(name, out rslt))
            {
                return rslt;
            }

            return null;
        }

        Dictionary<string, List<MultiItem>> dict;
    }
}