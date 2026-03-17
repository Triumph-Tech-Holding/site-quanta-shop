using System;

namespace MMN.Dominio.Model
{
    public class UsuarioCarteira
    {
        public long IdUsuarioCarteira { get; set; }
        public string Endereco { get; set; }
        public string Descricao { get; set; }
        public DateTime DataCadastro { get; set; }
        public bool Ativo { get; set; }
        public bool Aprovado { get; set; }
        public DateTime? DataAprovacao { get; set; }
        public Guid IdUsuario { get; set; }
        
        public virtual Usuario Usuario { get; set; }
    }
}
