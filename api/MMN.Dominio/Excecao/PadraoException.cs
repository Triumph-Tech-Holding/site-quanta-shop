using MMN.Dominio.ViewModel;
using System;
using System.Resources;

namespace MMN.Dominio.Excecao
{
    public class PadraoException : Exception
    {
        public PadraoException(string errorCode)
        {
            ErrorCode = errorCode;

            if (string.IsNullOrEmpty(ErrorCode))
            {
                throw new ArgumentException("É nescessário informar um codigo de erro válido", "errorCode");
            }

            var mensagemErroResource = new ResourceManager(
                "MMN.Dominio.Excecao.MensagemErro",
                typeof(PadraoException).Assembly
            );

            var mensagem = mensagemErroResource.GetString(ErrorCode);

            if (string.IsNullOrEmpty(mensagem))
            {
                throw new ArgumentException($"É necessário cadastrar um valor para o código de erro informado. (valor = \"{errorCode})\"", "errorCode");
            }
        }

        public string ErrorCode { get; set; }

        public override string Message
        {
            get
            {
                var mensagemErroResource = new ResourceManager(
                    "MMN.Dominio.Excecao.MensagemErro",
                    typeof(PadraoException).Assembly
                );

                return mensagemErroResource.GetString(ErrorCode);
            }
        }

        public MensagemErroViewModel CriarViewModel()
        {
            return new MensagemErroViewModel
            {
                ErrorCode = this.ErrorCode,
                Mensagem = this.Message
            };
        }
    }
}
