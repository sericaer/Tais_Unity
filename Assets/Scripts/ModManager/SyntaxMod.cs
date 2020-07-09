using System;
using System.Collections.Generic;
using System.IO;
using SyntaxAnaylize;

namespace TaisEngine.ModManager
{
    internal class SyntaxMod
    {
        internal string path;

        internal class ModItem
        {
            internal string filePath;
            internal SyntaxAnaylize.MultiItem multiItem;
        }

        internal SyntaxMod(string path)
        {
            this.path = path;

            dict = new Dictionary<string, List<ModItem>>();

            foreach(var dir in Mod.modStructDict.Keys)
            {
                SyntaxDir(dir);
            }
        }

        private void SyntaxDir(string dir)
        {
            var subpath = $"{path}/{dir}/";

            List<ModItem> elements = new List<ModItem>();
            if(Directory.Exists(subpath))
            {
                foreach (var file in Directory.EnumerateFiles(subpath, "*.txt"))
                {
                    ModItem element = new ModItem()
                    {
                        filePath = file,
                        multiItem = Syntax.Anaylize(file, File.ReadAllText(file))
                    };

                    elements.Add(element);
                }
            }

            dict.Add(dir, elements);
        }

        internal List<ModItem> GetElements(string name)
        {
            List<ModItem> rslt;

            if(dict.TryGetValue(name, out rslt))
            {
                return rslt;
            }

            return null;
        }

        Dictionary<string, List<ModItem>> dict;
    }
}