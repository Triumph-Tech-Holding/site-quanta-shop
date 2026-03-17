using System;
using System.Collections.Generic;
using System.Text;
using MMN.Dominio.Model;

namespace MMN.Dominio.ViewModel
{
    public class StatusViewModel
    {
        public int IdStatus { get; set; }
        public string Nome { get; set; }
        public string ChaveTraducao { get; set; }
        public bool? Ativo { get; set; }
        //public virtual ICollection<Lancamento> Lancamento { get; set; }
        //public virtual ICollection<Transacao> Transacao { get; set; }
    }
}

