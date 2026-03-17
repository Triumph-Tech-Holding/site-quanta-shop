using MMN.Dominio.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace MMN.Dominio.ViewModel
{
    public class ParceiroViewModel
    {
        public int IdParceiro { get; set; }

        public Guid IdCredenciado { get; set; }

        public string Descricao { get; set; }
        public string Nome { get; set; }

        public bool Ativo { get; set; }
        public string Celular { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataAtualizacao { get; set; }
    }
}
