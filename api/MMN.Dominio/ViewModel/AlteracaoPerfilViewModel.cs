using System;

namespace MMN.Dominio.ViewModel
{
    public class AlteracaoPerfilViewModel
    {
        public int IdAlteracaoPerfil { get; set; }
        public string Observacao { get; set; }
        public bool Aceito { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime? DataAceite { get; set; }
        public Guid IdUsuario { get; set; }
        public Guid? IdUsuarioAcao { get; set; }

        public UsuarioViewModel Usuario { get; set; }
        public UsuarioViewModel UsuarioAcao { get; set; }
    }
}
