using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SyntaxAnaylize;

namespace TaisEngine.ModManager
{
    internal class BufferGroup
    {
        internal static Dictionary<string, List<BufferInterface>> dict;

        static BufferGroup()
        {
            dict = new Dictionary<string, List<BufferInterface>>();
        }

        internal List<BufferDefDepart> depart = new List<BufferDefDepart>();

        internal static BufferInterface FindByName(string name)
        {
            return BufferDefDepart.Find(name);
        }
    }

    internal interface BufferInterface
    {
        Tuple<string, double> effect_crop_growing_speed { get; }
    }

    internal class BufferDef<T> : BaseDef<T>, BufferInterface where T : new()
    {
        [ModProperty("name")]
        public string name;

        [ModProperty("effect")]
        public Dictionary<string, double> effect;

        public Tuple<string, double> effect_crop_growing_speed
        {
            get
            {
                if(effect.ContainsKey("crop_growing_speed"))
                {
                    return new Tuple<string, double>(name, effect["crop_growing_speed"]);
                }
                return null;
            }
        }

        internal void SetDefault()
        {
            if (effect == null)
            {
                effect = new Dictionary<string, double>();
            }
        }
    }

    internal class BufferDefDepart : BufferDef<BufferDefDepart>
    {

    }

    //internal class EffectDef
    //{
    //    string raw;
    //    Func<double, double> func;
    //}

    //internal class EventDefCommon : BufferDef<EventDefCommon>
    //{

    //}


    //internal class BufferDef
    //{
    //[ModProperty("name")]
    //public string name;

    //internal class Element
    //{
    //    [ModProperty("name")]
    //    public string name;

    //    //public Expr<object[]> title;
    //    //public Expr<object[]> desc;
    //    //public Expr<bool> trigger;
    //    //public Expr<double> occur_days;

    //    //public List<OptionDef> options;

    //    //public Element(SyntaxMod.MultiItem modElem)
    //    //{
    //    //    this.name = Path.GetFileNameWithoutExtension(modElem.filePath).ToUpper();

    //    //    //object[] defTitle = { $"{name}_TITLE" };
    //    //    //this.title = Expr_MultiValue.Parse(modElem, "title", defTitle);

    //    //    //object[] defDesc = { $"{name}_DESC" };
    //    //    //this.desc = Expr_MultiValue.Parse(modElem, "desc", defDesc);

    //    //    //this.trigger = Expr_Condition.Parse(modElem, "trigger", false);
    //    //    //this.occur_days = Expr_ModifierGroup.Parse(modElem, "occur_days", 1);

    //    //    //this.options = OptionDef.ParseList(modElem, "option", name);
    //    //}
    //}

    //internal static Element FindByName(string name)
    //{
    //    foreach(var elem in Mod.listMod.Where(x => x.content != null))
    //    {
    //        foreach(var list in elem.content.bufferDef.dictGroup.Values)
    //        {
    //            var buffDef = list.Find(x => x.name == name);
    //            if(buffDef != null)
    //            {
    //                return buffDef;
    //            }
    //        }
    //    }

    //    throw new Exception();
    //}

    //internal static List<Element> Anaylize(List<SyntaxMod.MultiItem> modElemnts)
    //{
    //    List<Element> rslt = new List<Element>();
    //    foreach(var elem in modElemnts)
    //    {
    //        rslt.Add(new Element(elem));
    //    }

    //    return rslt;
    //}


    //internal static Dictionary<string, Element> GetGroupDict(string groupName)
    //{
    //    return Mod.listMod.Where(x => x.content != null)
    //                      .SelectMany(x => x.content.bufferDef.dictGroup[groupName])
    //                      .ToDictionary(k => k.name, v => v);
    //}


    ////internal List<BufferDef.Element> commons = new List<Element>();
    //internal Dictionary<string, List<Element>> dictGroup = new Dictionary<string, List<Element>>();

    //internal List<Element> GetGroup(string groupName)
    //{
    //    if(!dictGroup.ContainsKey(groupName))
    //    {
    //        dictGroup.Add(groupName, new List<Element>());
    //    }

    //    return dictGroup[groupName];
    //}

    //static internal IEnumerable<BufferDef.Element> Enumerate()
    //{
    //    foreach(var mod in Mod.listMod.Where(x=>x.content != null))
    //    {
    //        foreach(var elem in mod.content.eventDef.commons)
    //        {
    //            yield return elem;
    //        }
    //    }
    //}

    //static internal IEnumerable<EventDef.Element> Generate()
    //{
    //    foreach (var mod in Mod.listMod.Where(x => x.content != null))
    //    {
    //        foreach (var elem in mod.content.eventDef.commons)
    //        {
    //            if(elem.trigger.Result())
    //            {
    //                if(Tools.GRandom.isOccurDays((int)elem.occur_days.Result()))
    //                {
    //                    yield return elem;
    //                }
    //            }
    //        }
    //    }
    //}
    //}
}