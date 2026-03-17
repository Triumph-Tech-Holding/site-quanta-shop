using System;

namespace MMN.Dominio.Model
{
    public class PremiacaoDownline
    {
        public int IdPremiacaoDownline { get; set; }
        public int IdGraduacao { get; set; }
        public string Premio { get; set; }
        public bool Ativo { get; set; }
        public int Nivel { get; set; }
        public DateTime DataCadastro { get; set; }

        public virtual Graduacao Graduacao { get; set; }
    }
}