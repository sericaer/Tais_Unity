using ModVisitor;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaisEngine.Init;

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
        public Taishou taishou;

        [JsonProperty, ModVisit]
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

        internal void DaysInc()
        {
            date++;
        }
    }
}
