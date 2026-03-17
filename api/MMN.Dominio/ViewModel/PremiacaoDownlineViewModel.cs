using System;

namespace MMN.Dominio.ViewModel
{
    public class PremiacaoDownlineViewModel
    {
        public int IdPremiacaoUpline { get; set; }
        public int IdGraduacao { get; set; }
        public string Premio { get; set; }
        public bool Ativo { get; set; }
        public int Nivel { get; set; }
        public DateTime DataCadastro { get; set; }
        public virtual GraduacaoViewModel Graduacao { get; set; }
    }
}
