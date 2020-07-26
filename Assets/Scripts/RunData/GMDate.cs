
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
        internal static GMDate Parse(int days)
        {
            return new GMDate()
            {
                year = days / 360,
                month = (days % 360) / 30,
                day = (days % 360) % 30
            };
        }

        [JsonProperty, VisitPropery("date.year")]
        public int year;

        [JsonProperty, VisitPropery("date.month")]
        public int month;

        [JsonProperty, VisitPropery("date.day")]
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
