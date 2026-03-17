using MMN.Util.Enum;
using System;
using System.Collections.Generic;

namespace MMN.Dominio.Model
{
    public class Mensagem
    {
        public int IdMensagem { get; set; }
        public Guid? IdUsuarioDestino { get; set; }
        public string Titulo { get; set; }
        public string Texto { get; set; }
        public bool Ativo { get; set; }
        public DateTime DataCadastro { get; set; }
        public TipoMensagem TipoMensagem { get; set; }
        public string UrlArquivo { get; set; }
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
        public DateTime? DataLeitura { get; set; }
        public virtual Usuario UsuarioDestino { get; set; }

        public virtual ICollection<MensagemGraduacao> MensagemGraduacao { get; set; }
    }
}