using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TaisEngine.ModManager;

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

        [VisitPropery("player.is_revoked")]
        public bool is_revoked = false;

        [VisitPropery("player.bad_appraise_count")]
        public int bad_appraise_count
        {
            get
            {
                return appraises.Count(x => x == enumAppraise.BAD);
            }
        }

        [VisitPropery("player.year_apprasie")]
        public string year_apprasie
        {
            set
            {
                enumAppraise enumValue;
                if(!Enum.TryParse<enumAppraise>(value, out enumValue))
                {
                    throw new Exception();
                }

                appraises.Add(enumValue);
            }
        }

        [JsonProperty]
        public string background;

        [JsonProperty]
        public List<enumAppraise> appraises = new List<enumAppraise>();

        public enum enumAppraise
        {
            BAD,
            NORMAL,
            GOOD,
        }
    }
}
