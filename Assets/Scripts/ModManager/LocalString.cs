using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using TaisEngine.ConfigManager;

namespace TaisEngine.ModManager
{
    class LocalString
    {

        internal static string Get(string format, params object[] objs)
        {

            var rslt = format;
            foreach (var mod in Mod.listMod.Where(x=>x.content != null && x.content.localString.dictlang.ContainsKey(Config.inst.lang)))
            {
                if(mod.content.localString.dictlang[Config.inst.lang].ContainsKey(format))
                {
                    rslt = mod.content.localString.dictlang[Config.inst.lang][format];
                    if(objs.Count() == 0)
                    {
                        return rslt;
                    }

                    rslt = string.Format(mod.content.localString.dictlang[Config.inst.lang][format], 
                                            objs.Select(y => y is string ? Get(y as string) : y).ToArray());
                }
            }

            return rslt;
        }

        Dictionary<string, Dictionary<string, string>> dictlang;
        internal Dictionary<string, PersonName> dictlan2PersonName = new Dictionary<string, PersonName>();

        internal LocalString(string path)
        {
            dictlang = new Dictionary<string, Dictionary<string, string>>();

            if (!Directory.Exists(path))
            {
                return;
            }

            foreach(var dir in Directory.EnumerateDirectories(path))
            {
                var lang = Path.GetFileNameWithoutExtension(dir);
                var dictText = new Dictionary<string, string>();

                foreach (var filePath in Directory.EnumerateFiles(dir, "*.txt"))
                {
                    foreach (var line in File.ReadAllLines(filePath))
                    {
                        if (line.Count() == 0)
                        {
                            continue;
                        }

                        var key = line.Substring(0, line.IndexOf(':'));
                        var value = line.Substring(line.IndexOf(':') + 1, line.Count() - line.IndexOf(':') - 1);

                        if (dictText.ContainsKey(key))
                        {
                            throw new ArgumentException($"already have the local string key, line:{line} in file:{filePath}");
                        }

                        dictText.Add(key, value);
                    }
                }

                dictlang.Add(lang, dictText);

                dictlan2PersonName.Add(lang, PersonName.Generate(dir));
            }
        }
    }
}
