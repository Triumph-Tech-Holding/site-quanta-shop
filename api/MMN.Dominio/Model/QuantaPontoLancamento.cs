using System;

namespace MMN.Dominio.Model
{
    public class QuantaPontoLancamento
    {
        public long IdQuantaPontoLancamento { get; set; }
        public Guid IdUsuario { get; set; }
        public string Tipo { get; set; }
        public int Pontos { get; set; }
        public string Origem { get; set; }
        public Guid? IdReferencia { get; set; }
        public DateTime DataLancamento { get; set; }
        public bool Ativo { get; set; }
    }
}
