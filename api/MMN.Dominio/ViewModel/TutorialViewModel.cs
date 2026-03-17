using System;
using System.Collections.Generic;
using System.Text;

namespace MMN.Dominio.ViewModel
{
    public class TutorialViewModel
    {
        public int IdTutorial { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string URL { get; set; }
        public bool Ativo { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime DataAtualizacao { get; set; }

    }
}
