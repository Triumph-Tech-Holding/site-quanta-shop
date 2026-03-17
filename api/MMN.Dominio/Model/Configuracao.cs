using System;
using System.Collections.Generic;
using System.Text;

namespace MMN.Dominio.Model
{
    public class Configuracao
    {
        public int IdConfiguracao { get; set; }
        public string Chave { get; set; }
        public string Valor { get; set; }
        public string Descricao { get; set; }
        public bool Ativo { get; set; }
        public bool Editavel { get; set; }
        public string Tipo { get; set; }
    }
}
