using System;

namespace MMN.Dominio.ViewModel
{
    public class UsuarioPremiacaoViewModel
    {
        public long IdUsuarioPremiacao { get; set; }
        public string Premio { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime? DataAtualizacao { get; set; }
        public bool PremioEntregue { get; set; }

        public int IdGraduacao { get; set; }
        public Guid IdUsuario { get; set; }
        public Guid? IdUsuarioAcao { get; set; }

        public UsuarioViewModel Usuario { get; set; }
        public GraduacaoViewModel Graduacao { get; set; }
    }
}
