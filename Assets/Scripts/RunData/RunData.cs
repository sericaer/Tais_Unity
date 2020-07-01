using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaisEngine.Init;
using TaisEngine.ModManager;
using UniRx.Async;

namespace TaisEngine.Run
{
    [JsonObject(MemberSerialization.OptIn)]
    class RunData
    {
        public static RunData inst;

        public static void New(InitData init)
        {
            inst = new RunData(init);
        }

        [JsonProperty]
        public Economy economy;

        [JsonProperty]
        public Taishou taishou;

        [JsonProperty, VisitPropery]
        public GMDate date;

        [JsonProperty]
        internal List<Depart> departs;

        [JsonProperty]
        internal List<Pop> pops;

        [JsonProperty]
        internal List<string> recordMsg = new List<string>();

        internal bool end_flag;

        public RunData(InitData initData)
        {
            date = new GMDate();

            economy = new Economy();

            taishou = new Taishou(initData.taishou);

            departs = DepartDef.Enumerate().Select(x => new Depart(x)).ToList();

            pops = new List<Pop>();
            foreach (var depart in departs)
            {
                foreach(var def in PopDef.Enumerate())
                {
                    var num = depart.def.popInitDict.ContainsKey(def.name) ? depart.def.popInitDict[def.name] : 0;
                    pops.Add(new Pop(def, depart.name, num));
                }
            }

        }

        public RunData()
        {
        }

        async internal UniTask DaysInc(Func<EventDef.Element, UniTask> act)
        {
            foreach (var gevent in EventDef.Generate())
            {
                await act(gevent);
            }

            date++;
        }
    }
}
