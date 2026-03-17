using System;

namespace MMN.Dominio.ViewModel
{
    public class RefreshTokenViewModel
    {
        public string Token { get; set; }
        public string JwtId { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool Used { get; set; }
        public bool Invalidated { get; set; }
        public Guid IdUsuario { get; set; }
        public UsuarioViewModel Usuario { get; set; }
    }
}
