using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SyntaxAnaylize;

namespace TaisEngine.ModManager
{
    internal class EventGroup
    {
        internal List<EventDefCommon> common = new List<EventDefCommon>();
        internal List<EventDefDepart> depart = new List<EventDefDepart>();

        static internal IEnumerable<EventInterface> Generate()
        {
            foreach (var elem in EventDefCommon.Enumerate())
            {
                if (elem.trigger.Result())
                {
                    if (Tools.GRandom.isOccurDays((int)elem.occur_days.Result()))
                    {
                        yield return elem;
                    }
                }
            }

            foreach (var elem in EventDefDepart.Enumerate())
            {
                foreach (var depart in Run.RunData.inst.departs)
                {
                    Visitor.SetObj("depart", depart);

                    if (elem.trigger.Result())
                    {
                        if (Tools.GRandom.isOccurDays((int)elem.occur_days.Result()))
                        {
                            yield return elem;
                        }
                    }

                    Visitor.SetObj("depart", null);
                }
            }
        }

        internal static EventInterface Find(string next_event)
        {
            EventInterface rslt = EventDefDepart.TryFind(next_event);
            if(rslt != null)
            {
                return rslt;
            }
            rslt = EventDefCommon.TryFind(next_event);
            if (rslt != null)
            {
                return rslt;
            }

            return null;
        }

        internal void Check()
        {
            foreach (var elem in EventDefCommon.Enumerate())
            {
                elem.Check();
            }

            foreach (var elem in EventDefDepart.Enumerate())
            {
                elem.Check();
            }
        }
    }

    internal class EventDefCommon : EventDef<EventDefCommon>
    {
        internal static void AnaylzeMod(Mod mod, SyntaxModElement modElemnts)
        {
            mod.content.eventGroup.common.Add(EventDefCommon.Parse(mod.info.name, modElemnts));
        }
    }

    internal class EventDefDepart : EventDef<EventDefDepart>
    {
        internal static void AnaylzeMod(Mod mod, SyntaxModElement modElemnts)
        {
            mod.content.eventGroup.depart.Add(EventDefDepart.Parse(mod.info.name, modElemnts));
        }
    }

    internal interface EventInterface
    {
        object[] GetTitle();
        object[] GetDesc();

        List<OptionDef> GetOption();
    }

    internal class EventDef<T> : BaseDefMulti<T>, EventInterface where T : ModAbstractNamed, new()
    {
        [ModProperty("name")]
        public string name;

        [ModProperty("title")]
        public Expr_MultiValue title;

        [ModProperty("desc")]
        public Expr_MultiValue desc;

        [ModProperty("trigger")]
        public Expr_Condition trigger;

        [ModProperty("occur_days")]
        public Expr_ModifierGroup occur_days;

        [ModPropertyList("option")]
        public List<OptionDef> options;

        public object[] GetDesc()
        {
            return desc.Result();
        }

        public object[] GetTitle()
        {
            return title.Result();
        }

        public List<OptionDef> GetOption()
        {
            return options;
        }

        internal override string GetName()
        {
            return name;
        }

        internal override void SetDefault()
        {
            if (title == null)
            {
                title = new Expr_MultiValue(name + "_TITLE");
            }
            if (desc == null)
            {
                desc = new Expr_MultiValue(name + "_DESC");
            }
            if (trigger == null)
            {
                trigger = new Expr_SingleCondition(false);
            }
            if (occur_days == null)
            {
                occur_days = new Expr_ModifierGroup(1);
            }

            for (int i = 0; i < options.Count(); i++)
            {
                var op = options[i];

                if (op.desc == null)
                {
                    op.desc = new Expr_MultiValue($"{name}_OPTION_{i + 1}_DESC");
                }
            }
        }

        internal void Check()
        {
            //trigger.Check();
            //occur_days.Check();

            foreach (var op in options)
            {
                op.Check();
            }
        }

        //internal class Element
        //{
        //    public string name;

        //    public Expr<object[]> title;
        //    public Expr<object[]> desc;
        //    public Expr<bool> trigger;
        //    public Expr<double> occur_days;

        //    public List<OptionDef> options;

        //    public Element(SyntaxMod.MultiItem modElem)
        //    {
        //        this.name = Path.GetFileNameWithoutExtension(modElem.filePath).ToUpper();

        //        object[] defTitle = { $"{name}_TITLE" };
        //        this.title = Expr_MultiValue.Parse(modElem, "title", defTitle);

        //        object[] defDesc = { $"{name}_DESC" };
        //        this.desc = Expr_MultiValue.Parse(modElem, "desc", defDesc);

        //        this.trigger = Expr_Condition.Parse(modElem, "trigger", false);
        //        this.occur_days = Expr_ModifierGroup.Parse(modElem, "occur_days", 1);

        //        //this.options = OptionDef.ParseList(modElem, "option", name);
        //    }

        //    internal void Check()
        //    {
        //        foreach(var option in options)
        //        {
        //            option.Check();
        //        }
        //    }
        //}

        //internal static List<Element> Anaylize(List<SyntaxMod.MultiItem> modElemnts)
        //{
        //    List<Element> rslt = new List<Element>();
        //    foreach (var elem in modElemnts)
        //    {
        //        rslt.Add(new Element(elem));
        //    }

        //    return rslt;
        //}

        //internal List<EventDef.Element> common;
        //internal List<EventDef.Element> depart;

        //static internal IEnumerable<EventDef.Element> Generate()
        //{
        //    foreach (var mod in Mod.listMod.Where(x => x.content != null))
        //    {
        //        foreach (var elem in mod.content.eventDef.common)
        //        {
        //            if(elem.trigger.Result())
        //            {
        //                if(Tools.GRandom.isOccurDays((int)elem.occur_days.Result()))
        //                {
        //                    yield return elem;
        //                }
        //            }
        //        }

        //        foreach (var elem in mod.content.eventDef.depart)
        //        {
        //            foreach(var depart in Run.RunData.inst.departs)
        //            {
        //                Visitor.SetObj("depart", depart);

        //                if (elem.trigger.Result())
        //                {
        //                    if (Tools.GRandom.isOccurDays((int)elem.occur_days.Result()))
        //                    {
        //                        yield return elem;
        //                    }
        //                }

        //                Visitor.SetObj("depart", null);
        //            }
        //        }
        //    }
        //}

        //internal void Check()
        //{
        //    foreach(var eventdef in common)
        //    {
        //        eventdef.Check();
        //    }

        //    foreach (var eventdef in depart)
        //    {
        //        eventdef.Check();
        //    }
        //}

        //public bool isTrigger()
        //{
        //    throw new NotImplementedException();
        //}

    }
}