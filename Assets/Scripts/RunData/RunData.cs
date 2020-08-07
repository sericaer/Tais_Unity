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

        [JsonProperty, VisitPropery]
        public Economy economy;

        [JsonProperty, VisitPropery]
        public Taishou taishou;

        [JsonProperty, VisitPropery]
        public GMDate date;

        [JsonProperty, VisitPropery]
        public Chaoting chaoting;

        [JsonProperty]
        internal List<Depart> departs;

        [JsonProperty]
        internal List<Pop> pops;

        [JsonProperty]
        internal List<string> recordMsg = new List<string>();

        internal bool end_flag
        {
            get
            {
                return taishou.is_revoked;
            }
        }

        public RunData(InitData initData)
        {
            date = new GMDate();

            economy = new Economy(Economy.TAX_LEVEL.level2);

            taishou = new Taishou(initData.taishou);

            departs = DepartDef.Enumerate().Select(x => new Depart(x)).ToList();

            pops = new List<Pop>();
            foreach (var depart in departs)
            {
                foreach (var def in PopDef.Enumerate())
                {
                    var num = depart.def.popInitDict.ContainsKey(def.name) ? depart.def.popInitDict[def.name] : 0;
                    pops.Add(new Pop(def, depart.name, num));
                }
            }

            chaoting = new Chaoting("", pops.Where(x => x.def.is_tax.Value).Sum(x => (int)x.num), 100);
        }

        public RunData()
        {
        }

        async internal UniTask DaysInc(Func<EventInterface, UniTask> act)
        {
            foreach (var gevent in EventGroup.Generate())
            {
                await act(gevent);
            }

            foreach (var depart in departs)
            {
                foreach (var gevent in depart.DaysInc())
                {
                    await act(gevent);
                }
            }

            foreach(var elem in DaysUpdate.all)
            {
                elem.DaysUpdateProcess();
            }

            date++;
        }
    }
}
