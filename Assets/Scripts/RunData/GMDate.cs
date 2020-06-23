
﻿using System;
using ModVisitor;
﻿using Newtonsoft.Json;
using TaisEngine.ModManager;
using UnityEngine.UI.Extensions;

namespace TaisEngine.Run
{
    [JsonObject(MemberSerialization.OptIn)]
    public class GMDate
    {
        [JsonProperty, ModVisit]
        public int year;

        [JsonProperty, ModVisit]
        public int month;

        [JsonProperty, ModVisit]
        public int day;

        public static GMDate operator++(GMDate date)
        {
            if(date.day != 30)
            {
                date.day++;
                return date;
            }

            if(date.month != 12)
            {
                date.day = 1;
                date.month++;
                return date;
            }
            date.month = 1;
            date.year++;
            return date;
        }


        public int total_days
        {
            get
            {
                return day + (month-1)*12 + (year-1)*360;
            }
        }

        public GMDate()
        {
            year = 1;
            month = 1;
            day = 1;
        }

        public override string ToString()
        { 
            return LocalString.Get("date", year, month, day);
        }
    }
}
