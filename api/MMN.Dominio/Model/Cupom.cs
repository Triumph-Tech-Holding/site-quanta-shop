using System;
using System.Collections.Generic;

namespace MMN.Dominio.Model
{
    public class Cupom
    {
        public int IdCupom { get; set; }
        public string Codigo { get; set; }
        public string Tipo { get; set; }
        public decimal Valor { get; set; }
        public DateTime? ValidoDe { get; set; }
        public DateTime? ValidoAte { get; set; }
        public decimal? MinimoPedido { get; set; }
        public int? MaxUsosTotal { get; set; }
        public int? MaxUsosPorCliente { get; set; }
        public bool Ativo { get; set; }
        public DateTime DataCadastro { get; set; }
        public string Descricao { get; set; }

        public virtual ICollection<CupomUso> Usos { get; set; }
    }
}
