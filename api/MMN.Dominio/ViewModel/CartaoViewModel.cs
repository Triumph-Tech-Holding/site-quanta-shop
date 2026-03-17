using MMN.Util.Enum;
using System;

namespace MMN.Dominio.ViewModel
{
    public class CartaoViewModel
    {
        public string Card_number { get; set; }
        public string Card_holder_name { get; set; }
        public int Card_expiration_month { get; set; }
        public string Card_expiration_year { get; set; }
        public string Card_cvv { get; set; }
    }
}
