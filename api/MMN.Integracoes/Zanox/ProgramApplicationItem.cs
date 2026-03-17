using System.Text.Json.Serialization;

namespace MMN.Integracoes.Zanox
{
    public class ProgramApplicationItem
    {
        public Program program { get; set; }
        public Adspace adspace { get; set; }
        public string status { get; set; }
    }
}
