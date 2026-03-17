using System;

namespace MMN.Dominio.ViewModel
{
    public class UsuarioDownLineViewModel
    {
        public Guid IdUsuario { get; set; }
        public int Nivel { get; set; }
        public string Nome { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
        public int IdGraduacao { get; set; }
        public string Celular { get; set; }
        public DateTime DataCadastro { get; set; }

    }
}

