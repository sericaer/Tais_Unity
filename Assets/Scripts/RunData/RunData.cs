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

        internal static void AssocVisitor()
        {
            Visitor.Add("gm.day", () => (double)inst.date.day, null, typeof(double));
            Visitor.Add("gm.year", () => (double)inst.date.year, null, typeof(double));
            Visitor.Add("gm.month", () => (double)inst.date.month, null, typeof(double));
        }

        [JsonProperty]
        public Taishou taishou;

        [JsonProperty]
        public GMDate date;

        internal bool end_flag;

        public RunData(InitData initData)
        {
            taishou = new Taishou(initData.taishou);

            date = new GMDate();
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
