using System;

namespace MMN.Dominio.Model
{
    public class CacheResumoBinario
    {
        public long IdCache { get; set; }
        public Guid IdUsuario { get; set; }
        public int IdProduto { get; set; }
        public short Esquerda { get; set; }
        public short Direita { get; set; }
        public string DataLimite { get; set; }

        public virtual Produto Produto { get; set; }
        public virtual Usuario Usuario { get; set; }
    }
}
