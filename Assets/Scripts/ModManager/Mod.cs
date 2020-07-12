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
            modStructDict = new Dictionary<string, Action<Mod, SyntaxModElement>>();

            modStructDict.Add("init_select",   (mod, modElemnts) => InitSelectDef.AnaylzeMod(mod, modElemnts));
            modStructDict.Add("event/common",  (mod, modElemnts) => EventDefCommon.AnaylzeMod(mod, modElemnts));
            modStructDict.Add("event/depart",  (mod, modElemnts) => EventDefDepart.AnaylzeMod(mod, modElemnts));
            modStructDict.Add("buffer/depart", (mod, modElemnts) => BufferDefDepart.AnaylzeMod(mod, modElemnts));
            modStructDict.Add("depart",        (mod, modElemnts) => DepartDef.AnaylzeMod(mod, modElemnts));
            modStructDict.Add("pop",           (mod, modElemnts) => PopDef.AnaylizeMod(mod, modElemnts));
            modStructDict.Add("defines",       (mod, modElemnts) => CommonDef.AnaylizeMod(mod, modElemnts));
        }


        internal static string modRootPath = Application.streamingAssetsPath + "/mod/";
        internal static Dictionary<string, Action<Mod, SyntaxModElement>> modStructDict;

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
        internal Content content
        {
            get
            {
                if(!info.isloadMod)
                {
                    return null;
                }

                if(_content == null)
                {
                    _content = new Content();
                }

                return _content;
            }
        }

        internal Content _content;

        internal Mod(string modPath)
        {
            path = Path.GetFullPath(modPath);

            info = Info.Load(path);

            if(info.isloadMod)
            {
                Log.INFO("Load mod content start");

                content.localString = new LocalString($"{path}/lang/");
                content.commonDef = new CommonDef();
                content.bufferGroup = new BufferGroup();
                content.eventGroup = new EventGroup();
                content.initSelectDefs = new List<InitSelectDef>();
                content.popDefs = new List<PopDef>();

                var syntaxMod = new SyntaxMod(path);

                foreach (var elem in Mod.modStructDict)
                {
                    foreach(var syntaxModElem in syntaxMod.GetElements(elem.Key))
                    {
                        elem.Value(this, syntaxModElem);
                    }
                }

                Log.INFO("Load mod content finish");
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
            internal List<InitSelectDef> initSelectDefs;
            internal EventGroup eventGroup;
            internal List<DepartDef> departDefs;
            internal List<PopDef> popDefs;
            internal BufferGroup bufferGroup;
            internal CommonDef commonDef;

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

            internal Content()
            { 
                initSelectDefs = new List<InitSelectDef>();
                departDefs = new List<DepartDef>();
                popDefs = new List<PopDef>();

                eventGroup = new EventGroup();
                bufferGroup = new BufferGroup();          
            }

            internal void Check()
            {
                //initSelectDefs.ForEach(x=>x.CheckDefault());
                eventGroup.Check();
                //departDef.Check();
            }
        }
    }
}
