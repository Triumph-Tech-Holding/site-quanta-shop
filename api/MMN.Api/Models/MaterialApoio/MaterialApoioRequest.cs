using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MMN.Api.Models.MaterialApoio
{
    public class MaterialApoioRequest
    {
        public int? IdMaterial { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string Base64 { get; set; }
        public bool Ativo { get; set; }
        public string Diretorio { get; set; }
    }
}
