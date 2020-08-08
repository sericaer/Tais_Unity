using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using TaisEngine.ModManager;

namespace TaisEngine.Run
{

    //[JsonConverter(typeof(PopConverter))]
    [JsonObject(MemberSerialization.OptIn)]
    public class Pop
    {
        //internal Family family;
        [JsonProperty, VisitPropery("pop.name")]
        public string pop_name;

        [JsonProperty, VisitPropery("pop.depart_name")]
        public string depart_name;

        [JsonProperty]
        public double num;

        [JsonProperty, VisitPropery("pop.buffer")]
        public BufferManager buffMgr = new BufferManager();

        [JsonProperty]
        public string family_name;

        //[JsonProperty]
        //internal List<(int days, HISTROY_RECORD histroy)> histroy_rec = new List<(int days, HISTROY_RECORD histroy)>();

        internal string key
        {
            get
            {
                return $"{depart_name}|{pop_name}";
            }
        }

        internal PopDef def
        {
            get
            {
                return PopDef.Find(pop_name);
            }
        }

        public Depart depart
        {
            get
            {
                return RunData.inst.departs.Single(x => x.name == depart_name);
            }
        }

        [VisitPropery("pop.is_consume")]
        public bool is_consume
        {
            get
            {
                return def.consume != null;
            }
        }

        [VisitPropery("pop.is_tax")]
        public bool is_tax
        {
            get
            {
                return def.is_tax.Value;
            }
        }

        [VisitPropery("pop.consume")]
        public double consume
        {
            get
            {
                if(consumeDetail == null)
                {
                    return -1;
                }

                return consumeDetail.Sum(x => x.value);
            }
        }

        [VisitPropery("pop.tax")]
        public double tax
        {
            get
            {
                if (!is_tax)
                {
                    throw new Exception();
                }

                var percent = (100 + taxEffects.Sum(x => x.value))/100;

                return taxBaseValue * percent;
            }
        }

        //public Family family
        //{
        //    get
        //    {
        //        if(family_name == "")
        //        {
        //            return null;
        //        }

        //        return GMData.inst.families.Single(x => x.name == family_name);
        //    }
        //}

        internal IEnumerable<(string name, double value)> consumeDetail
        {
            get
            {
                if (!is_consume)
                {
                    return null;
                }

                List<(string name, double value)> rslt = new List<(string name, double value)>();
                rslt.Add(("BASE_VALUE", def.consume.Value));
                if(def.is_tax.Value)
                {
                    rslt.Add(("CURR_TAX_EFFECT", CommonDef.TaxLevel.getConsume(RunData.inst.economy.popTax.currLevel)));
                }

                rslt.AddRange(buffMgr.getValid(x=>x.effect_consume));
                rslt.AddRange(depart.bufferManager.getValid(x=>x.effect_consume));

                return rslt;
            }
        }

        internal double GetExpectTax(int level)
        {
            if (!is_tax)
            {
                throw new Exception();
            }

            var taxbaseExpcet = CommonDef.TaxLevel.getInCome(level) * (int)num;

            var percent =(100 + taxEffects.Sum(x => x.value)) / 100;

            return Math.Round(taxbaseExpcet * percent, 1);

        }

        internal double taxBaseValue
        {
            get
            {
                return CommonDef.TaxLevel.getInCome(RunData.inst.economy.popTax.currLevel) * (int)num;
            }
        }


        internal IEnumerable<(string name, double value)> taxEffects
        {
            get
            {
                var effects = new List<(string name, double value)>();
                effects.AddRange(buffMgr.getValid(x => x.effect_tax));
                effects.AddRange(depart.bufferManager.getValid(x => x.effect_tax));

                return effects;
            }
        }

        internal Pop(PopDef popDef, string depart, double num)
        {
            this.pop_name = popDef.name;
            this.depart_name = depart;
            this.num = num;
            this.family_name = "";

            //foreach (var elem in BufferDef.BufferPopDef.Enumerate())
            //{
            //    buffers.Add(new Buffer(elem));
            //}

            //if(def.is_family)
            //{
            //    var family = Family.Generate(BackgroundDef.Enumerate().OrderBy(x => Guid.NewGuid()).First().name);
            //    this.family_name = family.name;
            //}

            //this.def.mod.AddBuffersPyObj(this.def, buffers);

            //RunData.inst.pops.Add(this);
            //GMData.inst.allBuffers.Add(this.buffers);
        }

        internal Pop()
        {

        }

        internal class HISTROY_RECORD
        {
            internal int num;
        }

        //internal double per_tax
        //{
        //    get
        //    {
        //        if(!def.is_tax)
        //        {
        //            return 0.0;
        //        }

        //        if (depart.cancel_tax)
        //        {
        //            return 0.0;
        //        }

        //        return GMData.inst.currTax / GMData.inst.pops.Where(x => x.def.is_tax && !depart.cancel_tax).Sum(x => x.num);
        //    }
        //}

        //internal double total_tax
        //{
        //    get
        //    {
        //        return per_tax * num;
        //    }
        //}

        //internal double getExpectTax(int level)
        //{
        //    return def.num * 0.001;
        //}
    }

    //public class PopConverter : JsonConverter
    //{
    //    public override bool CanConvert(Type objectType)
    //    {
    //        return typeof(Pop) == objectType;
    //    }

    //    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    //    {
    //        var pop = value as Pop;

    //        var popJObject = new JObject();
    //        popJObject.Add("name", pop.name);
    //        popJObject.Add("num", pop.num);

    //        popJObject.WriteTo(writer);
    //    }
    //}
}
