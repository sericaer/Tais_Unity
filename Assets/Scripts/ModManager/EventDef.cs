using System;
using System.Collections.Generic;
using System.Linq;
using SyntaxAnaylize;

namespace TaisEngine.ModManager
{
    internal class EventDef
    {
        internal class Element
        {
            public string name;

            public EvalExpr_MultiValue title;
            public EvalExpr_MultiValue desc;
            public EvalExpr_Condition trigger;
            public EvalExpr_ModifierGroup occur_days;

            public List<OptionDef> options;

            public Element(string name, MultiItem modItems)
            {
                this.name = name.ToUpper();

                object[] defTitle = { $"{name}_TITLE" };
                this.title = EvalExpr_MultiValue.Parse(modItems, "title", defTitle);

                object[] defDesc = { $"{name}_DESC" };
                this.desc = EvalExpr_MultiValue.Parse(modItems, "desc", defDesc);

                this.trigger = EvalExpr_Condition.Parse(modItems, "trigger", false);
                this.occur_days = EvalExpr_ModifierGroup.Parse(modItems, "occur_days", 1);

                this.options = new List<OptionDef>();
                var rawOptions = modItems.elems.Where(x => x.key == "option").ToArray();
                for (int i = 0; i < rawOptions.Count(); i++)
                {
                    this.options.Add(new OptionDef($"{name}_OPTION_{i + 1}", rawOptions[i].value as MultiItem));
                }
            }
        }

        internal List<EventDef.Element> commons = new List<Element>();
        internal List<EventDef.Element> departs = new List<Element>();

        internal void CreateCommons(List<SyntaxMod.Element> modElements)
        {
            foreach (var modElem in modElements)
            {
                var elem = new Element(modElem.name, modElem.multiItem);
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