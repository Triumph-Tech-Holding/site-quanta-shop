using MMN.Dominio.ViewModel;
using System;
using System.Collections.Generic;

namespace MMN.Dominio.Model
{
    public class LancarAssinatura
    {
       public string Observacao {  get; set; }
        public decimal valor { get; set; }
        public string email { get; set; }
        public string documento { get; set; }
        public string nome { get; set; }
        public string ddd { get; set; }
        public string telefone { get; set; }
        public string logradouro { get; set; }
        public string numero { get; set; }
        public string complemento { get; set; }
        public string bairro { get; set; }
        public string cidade { get; set; }
        public string uf { get; set; }
        public string cep { get; set; }
        public string cardHash { get; set; }
        public string cardExpirationDat { get; set; }
        public CartaoViewModel Card { get; set; }
    }
}
