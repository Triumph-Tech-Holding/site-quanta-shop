using System;

namespace MMN.Dominio.Model
{
    public class Tutorial
    {
        public int IdTutorial { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string URL { get; set; }
        public bool Ativo { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime DataAtualizacao { get; set; }
    }
}
