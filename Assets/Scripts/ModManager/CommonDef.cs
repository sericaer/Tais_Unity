using SyntaxAnaylize;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TaisEngine.ModManager
{
    class CommonDef
    {
        internal CropGrowingInfo cropGrowingInfo;

        public CommonDef(List<SyntaxMod.Element> modElemnts)
        {
            var defines = modElemnts.SingleOrDefault(x => x.filePath.EndsWith("defines.txt"));
            if(defines != null)
            {
                this.cropGrowingInfo = new CropGrowingInfo(defines.multiItem.TryFind<MultiItem>("crop_growing"));
            }
        }

        internal static (int month, int day) getCropGrowingStartDate()
        {
            foreach (var mod in Mod.listMod.Where(x => x.content != null && x.content.commonDef != null))
            {
                if (mod.content.commonDef.cropGrowingInfo.startDate != null)
                {
                    return (mod.content.commonDef.cropGrowingInfo.startDate.month, mod.content.commonDef.cropGrowingInfo.startDate.day);
                }
            }

            throw new Exception();
        }

        internal static (int month, int day) getCropGrowingEndDate()
        {
            foreach (var mod in Mod.listMod.Where(x => x.content != null && x.content.commonDef != null))
            {
                if (mod.content.commonDef.cropGrowingInfo.endDate != null)
                {
                    return (mod.content.commonDef.cropGrowingInfo.endDate.month, mod.content.commonDef.cropGrowingInfo.endDate.day);
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
            internal int? fullGrowDays;
            internal Date startDate;
            internal Date endDate;

            public CropGrowingInfo(MultiItem multiItem)
            {
                var rawFullGrowDays = multiItem.TryFind<SingleValue>("full_grow_days");
                if(rawFullGrowDays != null)
                {

                }
            }

            internal class Date
            {
                internal int month;
                internal int day;
            }

        }
    }
}
