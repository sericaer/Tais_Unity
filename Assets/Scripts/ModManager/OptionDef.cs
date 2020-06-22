using System;
using SyntaxAnaylize;

namespace TaisEngine.ModManager
{
    internal class OptionDef
    {
        public string name;
        public Eval<string> desc;
        public EvalSelected selected;
        public Eval<string> next_select;

        private Value opRaw;

        public OptionDef(string name, Value opRaw)
        {
            this.name = name;
            this.desc = Eval<string>.Parse("desc", opRaw as MultiItem, $"{name}_DESC");
            this.selected = EvalSelected.Parse("selected", opRaw as MultiItem);
            this.next_select = Eval<string>.Parse("next_select", opRaw as MultiItem, "");
            this.opRaw = opRaw;
        }
    }
}
