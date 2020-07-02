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

        //internal static IEnumerable<EventDef.Interface> DaysInc()
        //{
        //    foreach(var depart in GMData.inst.departs)
        //    {
        //        depart.cropGrowingProcess();
        //    }

        //    yield break;
        //}

        public IEnumerable<Pop> pops
        {
            get
            {
                return RunData.inst.pops.Where(x=>x.depart_name == name);
            }
        }


        ////public List<Buffer> buffers = new List<Buffer>();

        //internal string color
        //{
        //    get
        //    {
        //        return string.Format("({0:0}, {1:0}, {2:0})", def.color[0], def.color[1], def.color[2]);
        //    }
        //}

        [JsonProperty, VisitPropery("depart.name")]
        public string name;

        [JsonProperty, VisitPropery("depart.buffer")]
        public BufferManager bufferManager = new BufferManager();

        [JsonProperty]
        public bool is_crop_growing;

        [JsonProperty]
        public double crop_grow_percent;

        //[JsonProperty]
        //public List<Buffer> buffers;

        [JsonProperty]
        internal bool cancel_tax;

        //internal double cropGrowingSpeed
        //{
        //    get
        //    {
        //        return growSpeedDetail.Sum(x => x.value);
        //    }
        //}


        internal DepartDef.Element def
        {
            get
            {
                return DepartDef.Find(name);
            }
        }


        //internal List<(string name, double value)> growSpeedDetail
        //{
        //    get
        //    {
        //        var rslt = new List<(string name, double value)>();

        //        rslt.Add(("BASE_VALUE", 100.0 / growingdays));
        //        rslt.AddRange(buffers.exist_crop_growing_effects()
        //                             .Select(x => (x.name, x.value * 100.0 / growingdays)));

        //        return rslt;
        //    }
        //}

        internal Depart(DepartDef.Element def)
        {
            this.name = def.name;
            //this.buffers = new List<Buffer>();
            this.cancel_tax = false;
           //GMData.inst.allBuffers.Add(this.buffers);


            //foreach (var elem in BufferDef.BufferDepartDef.Enumerate())
            //{
            //    buffers.Add(new Buffer(elem));
            //}

            //this.def.mod.AddBuffersPyObj(this.def, buffers);
        }

        internal Depart()
        {

        }

        //private void cropGrowingProcess()
        //{
        //    if(is_crop_growing)
        //    {
        //        crop_grow_percent += cropGrowingSpeed;
        //    }
        //    else
        //    {
        //        crop_grow_percent = 0;
        //    }

        //    if(crop_grow_percent < 0)
        //    {
        //        crop_grow_percent = 0;
        //    }
        //}

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
