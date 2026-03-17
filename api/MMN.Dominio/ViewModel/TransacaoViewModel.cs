using System;
using System.Collections.Generic;
using System.Text;

namespace MMN.Dominio.ViewModel
{
    public class TransacaoViewModel
    {
        public long IdTransacao { get; set; }
        public Guid IdUsuario { get; set; }
        public int IdTipo { get; set; }
        public decimal ValorPrincipal { get; set; }
        public DateTime DataTransacao { get; set; }
        public string Descricao { get; set; }
        public int IdStatus { get; set; }
        public bool Ativo { get; set; }
        public Guid? IdVendaZanox { get; set; }
        public int? IdAnunciante { get; set; }
        public decimal? ComissaoTotal { get; set; }
        public long? IdVendaAfilio { get; set; }
        public virtual StatusViewModel StatusViewModel { get; set; }
        public virtual TipoViewModel TipoViewModel { get; set; }
        public virtual UsuarioViewModel UsuarioViewModel { get; set; }
        public virtual AnuncianteViewModel AnuncianteViewModel { get; set; }
        //public virtual ICollection<LancamentoViewModel> Lancamento { get; set; }
        public virtual ICollection<SaqueViewModel> SaqueViewModel { get; set; }
    }
}
