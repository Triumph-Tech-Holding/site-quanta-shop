using System;
using System.Collections.Generic;
using System.Text;

namespace MMN.Dominio.ViewModel
{
    public class FiltroParceiroViewModel
    {
        public string Nome { get; set; }
        public string Celular { get; set; }
        public string Descricao { get; set; }
        public bool? Ativo { get; set; }
        public DateTime? DataCriacaoInicio { get; set; }
        public DateTime? DataCriacaoFim { get; set; }
        public DateTime? DataAtualizacaoInicio { get; set; }
        public DateTime? DataAtualizacaoFim { get; set; }
    }
}
