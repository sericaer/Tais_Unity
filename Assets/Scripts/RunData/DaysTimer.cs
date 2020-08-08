using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaisEngine.Run
{
    class DaysTimer
    {
        internal static List<DaysTimer> all = new List<DaysTimer>();

        internal static void Set((int? year, int? month, int?day) date, Action act)
        {
            var finded = all.SingleOrDefault(x => x.isSameDate(date));
            if(finded != null)
            {
                finded.actions.Add(act);
            }

            all.Add(new DaysTimer(date, act));
        }

        internal DaysTimer((int? year, int? month, int? day) date, Action act)
        {
            this.date = date;
            this.actions.Add(act);
        }

        internal void OnTimer()
        {
            foreach(var act in actions)
            {
                act();
            }
        }

        internal bool isSameDate((int? year, int? month, int? day) date)
        {
            if(this.date.year != date.year)
            {
                return false;
            }
            if (this.date.month != date.month)
            {
                return false;
            }
            if (this.date.day != date.day)
            {
                return false;
            }

            return true;
        }

        internal (int? year, int? month, int? day) date;
        List<Action> actions = new List<Action>();
    }
}
