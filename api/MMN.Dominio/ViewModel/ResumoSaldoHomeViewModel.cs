using System;
using System.Collections.Generic;
using System.Text;

namespace MMN.Dominio.ViewModel
{
    public class ResumoSaldoHomeViewModel
    {
        public decimal SaldoDisponivel { get; set; }
        public decimal Ganhos { get; set; }
        public decimal Saque { get; set; }
        public decimal Investido { get; set; }
    }
}
