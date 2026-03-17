using System;

namespace MMN.Dominio.Model
{
    public class AlteracaoPerfil
    {
        public int IdAlteracaoPerfil { get; set; }
        public string Observacao { get; set; }
        public bool Aceito { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime? DataAceite { get; set; }
        public Guid IdUsuario { get; set; }
        public Guid? IdUsuarioAcao { get; set; }

        public Usuario Usuario { get; set; }
        public Usuario UsuarioAcao { get; set; }
    }
}
