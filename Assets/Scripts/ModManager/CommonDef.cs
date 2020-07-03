using SyntaxAnaylize;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TaisEngine.ModManager
{
    class CommonDef
    {
        internal CropGrowingInfo cropGrowingInfo;

        public CommonDef(List<SyntaxMod.MultiItem> modElemnts)
        {
            var cropMod = modElemnts.SingleOrDefault(x => x.filePath.EndsWith("crop_growing.txt"));
            if(cropMod != null)
            {
                this.cropGrowingInfo = ModAnaylize.Parse<CropGrowingInfo>(cropMod.multiItem);
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
    }
}
