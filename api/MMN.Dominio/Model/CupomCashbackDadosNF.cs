using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MMN.Dominio.Model
{
    public class CupomCashbackDadosNF
    {
        [Key]
        public long IdCupomCashbackDadosNF { get; set; }
        public string ChaveDeAcesso { get; set; }
        public string CNPJ { get; set; }
        public string CPF { get; set; }
        public double QtdTotalDeItens { get; set; }
        public double ValorAPagar { get; set; }
        public double ValorTotal { get; set; }
        public double Descontos { get; set; }
        public string IdCuponCashback { get; set; }
        public DateTime DataCadastro { get; set; }
        
        [ForeignKey("IdCuponCashback")]
        public virtual CupomCashback CupomCashback { get; set; }
        public virtual List<CupomCashbackItemNF> Itens { get; set; }
    }
}
