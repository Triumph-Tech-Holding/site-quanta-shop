using System;
using System.Collections.Generic;
using System.Text;

namespace MMN.Dominio.ViewModel
{
    public class LancamentoRedeUsuarioViewModel
    {
        public Guid IdUsuario{ get; set; }
        public string Nome { get; set; }
        public string Login { get; set; }
        public decimal Valor { get; set; }
        public DateTime DataLancamento { get; set; }
        public string Descricao { get; set; }

    }
}
