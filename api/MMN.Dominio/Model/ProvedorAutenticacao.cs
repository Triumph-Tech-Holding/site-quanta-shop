using System.Collections.Generic;

namespace MMN.Dominio.Model
{
    public class ProvedorAutenticacao
    {
        public int IdProvedorAutenticacao { get; set; }
        public int Protocolo { get; set; }
        public int Provedor { get; set; }
        public string EndpointCadastro { get; set; }
        public string EndpointLogin { get; set; }
        public string Login { get; set; }
        public string ParametrosLogin { get; set; }
        public string Senha { get; set; }
        public string UrlApi { get; set; }

        public virtual ICollection<AutenticacaoExterna> AutenticacaoExterna { get; set; }
    }
}
