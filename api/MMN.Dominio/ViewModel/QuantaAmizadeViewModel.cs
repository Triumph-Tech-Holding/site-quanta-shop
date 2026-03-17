using System;

namespace MMN.Dominio.ViewModel
{
    public class QuantaAmizadeViewModel
    {
        public int IdQuantaAmizade { get; set; }
        public Guid IdUsuarioPai { get; set; }
        public Guid IdUsuarioFilho { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime DataFim { get; set; }
        public bool ObjetivoAtingido { get; set; }
    }
}
