using System;
using System.Collections.Generic;

namespace TaisEngine.Run
{
    public abstract class DaysUpdate
    {
        public static List<DaysUpdate> all = new List<DaysUpdate>();

        public DaysUpdate()
        {
            all.Add(this);
        }

        public abstract void DaysUpdateProcess();
    }
}
