using System;

namespace MMN.Dominio.Model
{
    public class AuditoriaLgpd
    {
        public long IdAuditoriaLgpd { get; set; }
        public Guid IdUsuarioMaster { get; set; }
        public Guid IdUsuarioAlvo { get; set; }
        public string Campo { get; set; }
        public string Motivo { get; set; }
        public string IpOrigem { get; set; }
        public string UserAgent { get; set; }
        public DateTime DataAcesso { get; set; }
    }
}
