using System;
using System.Collections.Generic;
using System.Text;

namespace MMN.Integracoes.Zanox
{
    public class ProgramApp
    {
        public int page { get; set; }
        public int items { get; set; }
        public int total { get; set; }
        public programApplicationItems programApplicationItems { get; set; }
    }
    public class programApplicationItems
    {
        public List<ProgramApplicationItem> programApplicationItem { get; set; }
    }
}
