using System;
using System.Collections.Generic;
using System.Text;

namespace MMN.Dominio.ViewModel
{
    public class FiltroSuporteViewModel
    {
        public int? IdStatus { get; set; }
        public int? IdTipo { get; set; }
        public string codigoPedido { get; set; }
        public DateTime? DataInicioInicio { get; set; }
        public DateTime? DataInicioFim { get; set; }
        public DateTime? DataAtualizacaoInicio { get; set; }
        public DateTime? DataAtualizacaoFim { get; set; }
        public int Page { get; set; }
        public int PerPage { get; set; }

        public string LoginPatrocinador { get; set; }
        public string LoginUsuario { get; set; }
    }
}
