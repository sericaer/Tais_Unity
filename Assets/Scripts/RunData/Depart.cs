﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using TaisEngine.ModManager;
using Tools;
using UnityEngine;

namespace TaisEngine.Run
{

    //[JsonConverter(typeof(DepartConverter))]
    [JsonObject(MemberSerialization.OptIn)]
    public class Depart
    {
        //public static int growingdays = 250;

        internal IEnumerable<EventInterface> DaysInc()
        {
            cropGrowingProcess();

            yield break;
        }

        public IEnumerable<Pop> pops
        {
            get
            {
                return RunData.inst.pops.Where(x=>x.depart_name == name);
            }
        }

        public string color
        {
            get
            {
                var colorDef = def.color.Result();

                return string.Format("{0:0},{1:0},{2:0}", colorDef[0], colorDef[1], colorDef[2]);
            }
        }

        [JsonProperty, VisitPropery("depart.name")]
        public string name;

        [JsonProperty, VisitPropery("depart.buffer")]
        public BufferManager bufferManager = new BufferManager();

        [JsonProperty, VisitPropery("depart.crop_grown")]
        public double crop_grow_percent;

        //[JsonProperty]
        //public List<Buffer> buffers;

        [JsonProperty]
        internal bool cancel_tax;

        internal double cropGrowingSpeed
        {
            get
            {
                return growSpeedDetail.Sum(x => x.value);
            }
        }

        internal bool cropGrowingValid
        {
            get
            {
                var startDate = CommonDef.CropGrowingInfo.Get().startDate;
                if (RunData.inst.date.month < startDate.month)
                {
                    return false;
                }

                if (RunData.inst.date.month == startDate.month)
                {
                    if (RunData.inst.date.day < startDate.day)
                    {
                        return false;
                    }
                }

                var endDate = CommonDef.CropGrowingInfo.Get().endDate;

                if (RunData.inst.date.month > endDate.month)
                {
                    return false;
                }

                if (RunData.inst.date.month == endDate.month)
                {
                    if (RunData.inst.date.day > endDate.day)
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        internal DepartDef def
        {
            get
            {
                return DepartDef.Find(name);
            }
        }


        internal List<(string name, double value)> growSpeedDetail
        {
            get
            {
                var rslt = new List<(string name, double value)>();

                rslt.Add(("BASE_VALUE", CommonDef.CropGrowingInfo.Get().base_speed.Value));
                rslt.AddRange(bufferManager.crop_growing_effects().Select(x => (x.name, x.value)));

                return rslt;
            }
        }

        internal Depart(DepartDef def)
        {
            this.name = def.name;
            this.cancel_tax = false;

        }

        internal Depart()
        {

        }

        private void cropGrowingProcess()
        {
            if (cropGrowingValid)
            {
                crop_grow_percent += cropGrowingSpeed;
            }
        }

        //internal void growStart()
        //{
        //    cropGrowing = 0;
        //}

        //internal void growFinish()
        //{
        //    cropGrowing = null;
        //}
    }

    //public class DepartConverter : JsonConverter
    //{
    //    public override bool CanConvert(Type objectType)
    //    {
    //        return typeof(Depart) == objectType;
    //    }

    //    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    //    {
    //        var depart = value as Depart;

    //        var departJObject = new JObject();
    //        departJObject.Add("name", depart.def.name);

    //        var popsJObject = new JArray();
    //        foreach (var pop in depart.pops)
    //        {
    //            popsJObject.Add(JToken.FromObject(pop));
    //        }
    //        departJObject.Add("pops", popsJObject);

    //        departJObject.WriteTo(writer);
    //    }
    //}
}
