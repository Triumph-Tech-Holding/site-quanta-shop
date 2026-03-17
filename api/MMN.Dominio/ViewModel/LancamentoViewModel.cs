using System;
using System.Collections.Generic;
using System.Text;
using MMN.Dominio.Model;

namespace MMN.Dominio.ViewModel
{
    public class LancamentoViewModel
    {
        public long IdLancamento { get; set; }
        public Guid IdUsuario { get; set; }
        public long IdTransacao { get; set; }
        public int IdTipo { get; set; }
        public decimal Valor { get; set; }
        public DateTime DataLancamento { get; set; }
        public bool Ativo { get; set; }
        public string Descricao { get; set; }
        public int IdStatus { get; set; }
        public bool Bloqueado { get; set; }
        public DateTime? DataReferencia { get; set; }
        public int OrdemExibicao { get; set; }
        public virtual TipoViewModel Tipo { get; set; }
        public virtual TransacaoViewModel Transacao { get; set; }
        public virtual UsuarioViewModel Usuario { get; set; }
        public virtual ICollection<LancamentoRetidoViewModel> LancamentoRetido { get; set; }
    }
}
