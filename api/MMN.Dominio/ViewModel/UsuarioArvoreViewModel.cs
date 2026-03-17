using System;

namespace MMN.Dominio.ViewModel
{
    public class UsuarioArvoreViewModel
    {
        public Guid IdUsuario { get; set; }
        public int Posicao { get; set; }
        public int LadoPai { get; set; }
        public int Nivel { get; set; }
    }
}