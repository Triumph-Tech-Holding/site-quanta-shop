using System;

namespace MMN.Dominio.Model
{
    public class RefreshToken
    {
        public string Token { get; set; }
        public string JwtId { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool Used { get; set; }
        public bool Invalidated { get; set; }
        public Guid IdUsuario { get; set; }
        public virtual Usuario Usuario { get; set; }
    }
}
