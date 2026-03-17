using MMN.Util.Enum;
using System;
using System.Collections.Generic;

namespace MMN.Dominio.ViewModel
{
    public class FaturaViewModel
    {
        public long IdFatura { get; set; }

        public DateTime DataInicio { get; set; }
        public DateTime DataFechamento { get; set; }
        public DateTime? DataPagamento { get; set; }
        public DateTime? DataVencimento { get; set; }
        public decimal Valor { get; set; }
        public string UrlBoleto { get; set; }
        public string CodigoBoleto { get; set; }
        public Guid IdUsuario { get; set; }
        public UsuarioViewModel UsuarioViewModel { get; set; }
        public List<LancamentosDetalhes> Lancamentos { get; set; }
        public bool Expired { get; set; }
    }

    public class LancamentosDetalhes
    {
        public string Descricao { get; set; }
        public decimal Valor { get; set; }
        public DateTime DataPedido { get; set; }
    }
}
