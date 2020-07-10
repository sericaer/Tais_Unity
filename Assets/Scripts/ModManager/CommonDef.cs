using SyntaxAnaylize;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TaisEngine.ModManager
{
    class CommonDef
    {
        internal CropGrowingInfo cropGrowingInfo;
        internal TaxLevel taxLevel;

        internal static void AnaylizeMod(Mod mod, SyntaxModElement modElemnt)
        {
            var fileName = Path.GetFileNameWithoutExtension(modElemnt.filePath).ToUpper();
            switch (fileName)
            {
                case "CROP_GROWING":
                    mod.content.commonDef.cropGrowingInfo = CropGrowingInfo.Parse(mod.info.name, modElemnt);
                    break;
                case "TAX_LEVEL":
                    mod.content.commonDef.taxLevel = TaxLevel.Parse(mod.info.name, modElemnt);
                    break;
            }
        }

        internal class CropGrowingInfo : BaseDef<CropGrowingInfo>
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

            internal override void SetDefault()
            {

            }
        }

        internal class TaxLevel : BaseDef<TaxLevel>
        {
            [ModProperty("tax_change_intervl")]
            internal int? tax_change_intervl;

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

            internal static double getInCome(double curr_tax_level)
            {
                var levels = Get().levels;

                var levelbase = (int)curr_tax_level;
                var valueBase = levels.Single(x => x.value == levelbase).income.Value;
                if (levelbase == (int)Run.Economy.TAX_LEVEL.levelmax)
                {
                    return valueBase;
                }

                var valueNext = levels.Single(x => x.value == levelbase + 1).income.Value;
                var leveloffset = curr_tax_level - levelbase;

                return valueBase + leveloffset * (valueNext - valueBase);
            }

            internal static double getConsume(double curr_tax_level)
            {
                var levels = Get().levels;

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

            internal override void SetDefault()
            {

            }
        }
    }
}
