using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MMN.Dominio.Model
{
    public class CupomCashbackItemNF
    {
        [Key]
        public long IdCupomCashbackItemNF { get; set; }
        public string NomeProduto { get; set; }
        public string CodigoProduto { get; set; }
        public double QuantidadeProduto { get; set; }
        public double ValorUnitarioProduto { get; set; }
        public double SubTotal => (ValorUnitarioProduto * QuantidadeProduto);
        public long IdCupomCashbackDadosNF { get; set; }
        
        [ForeignKey("IdCupomCashbackDadosNF")]
        public virtual CupomCashbackDadosNF CashbackDadosNF { get; set; }
    }
}