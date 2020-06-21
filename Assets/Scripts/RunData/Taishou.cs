using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace TaisEngine.Run
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Taishou
    {
        [JsonProperty]
        public string name;

        [JsonProperty]
        public int age;

        [JsonProperty]
        public double prestige;

        //[JsonProperty]
        //public List<Buffer> buffers;

        //public BackgroundDef.Interface background
        //{
        //    get
        //    {
        //        return BackgroundDef.Enumerate().Single(x => x.name == _background);
        //    }
        //}

        public Taishou(Init.Taishou initdata)
        {
            this.name = initdata.name;
            this.age = initdata.age;
            this.background = initdata.background;

            //this.buffers = new List<Buffer>();
            //GMData.inst.allBuffers.Add(this.buffers);
        }

        public Taishou()
        {

        }

        [JsonProperty]
        public string background;
    }
}
