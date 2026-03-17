using System;

namespace MMN.Dominio.ViewModel
{
    public class CarrosselViewModel
    {
        public int IdCarrossel { get; set; }
        public string Texto1 { get; set; }
        public string Texto2 { get; set; }
        public string Texto3 { get; set; }
        public string Link { get; set; }
        public string TextoLink { get; set; }
        public string Imagem { get; set; }
        public string CorFundo { get; set; } // "is-light ou green-dark-bg";
        public string Posicao { get; set; }
        public bool Ativo { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? UltimaAtualizacao { get; set; }
        public int OrdemExibicao { get; set; }
        public bool SomenteImagem { get; set; }
    }
}
