using System;

namespace MMN.Dominio.ViewModel
{
    public class LogProdutoViewModel
    {
        public long IdLogProduto { get; set; }
        public DateTime DataAtualizacao { get; set; }
        public string Texto { get; set; }
        public long IdProduto { get; set; }
    }
}
