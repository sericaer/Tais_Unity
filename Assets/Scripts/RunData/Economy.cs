using System;
using System.Collections.Generic;
using System.Linq;

using Newtonsoft.Json;
using TaisEngine.ModManager;

namespace TaisEngine.Run
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Economy //: DaysUpdate
    {
        [JsonProperty, VisitPropery("economy.value")]
        public double value;

        [JsonProperty, VisitPropery]
        public InComePopTax popTax;

        [JsonProperty, VisitPropery]
        public ExpendCountryTax countryTax;
        
        //[JsonProperty, VisitPropery("common.economy.tax_level_limit")]
        //public int tax_level_limit;

        //public void currTaxChanged(float value)
        //{
        //    curr_tax_level += value;

        //    validTaxChangedDays = RunData.inst.date.total_days + CommonDef.TaxLevel.Get().tax_change_intervl.Value;
        //}

        //[JsonProperty]
        //internal float curr_tax_level;

        //internal double curr_tax_per
        //{
        //    get
        //    {
        //        return CommonDef.TaxLevel.getInCome(curr_tax_level);
        //    }
        //}

        //[JsonProperty]
        //internal int validTaxChangedDays = 0;


        //internal enum TAX_LEVEL
        //{
        //    level0,
        //    level1,
        //    level2,
        //    level3,
        //    level4,
        //    level5,
        //    level6,
        //    levelmax = level6
        //}

        internal double currTax
        {
            get
            {
                return RunData.inst.pops.Where(x => x.is_tax).Sum(x => x.tax);
            }
        }

        //internal double maxTax
        //{
        //    get
        //    {
        //        return taxed_pop_num * Defines.getExpectTax(TAX_LEVEL.level5);
        //    }
        //}

        //internal int taxed_pop_num
        //{
        //    get
        //    {
        //        return RunData.inst.pops.Where(x => x.def.is_tax.Value && !x.depart.cancel_tax).Sum(x => (int)x.num);
        //    }
        //}

        [JsonProperty, VisitPropery("economy.surplus")]
        public double surplus
        {
            get
            {
                return InCome.all.Sum(x=>x.CalcCurrValue()) - Expend.all.Sum(x=>x.CalcCurrValue());
            }
        }

        //internal bool local_tax_change_valid
        //{
        //    get
        //    {
        //        return RunData.inst.date.total_days >= validTaxChangedDays;
        //    }
        //}

        internal Economy()
        {
        }

        internal Economy(int popTaxLevel)
        {
            //this.curr_tax_level = (int)level;
            popTax = new InComePopTax() { currLevel = popTaxLevel };
            countryTax = new ExpendCountryTax() { currLevel = 10 };

            Init();
        }

        internal void Init()
        {
            DaysTimer.Set((null, null, 30), () =>
            {
                if(surplus+value < 0)
                {
                    return "ECONOMY_NOT_SUPPORT_CURR_DEFICILT";
                }

                value += InCome.all.Sum(x => x.CalcCurrValue());

                foreach (var elem in Expend.all)
                {
                    value = elem.Do(value);
                }

                return null;
            });
        }

        //public override void DaysUpdateProcess()
        //{
        //    if (RunData.inst.date.day == 30)
        //    {
        //        //value += currTax;
        //        //value = RunData.inst.chaoting.ReportTax(value);
        //        value += InCome.all.Sum(x => x.CalcCurrValue());

        //        foreach (var elem in Expend.all)
        //        {
        //            value = elem.Do(value);
        //        }
        //    }
        //}

        //internal double getExpectTaxValue(float level)
        //{
        //    return RunData.inst.pops.Where(x => x.is_tax).Sum(x => x.GetExpectTax(level));
        //}
    }

    public abstract class InCome
    {
        public static List<InCome> all = new List<InCome>();

        public InCome()
        {
            all.Add(this);
        }

        public double CalcCurrValue()
        {
            return CalcExpandValue(GetCurrLevel());
        }

        public abstract int GetCurrLevel();

        public abstract double CalcExpandValue(int level);

        public abstract void SetLevel(int level);
    }

    public abstract class Expend
    {
        public static List<Expend> all = new List<Expend>();

        public Expend()
        {
            all.Add(this);
        }

        public double CalcCurrValue()
        {
            return CalcExpandValue(GetCurrLevel());
        }

        public abstract void SetLevel(int level);

        public abstract int GetCurrLevel();

        public abstract double CalcExpandValue(int level);

        internal abstract double Do(double income);
    }

    public class InComePopTax : InCome
    {
        public override double CalcExpandValue(int level)
        {
            return Math.Round(RunData.inst.pops.Where(x => x.is_tax).Sum(x => x.GetExpectTax(level)), 1);
        }

        public override int GetCurrLevel()
        {
            return currLevel;
        }

        public override void SetLevel(int level)
        {
            currLevel = level;
        }

        internal int currLevel;
    }

    public class ExpendCountryTax : Expend
    {
        public override double CalcExpandValue(int level)
        {
            return Math.Round(RunData.inst.chaoting.expect_tax * level / 10, 1);
        }

        public override int GetCurrLevel()
        {
            return currLevel;
        }

        public override void SetLevel(int level)
        {
            currLevel = level;
        }

        internal override double Do(double income)
        {
            double need = CalcCurrValue();

            RunData.inst.chaoting.year_expect_tax += need;

            if (income > need)
            {
                RunData.inst.chaoting.year_real_tax += need;
                return income - need;
            }
            else
            {
                RunData.inst.chaoting.year_real_tax += income;
                return 0;
            }
        }

        internal int currLevel;
    }
}
