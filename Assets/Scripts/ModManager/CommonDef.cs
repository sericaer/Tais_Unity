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

        public CommonDef(List<SyntaxMod.MultiItem> modElemnts)
        {
            var cropMod = modElemnts.SingleOrDefault(x => x.filePath.EndsWith("crop_growing.txt"));
            if(cropMod != null)
            {
                this.cropGrowingInfo = ModAnaylize.Parse<CropGrowingInfo>(cropMod.multiItem);
            }

            var taxLevelMod = modElemnts.SingleOrDefault(x => x.filePath.EndsWith("tax_level.txt"));
            if (taxLevelMod != null)
            {
                this.taxLevel = ModAnaylize.Parse<TaxLevel>(taxLevelMod.multiItem);
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
                if(mod.content.commonDef.cropGrowingInfo.fullGrowDays != null)
                {
                    return 100.0 / mod.content.commonDef.cropGrowingInfo.fullGrowDays.Value;
                }
            }

            throw new Exception();
        }

        internal class CropGrowingInfo
        {
            [ModProperty("full_grow_days")]
            internal int? fullGrowDays;

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

            private double getInComeImp(float curr_tax_level)
            {
                var levelbase = (int)curr_tax_level;
                var valueBase = popInitDict[levelbase.ToString()].income.Value;
                if(levelbase == (int)Run.Economy.TAX_LEVEL.levelmax)
                {
                    return valueBase;
                }

                var valueNext = popInitDict[(levelbase+1).ToString()].income.Value;
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

            [ModProperty("level")]
            internal Dictionary<string, LEVEL_INFO> popInitDict;

            internal class LEVEL_INFO
            {
                [ModProperty("income")]
                internal double? income;

                [ModProperty("consume")]
                internal double? consume;
            }
        }
    }
}
