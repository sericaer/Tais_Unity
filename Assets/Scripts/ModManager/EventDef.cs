using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SyntaxAnaylize;

namespace TaisEngine.ModManager
{
    internal class EventDef
    {
        internal class Element
        {
            public string name;

            public Expr<object[]> title;
            public Expr<object[]> desc;
            public Expr<bool> trigger;
            public Expr<double> occur_days;

            public List<OptionDef> options;

            public Element(SyntaxMod.Element modElem)
            {
                this.name = Path.GetFileNameWithoutExtension(modElem.filePath).ToUpper();

                object[] defTitle = { $"{name}_TITLE" };
                this.title = Expr_MultiValue.Parse(modElem, "title", defTitle);

                object[] defDesc = { $"{name}_DESC" };
                this.desc = Expr_MultiValue.Parse(modElem, "desc", defDesc);

                this.trigger = Expr_Condition.Parse(modElem, "trigger", false);
                this.occur_days = Expr_ModifierGroup.Parse(modElem, "occur_days", 1);

                this.options = OptionDef.ParseList(modElem, "option", name);
            }

            internal void Check()
            {
                foreach(var option in options)
                {
                    option.Check();
                }
            }
        }

        internal static List<Element> Anaylize(List<SyntaxMod.Element> modElemnts)
        {
            List<Element> rslt = new List<Element>();
            foreach (var elem in modElemnts)
            {
                rslt.Add(new Element(elem));
            }

            return rslt;
        }

        internal List<EventDef.Element> common;
        internal List<EventDef.Element> depart;

        static internal IEnumerable<EventDef.Element> Generate()
        {
            foreach (var mod in Mod.listMod.Where(x => x.content != null))
            {
                foreach (var elem in mod.content.eventDef.common)
                {
                    if(elem.trigger.Result())
                    {
                        if(Tools.GRandom.isOccurDays((int)elem.occur_days.Result()))
                        {
                            yield return elem;
                        }
                    }
                }

                foreach (var elem in mod.content.eventDef.depart)
                {
                    foreach(var depart in Run.RunData.inst.departs)
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
        }

        internal void Check()
        {
            foreach(var eventdef in common)
            {
                eventdef.Check();
            }

            foreach (var eventdef in depart)
            {
                eventdef.Check();
            }
        }
    }
}