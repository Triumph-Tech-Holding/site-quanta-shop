using System;

namespace MMN.Dominio.Model
{
    public class LogProduto
    {
        public long IdLogProduto { get; set; }
        public DateTime DataAtualizacao { get; set; }
        public string Texto { get; set; }
        public int IdProduto { get; set; }
        public Guid IdUsuario { get; set; }

        public Usuario Usuario { get; set; }
        public Produto Produto { get; set; }
    }
}
