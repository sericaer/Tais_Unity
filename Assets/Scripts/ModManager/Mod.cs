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
            modStructDict = new Dictionary<string, Action<Mod, IEnumerable<MultiItem>>>();

            modStructDict.Add("init_select",  (mod, modElemnts) => { mod.content.initSelectDefs.AddRange(InitSelectDef.ParseList(mod.info.name, modElemnts)); });
            modStructDict.Add("event/common", (mod, modElemnts) => { mod.content.eventGroup.common.AddRange(EventDefCommon.ParseList(mod.info.name, modElemnts)); });
            modStructDict.Add("event/depart", (mod, modElemnts) => { mod.content.eventGroup.depart.AddRange(EventDefDepart.ParseList(mod.info.name, modElemnts)); });
            modStructDict.Add("buffer/depart", (mod, modElemnts) => { mod.content.bufferGroup.depart.AddRange(BufferDefDepart.ParseList(mod.info.name, modElemnts)); });
            modStructDict.Add("depart",       (mod, modElemnts) => { mod.content.departDefs.AddRange(DepartDef.ParseList(mod.info.name, modElemnts)); });
            modStructDict.Add("pop",          (mod, modElemnts) => { mod.content.popDefs.AddRange(PopDef.ParseList(mod.info.name, modElemnts)); });
            modStructDict.Add("defines",       (mod, modElemnts) => { mod.content.commonDef = new CommonDef(modElemnts); });
        }


        internal static string modRootPath = Application.streamingAssetsPath + "/mod/";
        internal static Dictionary<string, Action<Mod, IEnumerable<MultiItem>>> modStructDict;

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

                var syntaxMod = new SyntaxMod(path);

                foreach (var elem in Mod.modStructDict)
                {
                    elem.Value(this, syntaxMod.GetElements(elem.Key).Select(x => x.multiItem));
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
            SyntaxMod syntaxMod;

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
