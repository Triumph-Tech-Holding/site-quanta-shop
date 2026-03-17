using System;
using System.Collections.Generic;
using System.Text;
using MMN.Dominio.Model;

namespace MMN.Dominio.ViewModel
{
    public class TipoViewModel
    {
        public int IdTipo { get; set; }
        public int IdTipoPai { get; set; }
        public string Descricao { get; set; }
        public string Chave { get; set; }
        public bool Ativo { get; set; }
    }
}
