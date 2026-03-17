using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace MMN.Dominio.ViewModel
{
    public class ResumoSaldoExtratoViewModel
    {
        public string TipoSaldo { get; set; }
        public decimal Ganhos { get; set; }
        public decimal Gastos { get; set; }
        public decimal Saldo { get; set; }
    }
}
