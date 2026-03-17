using MMN.Util.Enum;
using System;
using System.Collections.Generic;

namespace MMN.Dominio.Model
{
    public class Suporte
    {
        public int IdSuporte { get; set; }
        public DateTime DataCompra { get; set; }
        public string SiteCompra { get; set; }
        public string NumeroPedido { get; set; }
        public decimal ValorPedido { get; set; }
        public string Observacao { get; set; }
        public string UrlComprovante { get; set; }
        public DateTime DataSolicitacao { get; set; }
        public DateTime? DataAtualizacao { get; set; }
        public string ObservacaoAdmin { get; set; }
        public TipoContatoEnum TipoContato { get; set; }

        public Guid IdUsuario { get; set; }
        public int IdStatus { get; set; }
        public Guid? IdUsuarioAcao { get; set; }

        public Usuario Usuario { get; set; }
        public Status Status { get; set; }
        public Usuario UsuarioAcao { get; set; }

        public virtual ICollection<SuporteLog> SuporteLog { get; set; }
    }
}
