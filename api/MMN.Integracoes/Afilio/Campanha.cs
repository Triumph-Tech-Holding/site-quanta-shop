using System;
using System.Collections.Generic;
using System.Text;

namespace MMN.Integracoes.Afilio
{
    public class Root
    {
        public Campanha Campanha { get; set; }
    }

    public class Campanha
    {
        public string id { get; set; }
        public string nom { get; set; }
        public string url { get; set; }
        public string description { get; set; }
        public string progdate { get; set; }
        public string progdeb { get; set; }
        public string progfin { get; set; }
        public string siteid { get; set; }
        public string cpmprice { get; set; }
        public string clicprice { get; set; }
        public string dblclicprice { get; set; }
        public string leadprice { get; set; }
        public string saleprice { get; set; }
        public string salecurrency { get; set; }
        public string downloadprice { get; set; }
        public string status { get; set; }
        public object url_image { get; set; }
        public string merpays { get; set; }
    }
}
