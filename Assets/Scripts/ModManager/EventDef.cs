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


        //public EventDef(SyntaxMod syntaxMod)
        //{
        //    var modElems = syntaxMod.GetElements("event");
        //    if (modElems == null)
        //    {
        //        return;
        //    }

        //    foreach (var modElem in modElems)
        //    {
        //        var elem = new Element(modElem.name, modElem.multiItem);
        //        commons.Add(elem);
        //    }
        //}

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
    }
}