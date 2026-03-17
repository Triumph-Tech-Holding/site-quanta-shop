using MMN.Dominio.Model;
using MMN.Util.Enum;
using System;
using System.Collections.Generic;

namespace MMN.Dominio.ViewModel
{
    public class PedidoViewModel
    {
        public long IdPedido { get; set; }
        public Guid IdUsuario { get; set; }
        public long? IdTransacao { get; set; }
        public DateTime DataPedido { get; set; }
        public string Codigo { get; set; }
        public decimal ValorTaxa { get; set; }
        public decimal ValorPedido { get; set; }
        public decimal? ValorPago { get; set; }
        public DateTime? DataPagamento { get; set; }
        public bool Pago { get; set; }
        public bool? Ativo { get; set; }
        public bool? Cancelado { get; set; }
        public string EnderecoDeposito { get; set; }
        public int MeioPagamento { get; set; }
        public int NumeroParcelas { get; set; }
        public int Quantidade { get; set; }
        public string UrlPagamento { get; set; }
        public decimal Cotacao { get; set; }
        public string CodigoReferenciaBoleto { get; set; }
        public string UrlBoleto { get; set; }
        public Guid? IdVendaZanox { get; set; }
        public long? IdVendaAfilio { get; set; }
        public int? IdAwinTransaction { get; set; }
        public string LinhaDigitavelBoleto { get; set; }
        public decimal? Cashback { get; set; }
        public decimal? CashbackReceber { get; set; }
        public bool ContabilizarPontuacao { get; set; }
        public decimal? ReaisPorPonto { get; set; }
        public bool GeradoManualmente { get; set; }
        public DateTime? DataReferencia { get; set; }

        public Guid? IdUsuarioComerciante { get; set; }
        public decimal? PercentualCashback { get; set; }
        public UsuarioViewModel Usuario { get; set; }
        public virtual TransacaoViewModel Transacao { get; set; }
        public UsuarioViewModel UsuarioComerciante { get; set; }
        public ICollection<UsuarioProduto> UsuarioProduto { get; set; }
        public ICollection<PedidoDetalheViewModel> PedidoDetalhe { get; set; }
        public ICollection<PagamentoViewModel> Pagamentos { get; set; }
        public int Status { get; set; }
        public int Tipo { get; set; }
    }
}
