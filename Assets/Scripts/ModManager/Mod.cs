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

        static Mod()
        {
            modStructDict = new Dictionary<string, Action<Content, List<SyntaxMod.Element>>>();

            modStructDict.Add("init_select",  (content, modElemnts) => { content.initSelectDef = new InitSelectDef(modElemnts); });
            modStructDict.Add("event/common", (content, modElemnts) => { content.CreateCommonEvent(modElemnts); });
            modStructDict.Add("depart",       (content, modElemnts) => { content.departDef = new DepartDef(modElemnts); });
        }


        internal static string modRootPath = Application.streamingAssetsPath + "/mod/";
        internal static Dictionary<string, Action<Content, List<SyntaxMod.Element>>> modStructDict;

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
            SyntaxMod syntaxMod;

            internal LocalString localString;
            internal InitSelectDef initSelectDef;
            internal EventDef eventDef;
            internal DepartDef departDef;

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

                syntaxMod = new SyntaxMod(path);

                foreach(var elem in Mod.modStructDict)
                {
                    elem.Value(this, syntaxMod.GetElements(elem.Key));
                }

                CheckMod();

                Log.INFO("Load mod content finish");
            }

            private void CheckMod()
            {
                //initSelectDef.Check();
                //eventDef.Check();
                departDef.Check();
            }

            internal void CreateCommonEvent(List<SyntaxMod.Element> modElements)
            {
                if(eventDef == null)
                {
                    eventDef = new EventDef();
                }

                eventDef.CreateCommons(modElements);
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

        enum MOD_STRUCT
        {
            INIT_SELECT,

        }
    }
}
