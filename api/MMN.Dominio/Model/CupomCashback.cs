using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MMN.Dominio.Model
{
    public class CupomCashback
    {
        public string IdCuponCashback { get; set; }
        public string Token { get; set; }
        public decimal Valor { get; set; }
        public decimal PercentualCashback { get; set; }
        public string Documento { get; set; }
        public DateTime DataCompra { get; set; }
        public string Descricao { get; set; }
        public int MeioPagamento { get; set; }
        public Guid? IdComerciante { get; set; }
        public int Status { get; set; }
        public string ChaveDeAcessoNF { get; set; }
        public string UrlChaveDeAcessoNF { get; set; }
        public Usuario Comerciante { get; set; }
        public bool CompraUsuario { get; set; }
        public string ComprovanteCompra { get; set; }
        public bool CompraUsuarioAprovada { get; set; }
        public bool ChaveManual { get; set; }
        public string Justificativa { get; set; }
        public CuponCashbackPedido CuponCashbackPedido { get; set; }
    }
}