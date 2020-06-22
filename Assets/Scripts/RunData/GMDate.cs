using System;
using TaisEngine.ModManager;
using UnityEngine.UI.Extensions;

namespace TaisEngine.Run
{
    public class GMDate
    {
        //internal bool isSpring
        //{
        //    get
        //    {
        //        return (month < 4);
        //    }
        //}
        //internal bool isSummer
        //{
        //    get
        //    {
        //        return (month < 7 && month >=4);
        //    }
        //}
        //internal bool isAutumn
        //{
        //    get
        //    {
        //        return (month < 10 && month >= 7);
        //    }
        //}
        //internal bool isWinter
        //{
        //    get
        //    {
        //        return (month <= 12 && month >= 10);
        //    }
        //}

        public int total_days
        {
            get
            {
                return RunData.inst.days;
            }
        }

        public int year
        {
            get
            {
                return _year(total_days);
            }

        }

        public int month
        {
            get
            {
                return _month(total_days);
            }
        }

        public int day
        {
            get
            {
                return _day(total_days);
            }
        }

        public override string ToString()
        {
            if (total_days == 0)
            {
                return "--";
            }

            return LocalString.Get("date", year, month, day);
        }

        //public static string ToString(int days)
        //{
        //    if (days == 0)
        //    {
        //        return "--";
        //    }

        //    return Mod.GetLocalString("date", _year(days), _month(days), _day(days));
        //}

        internal static int _day(int days)
        {
            return days % 30 == 0 ? 30 : days % 30;
        }

        internal static int _month(int days)
        {
            return days % 30 == 0 ? (days % 360 == 0 ? 360 : days % 360) / 30 : days % 360 / 30 + 1;
        }

        internal static int _year(int days)
        {
            return days % 360 == 0 ? days / 360 : days / 360 + 1;
        }
    }
}
