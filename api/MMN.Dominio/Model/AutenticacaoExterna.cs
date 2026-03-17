using System;

namespace MMN.Dominio.Model
{
    public class AutenticacaoExterna
    {
        public int IdAutenticacaoExterna { get; set; }
        public string IdExterno { get; set; }
        public bool Ativo { get; set; }
        public int IdProvedorAutenticacao { get; set; }
        public Guid IdUsuario { get; set; }

        public virtual Usuario Usuario { get; set; }
        public virtual ProvedorAutenticacao ProvedorAutenticacao { get; set; }
    }
}