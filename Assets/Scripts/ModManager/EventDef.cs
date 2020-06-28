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
        }

        internal List<EventDef.Element> commons = new List<Element>();
        internal List<EventDef.Element> departs = new List<Element>();

        internal void CreateCommons(List<SyntaxMod.Element> modElements)
        {
            foreach (var modElem in modElements)
            {
                var elem = new Element(modElem);
                commons.Add(elem);
            }
        }

        static internal IEnumerable<EventDef.Element> Enumerate()
        {
            foreach(var mod in Mod.listMod.Where(x=>x.content != null))
            {
                foreach(var elem in mod.content.eventDef.commons)
                {
                    yield return elem;
                }
            }
        }

        static internal IEnumerable<EventDef.Element> Generate()
        {
            foreach (var mod in Mod.listMod.Where(x => x.content != null))
            {
                foreach (var elem in mod.content.eventDef.commons)
                {
                    if(elem.trigger.Result())
                    {
                        if(Tools.GRandom.isOccurDays((int)elem.occur_days.Result()))
                        {
                            yield return elem;
                        }
                    }
                }
            }
        }
    }
}