using System;
using System.Collections.Generic;
using System.Text;

namespace MMN.Dominio.ViewModel
{
    public class RelatorioCadastroAtivacaoViewModel
    {
        public int Id { get; set; }
        public object Data { get; set; }
        public int Ativados { get; set; }
        public int Cadastrados { get; set; }
    }
}
