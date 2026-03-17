using System;
using System.Collections.Generic;
using System.Text;

namespace MMN.Dominio.ViewModel
{
    public class FiltroCategoriaViewModel
    {
        public DateTime? DataCriacaoInicio { get; set; }
        public DateTime? DataCriacaoFim { get; set; }
        public DateTime? DataAtualizacaoInicio { get; set; }
        public DateTime? DataAtualizacaoFim { get; set; }
        public string Nome { get; set; }
        public string NomePai { get; set; }
        public bool Ativo { get; set; }
    }

    public class FiltroMapeamentoViewModel
    {
        public string NomeAnunciante { get; set; }
        public int? IdCategoria { get; set; }
    }
}
