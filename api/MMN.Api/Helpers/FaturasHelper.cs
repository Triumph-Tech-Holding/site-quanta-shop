using MMN.Dominio.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MMN.Api.Helpers
{
    public class GerarFaturasRequest
    {
        public string Credenciamento { get; set; }
    }
    public class NovoBoletoRequest
    {
        public string codigoBoleto { get; set; }
    }
}
