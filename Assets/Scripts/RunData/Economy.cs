﻿using System;
using System.Linq;

using Newtonsoft.Json;
using TaisEngine.ModManager;

namespace TaisEngine.Run
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Economy
    {
        [JsonProperty, VisitPropery("common.economy")]
        public double value;

        [JsonProperty, VisitPropery("common.economy.tax_level_limit")]
        public int tax_level_limit;

        public void currTaxChanged(float value)
        {
            curr_tax_level += value;

            validTaxChangedDays = RunData.inst.date.total_days + CommonDef.TaxLevel.Get().tax_change_intervl.Value;
        }

        [JsonProperty]
        internal float curr_tax_level;

        //internal double curr_tax_per
        //{
        //    get
        //    {
        //        return CommonDef.TaxLevel.getInCome(curr_tax_level);
        //    }
        //}

        [JsonProperty]
        internal int validTaxChangedDays = 0;


        internal enum TAX_LEVEL
        {
            level0,
            level1,
            level2,
            level3,
            level4,
            level5,
            level6,
            levelmax = level6
        }

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

        internal double surplus
        {
            get
            {
                return currTax - RunData.inst.chaoting.expect_tax;
            }
        }

        internal bool local_tax_change_valid
        {
            get
            {
                return RunData.inst.date.total_days >= validTaxChangedDays;
            }
        }

        internal Economy()
        {
        }

        internal Economy(TAX_LEVEL level)
        {
            this.curr_tax_level = (int)level;
        }

        internal void DayInc()
        {
            if(RunData.inst.date.day == 30)
            {
                value += currTax;
                value = RunData.inst.chaoting.ReportTax(value);
            }
        }

        internal double getExpectTaxValue(float level)
        {
            return RunData.inst.pops.Where(x => x.is_tax).Sum(x => x.GetExpectTax(level));
        }
    }
}
