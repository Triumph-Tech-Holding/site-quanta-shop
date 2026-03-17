using MMN.Util.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace MMN.Dominio.ViewModel
{
    public class FiltroRelatorioAnunciantesViewModel
    {
        public string Nome { get; set; }
        public string Moeda { get; set; }
        public EnumStatusAnunciante? Status { get; set; }
        public EnumTipoCashbackAnunciante? TipoCashback { get; set; }
        public EnumConexaoAnunciante? Conexao { get; set; }
        public int Pagina { get; set; }
        public int QuantidadePorPagina { get; set; }
        public bool? Asc { get; set; }
        public bool? ObterTodos { get; set; }
        public EnumOrdenacaoAnuncitantes? Ordenacao { get; set; }
    }

    public class AnuncianteRelatorioViewModel
    {
        public long Id { get; set; }
        public string IdConexao { get; set; }
        public string Nome { get; set; }
        public string Conexao { get; set; }
        public decimal? CashbackMin { get; set; }
        public decimal? CashbackMax { get; set; }
        public string TipoCashback { get; set; }
        public DateTime UltimaAtualizacao { get; set; }
        public string Moeda { get; set; }
    }
}
