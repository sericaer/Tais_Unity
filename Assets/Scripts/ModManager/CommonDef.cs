using SyntaxAnaylize;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TaisEngine.ModManager
{
    class CommonDef
    {
        internal CropGrowingInfo cropGrowingInfo;
        internal TaxLevel taxLevel;

        public CommonDef(IEnumerable<MultiItem> modElemnts)
        {
            foreach(var modElem in modElemnts)
            {
                if(modElem.Find<SingleValue>("name").value == "CROP_GROWING")
                {
                    this.cropGrowingInfo = ModAnaylize.Parse<CropGrowingInfo>(modElem);
                }

                if (modElem.Find<SingleValue>("name").value == "TAX_LEVEL")
                {
                    this.taxLevel = ModAnaylize.Parse<TaxLevel>(modElem);
                }
            }
        }

        internal static (int month, int day) getCropGrowingStartDate()
        {
            foreach (var mod in Mod.listMod.Where(x => x.content != null && x.content.commonDef != null))
            {
                var startData = mod.content.commonDef.cropGrowingInfo.startDate;
                if (startData != null)
                {
                    return (startData.month.Value, startData.day.Value);
                }
            }

            throw new Exception();
        }

        internal static (int month, int day) getCropGrowingEndDate()
        {
            foreach (var mod in Mod.listMod.Where(x => x.content != null && x.content.commonDef != null))
            {
                var endData = mod.content.commonDef.cropGrowingInfo.endDate;
                if (endData != null)
                {
                    return (endData.month.Value, endData.day.Value);
                }
            }

            throw new Exception();
        }

        internal static double getCropGrowingSpeed()
        {
            foreach(var mod in Mod.listMod.Where(x=>x.content != null))
            {
                if(mod.content.commonDef.cropGrowingInfo.base_speed != null)
                {
                    return mod.content.commonDef.cropGrowingInfo.base_speed.Value;
                }
            }

            throw new Exception();
        }

        internal class CropGrowingInfo
        {
            [ModProperty("base_speed")]
            internal double? base_speed;

            [ModProperty("start")]
            internal Date startDate;

            [ModProperty("end")]
            internal Date endDate;

            internal class Date
            {
                [ModProperty("month")]
                internal int? month;

                [ModProperty("day")]
                internal int? day;
            }
        }

        internal class TaxLevel
        {
            internal static List<(TaxLevel, string)> list = new List<(TaxLevel, string)>();

            internal static double getInCome(float curr_tax_level)
            {
                foreach (var mod in Mod.listMod.Where(x => x.content != null && x.content.commonDef != null))
                {
                    var taxLevel = mod.content.commonDef.taxLevel;
                    return taxLevel.getInComeImp(curr_tax_level);
                }

                throw new Exception();
            }

            internal static double getConsume(float curr_tax_level)
            {
                foreach (var mod in Mod.listMod.Where(x => x.content != null && x.content.commonDef != null))
                {
                    var taxLevel = mod.content.commonDef.taxLevel;
                    return taxLevel.getConsumeImp(curr_tax_level);
                }

                throw new Exception();
            }

            private double getInComeImp(float curr_tax_level)
            {
                var levelbase = (int)curr_tax_level;
                var valueBase = levels.Single(x => x.value == levelbase).income.Value;
                if(levelbase == (int)Run.Economy.TAX_LEVEL.levelmax)
                {
                    return valueBase;
                }

                var valueNext = levels.Single(x => x.value == levelbase+1).income.Value;
                var leveloffset = curr_tax_level - levelbase;

                return valueBase + leveloffset * (valueNext - valueBase);
            }

            private double getConsumeImp(float curr_tax_level)
            {
                var levelbase = (int)curr_tax_level;
                var valueBase = levels.Single(x => x.value == levelbase).consume.Value;
                if (levelbase == (int)Run.Economy.TAX_LEVEL.levelmax)
                {
                    return valueBase;
                }

                var valueNext = levels.Single(x => x.value == levelbase + 1).consume.Value;
                var leveloffset = curr_tax_level - levelbase;

                return valueBase + leveloffset * (valueNext - valueBase);
            }

            internal static int getTaxChangedIntervlDays()
            {
                foreach (var mod in Mod.listMod.Where(x => x.content != null && x.content.commonDef != null))
                {
                    var taxLevel = mod.content.commonDef.taxLevel;
                    return taxLevel.tax_change_intervl.Value;
                }

                throw new Exception();
            }

            [ModProperty("tax_change_intervl")]
            int? tax_change_intervl;

            [ModPropertyList("level")]
            internal List<LEVEL_INFO> levels;

            internal class LEVEL_INFO
            {
                [ModProperty("value")]
                internal int? value;

                [ModProperty("income")]
                internal double? income;

                [ModProperty("consume")]
                internal double? consume;
            }
        }
    }
}
