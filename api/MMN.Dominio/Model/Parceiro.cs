using System;

namespace MMN.Dominio.Model
{
    public class Parceiro
    {
        public int IdParceiro { get; set; }

        public Guid IdCredenciado { get; set; }

        public string Descricao { get; set; }

        public string Celular { get; set; }
        public string Nome { get; set; }
        public bool Ativo { get; set; }

        public DateTime DataCriacao { get; set; }
        public DateTime? DataAtualizacao { get; set; }

    }
}
