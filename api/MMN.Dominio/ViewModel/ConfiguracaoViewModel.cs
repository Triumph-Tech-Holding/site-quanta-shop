using System;
using System.Collections.Generic;
using System.Text;

namespace MMN.Dominio.ViewModel
{
    public class ConfiguracaoViewModel
    {
        public int IdConfiguracao { get; set; }
        public string Chave { get; set; }
        public string Valor { get; set; }
        public string Descricao { get; set; }
        public bool Ativo { get; set; }
    }
}
