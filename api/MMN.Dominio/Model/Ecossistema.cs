using System;
using System.ComponentModel.DataAnnotations;

namespace MMN.Dominio.Model
{
    public class Ecossistema
    {
        [Key]
        public int IdEcossistema { get; set; }
        public string Nome { get; set; }
        public string Regiao { get; set; }
        public bool Ativo { get; set; }
        public string IdUsuarioGerente { get; set; }
        public DateTime DataCadastro { get; set; }
    }
}
