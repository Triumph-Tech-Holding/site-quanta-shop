using System;

namespace MMN.Dominio.ViewModel
{
    public class EcossistemaViewModel
    {
        public int IdEcossistema { get; set; }
        public string Nome { get; set; }
        public string Regiao { get; set; }
        public bool? Ativo { get; set; }
        public string IdUsuarioGerente { get; set; }
        public DateTime DataCadastro { get; set; }
    }
}
