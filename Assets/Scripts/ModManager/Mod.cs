using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using UnityEngine;

using Tools;
using TaisEngine.ConfigManager;

using Newtonsoft.Json;
using SyntaxAnaylize;

namespace TaisEngine.ModManager
{
    partial class Mod
    {

        internal static string modRootPath = Application.streamingAssetsPath + "/mod/";
        internal static List<Mod> listMod = new List<Mod>();

        internal static void Load()
        {
            listMod.Clear();

            foreach (var path in Directory.EnumerateDirectories(modRootPath))
            {
                try
                {
                    var infoFile = $"{path}/info.json";
                    if (!File.Exists(infoFile))
                    {
                        continue;
                    }

                    listMod.Add(new Mod(path));
                }
                catch (Exception e)
                {
                    throw new Exception($"load mod {path} failed!", e);
                }

            }
        }

        //internal static string GetLocalString(string arg)
        //{
        //    object[] objs = { };
        //    return GetLocalString(arg, objs);
        //}

        //internal static IEnumerable<InitSelectDef> EnumerateInitSelect()
        //{
        //    foreach (var mod in Mod.listMod.Where(x=>x.content != null))
        //    {
        //        foreach (var select in mod.content.dictInitSelect.Values)
        //        {
        //            yield return select;
        //        }
        //    }
        //}

        //        internal static string GetLocalString(string arg, params object[] objs)
        //        {
        //            try
        //            {
        //#if UNITY_EDITOR_OSX
        //            return arg;
        //#endif
        //                var args = arg.Split('|');

        //                string rslt = String.Join("", args.Select(x =>
        //                {

        //                    string tRslt = x;

        //                    foreach (var mod in listMod.Where(y => y.content != null))
        //                    {
        //                        if (!mod.content.dictlang.ContainsKey(Config.inst.lang))
        //                        {
        //                            continue;
        //                        }

        //                        if (mod.content.dictlang[Config.inst.lang].ContainsKey(x))
        //                        {
        //                            tRslt = mod.content.dictlang[Config.inst.lang][x];
        //                            if (objs.Count() != 0)
        //                            {
        //                                var test = objs.Select(y => y is string ? GetLocalString(y as string) : y).ToArray();
        //                                tRslt = string.Format(tRslt, test);
        //                            }
        //                            break;
        //                        }
        //                    }

        //                    return tRslt;
        //                }));

        //                return rslt;
        //            }
        //            catch(Exception e)
        //            {
        //                throw new Exception(arg, e);
        //            }

        //        }

        internal string path;
        internal Info info;
        internal Content content;

        internal Mod(string modPath)
        {
            path = Path.GetFullPath(modPath);

            info = Info.Load(path);

            if(info.isloadMod)
            {
                content = new Content(path);
            }
        }

        public class Info
        {
            public string name = "";
            public bool master = false;
            public string author = "";


            internal static Info Load(string path)
            {
                Log.INFO($"Read mod info {path} ");

                return JsonConvert.DeserializeObject<Info>(File.ReadAllText($"{path}/info.json"));
            }

            internal bool isloadMod
            {
                get
                {
                    return master || Config.inst.select_mods.Contains(name);
                }
            }
        }

        internal class Content
        {
            internal LocalString localString;
            internal InitSelectDef initSelectDef;

            //internal BackgroundDef backgroundDef;
            //internal DepartDef departDef;
            //internal PopDef popDef;
            //internal EventDef eventDef;
            //internal TaskDef taskDef;
            //internal BufferDef bufferDef;
            //internal Defines defines;

            //internal Dictionary<string, InitSelectDef> dictInitSelect = new Dictionary<string, InitSelectDef>();

            //internal Dictionary<string, Dictionary<string, string>> dictlang = new Dictionary<string, Dictionary<string, string>>();
            //

            internal Content(string path)
            {
                Log.INFO("Load mod content start");

                localString = new LocalString($"{path}/lang/");
                initSelectDef = new InitSelectDef($"{path}/init_select/");

                Log.INFO("Load mod content finish");
            }

            //internal void Load(string mod, LuaEnv luaenv)
            //{
            //    popDef = new PopDef(mod, luaenv.Global);
            //    departDef = new DepartDef(mod, luaenv.Global);
            //    backgroundDef = new BackgroundDef(mod,luaenv.Global);
            //    eventDef = new EventDef(mod, luaenv.Global.Get<LuaTable>("EVENT"));
            //    taskDef = new TaskDef(mod, luaenv.Global);
            //    bufferDef = new BufferDef(mod, luaenv.Global.Get<LuaTable>("BUFFER"));
            //    defines = new Defines(mod, luaenv.Global.Get<LuaTable>("DEFINES"));
            //}
        }

        //private void LoadInitSelectDef(LuaEnv luaenv)
        //{
        //    LuaTable luaTable = luaenv.Global.Get<LuaTable>("INIT_SELECT");//映射到LuaTable，by ref

        //    foreach (var key in luaTable.GetKeys<string>())
        //    {
        //        var value = luaTable.Get<InitSelectDef>(key);
        //        if(value != null)
        //        {
        //            content.dictInitSelect.Add(key, value);
        //        }
                
        //    }
        //}

        //private void loadLocalText(IEnumerable<string> dirs)
        //{
        //    foreach (var dir in dirs)
        //    {
        //        var dirName = Path.GetFileNameWithoutExtension(dir);
        //        var dictText = new Dictionary<string, string>();

        //        foreach (var filePath in Directory.EnumerateFiles(dir, "*.txt"))
        //        {
        //            foreach (var line in File.ReadAllLines(filePath))
        //            {
        //                if (line.Count() == 0)
        //                {
        //                    continue;
        //                }

        //                var key = line.Substring(0, line.IndexOf(':'));
        //                var value = line.Substring(line.IndexOf(':')+1, line.Count()- line.IndexOf(':') - 1);

        //                if (dictText.ContainsKey(key))
        //                {
        //                    throw new ArgumentException($"already have the local string key, line:{line} in file:{filePath}");
        //                }

        //                dictText.Add(key, value);
        //            }
        //        }

        //        content.dictlang.Add(dirName, dictText);

        //        content.dictlan2PersonName.Add(dirName, PersonName.Generate(dir));
        //    }
        //}

//        private LuaEnv DoLuas()
//        {
//            var startRequire = @"require 'xlua.class_func'
//require 'xlua.inner_def'";

//            var luaDirs = new List<string>() {  "DEFINES",
//                                                "INIT_SELECT",
//                                                "POP",
//                                                "BACKGROUND",
//                                                "DEPART",
//                                                "EVENT/COMMON",
//                                                "EVENT/DEPART",
//                                                "EVENT/POP",
//                                                "TASK",
//                                                "BUFFER/DEPART",
//                                                "BUFFER/POP"
//                                              };

//            var luaFilePaths = luaDirs.Select(x => $"{path}/{x}/")
//                                        .Where(x => Directory.Exists(x))
//                                        .SelectMany(x => Directory.EnumerateFiles(x, "*.lua"))
//                                        .ToList();

//            var endRequire = "require 'xlua.inner_def_init'";

//            var luaenv = new LuaEnv();

//            luaenv.DoString(startRequire);

//            foreach (var luapath in luaFilePaths)
//            {
//                string convertText = convertLua(luapath);

//                try
//                {
//                    luaenv.DoString(convertText, luapath);
//                }
//                catch (Exception e)
//                {
//                    throw new Exception(convertText, e);
//                }
//            }

//            luaenv.DoString(endRequire);
//            return luaenv;
//        }

//        private string convertLua(string luapath)
//        {
//            var luaTableName = Path.GetDirectoryName(luapath)
//                                               .Replace(path, "")
//                                               .Trim(Path.DirectorySeparatorChar)
//                                               .Replace(Path.DirectorySeparatorChar, '.')
//                                               .ToUpper();

//            var rawLines = File.ReadLines(luapath).ToList();

//            string convertText = luaTableName + "." + Path.GetFileNameWithoutExtension(luapath) + "={";

//            int isInFunc = 0;
//            int isInIF = 0;
//            for (int i = 0; i < rawLines.Count(); i++)
//            {
//                string curr = rawLines[i].Trim();
//                if (curr == "")
//                {
//                    convertText += curr + "\n";
//                    continue;
//                }

//                if (curr.EndsWith(",")
//                    || curr.EndsWith("{")
//                    || curr.EndsWith("="))
//                {
//                    convertText += curr + "\n";
//                    continue;
//                }

//                if (curr.Contains("function"))
//                {
//                    convertText += curr + "\n";
//                    isInFunc++;
//                    continue;
//                }

//                if (curr.StartsWith("if") || curr.StartsWith("for"))
//                {
//                    convertText += curr + "\n";
//                    isInIF++;
//                    continue;
//                }

//                if (curr.EndsWith("end"))
//                {
//                    if (isInIF != 0)
//                    {
//                        convertText += curr + "\n";
//                        isInIF--;
//                        continue;
//                    }

//                    convertText += curr + "," + "\n";
//                    isInFunc--;
//                    continue;
//                }


//                if (isInFunc != 0)
//                {
//                    convertText += curr + "\n";
//                    continue;
//                }

//                convertText += curr + "," + "\n";
//            }

//            convertText += "\n}";
//            return convertText;
//        }
    }
}
