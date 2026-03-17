using System;

namespace MMN.Dominio.ViewModel
{
    public class SuporteLogViewModel
    {
        public int IdSuporteLog { get; set; }
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
        public Guid IdUsuario { get; set; }
        public int IdStatus { get; set; }
        public Guid? IdUsuarioAcao { get; set; }
        public SuporteViewModel Suporte { get; set; }

        public DateTime DataUpdate { get; set; }
    }
}
