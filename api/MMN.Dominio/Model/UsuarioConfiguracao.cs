using System;

namespace MMN.Dominio.Model
{
    public class UsuarioConfiguracao
    {
        public long IdUsuarioConfiguracao { get; set; }
        public Guid IdUsuario { get; set; }
        public decimal? TaxaSaque { get; set; }
        public virtual Usuario Usuario { get; set; }
    }
}