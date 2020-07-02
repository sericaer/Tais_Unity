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
            modStructDict.Add("event/common", (content, modElemnts) => { content.eventDef.common = EventDef.Anaylize(modElemnts); });
            modStructDict.Add("event/depart", (content, modElemnts) => { content.eventDef.depart = EventDef.Anaylize(modElemnts); });
            modStructDict.Add("depart",       (content, modElemnts) => { content.departDef = new DepartDef(modElemnts); });
            modStructDict.Add("pop",          (content, modElemnts) => { content.popDef = new PopDef(modElemnts); });
            modStructDict.Add("buffer/depart", (content, modElemnts) => { content.bufferDef.GetGroup("depart.buffer").AddRange(BufferDef.Anaylize(modElemnts)); });
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

            foreach(var mod in listMod.Where(x=>x.content != null))
            {
                mod.content.Check();
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
            internal PopDef popDef;
            internal BufferDef bufferDef;

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

                eventDef = new EventDef();
                bufferDef = new BufferDef();

                syntaxMod = new SyntaxMod(path);

                foreach (var elem in Mod.modStructDict)
                {
                    elem.Value(this, syntaxMod.GetElements(elem.Key));
                }

                Log.INFO("Load mod content finish");
            }

            internal void Check()
            {
                //initSelectDef.Check();
                eventDef.Check();
                //departDef.Check();
            }
        }
    }
}
