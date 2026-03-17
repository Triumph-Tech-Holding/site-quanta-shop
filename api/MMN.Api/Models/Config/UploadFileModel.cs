using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MMN.Api.Models.Config
{
    public class UploadFileModel
    {
        public string Diretorio { get; set; }
        public string Base64 { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
    }
}
