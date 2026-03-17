using System;

namespace MMN.Dominio.Model
{
    public class AuditoriaPremiacao
    {
        public long IdAuditoriaPremiacao { get; set; }
        public Guid IdUsuarioDono { get; set; }
        public Guid IdUsuario { get; set; }
        public string Login { get; set; }
        public int Pontuacao { get; set; }
        public DateTime DataCadastro { get; set; }
        public int IdGraduacao { get; set; }
        public Usuario Usuario { get; set; }
        public Graduacao Graduacao { get; set; }
    }
}
