﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Newtonsoft.Json;
using TaisEngine.ModManager;

namespace TaisEngine.Run
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Buffer
    {
        [JsonProperty]
        public string name;

        public Buffer(string buffName)
        {
            this.name = buffName;
        }

        internal BufferGroup.BufferInterface def
        {
            get
            {
                return BufferDef.FindByName(name);
            }
        }
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class BufferManager : IEnumerable<Buffer>
    {
        internal void SetValid(string buffName)
        {
            buffers.Add(new Buffer(buffName));
        }

        internal bool IsValid(string name)
        {
            return buffers.Find(x => x.name == name) != null;
        }

        public IEnumerator<Buffer> GetEnumerator()
        {
            return ((IEnumerable<Buffer>)buffers).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<Buffer>)buffers).GetEnumerator();
        }

        [JsonProperty]
        internal List<Buffer> buffers = new List<Buffer>();
    }
}
