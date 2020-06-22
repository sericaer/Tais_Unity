using Newtonsoft.Json;
using System;
using TaisEngine.ModManager;
using UnityEngine.UI.Extensions;

namespace TaisEngine.Run
{
    [JsonObject(MemberSerialization.OptIn)]
    public class GMDate
    {
        [JsonProperty]
        public int year;

        [JsonProperty]
        public int month;

        [JsonProperty]
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
                date.month++;
                return date;
            }
            
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

        ////internal bool isSpring
        ////{
        ////    get
        ////    {
        ////        return (month < 4);
        ////    }
        ////}
        ////internal bool isSummer
        ////{
        ////    get
        ////    {
        ////        return (month < 7 && month >=4);
        ////    }
        ////}
        ////internal bool isAutumn
        ////{
        ////    get
        ////    {
        ////        return (month < 10 && month >= 7);
        ////    }
        ////}
        ////internal bool isWinter
        ////{
        ////    get
        ////    {
        ////        return (month <= 12 && month >= 10);
        ////    }
        ////}

        //public int total_days
        //{
        //    get
        //    {
        //        return RunData.inst.days;
        //    }
        //}

        //public int year
        //{
        //    get
        //    {
        //        return _year(total_days);
        //    }

        //}

        //public int month
        //{
        //    get
        //    {
        //        return _month(total_days);
        //    }
        //}

        //public int day
        //{
        //    get
        //    {
        //        return _day(total_days);
        //    }
        //}

        //public override string ToString()
        //{
        //    if (total_days == 0)
        //    {
        //        return "--";
        //    }

        //    return LocalString.Get("date", year, month, day);
        //}

        ////public static string ToString(int days)
        ////{
        ////    if (days == 0)
        ////    {
        ////        return "--";
        ////    }

        ////    return Mod.GetLocalString("date", _year(days), _month(days), _day(days));
        ////}

        //internal static int _day(int days)
        //{
        //    return days % 30 == 0 ? 30 : days % 30;
        //}

        //internal static int _month(int days)
        //{
        //    return days % 30 == 0 ? (days % 360 == 0 ? 360 : days % 360) / 30 : days % 360 / 30 + 1;
        //}

        //internal static int _year(int days)
        //{
        //    return days % 360 == 0 ? days / 360 : days / 360 + 1;
        //}
    }
}
