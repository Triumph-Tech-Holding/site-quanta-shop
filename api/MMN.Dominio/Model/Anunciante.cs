using System;
using System.Collections.Generic;

namespace MMN.Dominio.Model
{
    public class Anunciante
    {
        public int IdAnunciante { get; set; }
        public string Nome { get; set; }
        public string ImagemUrl { get; set; }
        public decimal Cashback { get; set; }
        public bool Ativo { get; set; }
        public string IdProgramZanox { get; set; }
        public string IdAfilio { get; set; }
        public string IdAwin { get; set; }
        public bool EditadoUsuario { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime DataAtualizacao { get; set; }
        public string AccountId { get; set; }
        public bool Ancora { get; set; }

        public virtual OrdenacaoAnuncio OrdenacaoAnuncio { get; set; }
        public virtual ICollection<AnuncianteCashBack> AnuncianteCashBack { get; set; }
        public virtual ICollection<CategoriaAnunciante> CategoriaAnunciante { get; set; }
        public virtual ICollection<Transacao> Transacao { get; set; }
    }
}
