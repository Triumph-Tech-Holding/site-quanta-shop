using System;
using System.Collections.Generic;
using System.Text;

namespace MMN.Dominio.ViewModel
{
    public class LimitesGanhosViewModel
    {
        public decimal GanhoMaximoCashback { get; set; }
        public decimal GanhoAtualCashback { get; set; }
        public decimal PercentualAtualCashback { get; set; }
        public decimal GanhoMaximoRede { get; set; }
        public decimal GanhoAtualRede { get; set; }
        public decimal PercentualAtualRede { get; set; }
    }
}
