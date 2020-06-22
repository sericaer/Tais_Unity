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

            public Eval<string> title;
            public Eval<string> desc;
            public Eval<bool> trigger;
            public Eval<int> occur_days;

            public List<OptionDef> options;

            public Element(string name, MultiItem modItems)
            {
                this.name = name.ToUpper();
                this.title = Eval<string>.Parse("title", modItems, $"{name}_TITLE");
                this.desc = Eval<string>.Parse("desc", modItems, $"{name}_DESC");
                this.trigger = Eval<bool>.Parse("trigger", modItems, false);
                this.occur_days = Eval<int>.Parse("occur_days", modItems, 1);

                this.options = new List<OptionDef>();
                var rawOptions = modItems.elems.Where(x => x.key == "option").ToArray();
                for (int i = 0; i < rawOptions.Count(); i++)
                {
                    this.options.Add(new OptionDef($"{name}_OPTION_{i + 1}", rawOptions[i].value));
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