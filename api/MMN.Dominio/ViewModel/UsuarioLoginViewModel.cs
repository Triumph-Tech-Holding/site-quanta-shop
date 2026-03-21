using FluentValidation;
using MMN.Util.Enum;
using MMN.Util.Util;
using System.Reflection.Metadata;

namespace MMN.Dominio.ViewModel
{
    public class UsuarioLoginViewModel
    {
        public class Logar
        {
            public string Login { get; set; }
            public string Senha { get; set; }
            public OrigemLoginEnum? Origem { get; set; }
        }

        public class LogarGoogleViewModel
        {
            public string Code { get; set; }
            public OrigemLoginEnum? Origem { get; set; }
            public string RedirectUri { get; set; }
        }

        public class LogarGoogleCredentialViewModel
        {
            public string Credential { get; set; }
            public OrigemLoginEnum? Origem { get; set; }
        }

        public class EsqueciMinhaSenha
        {
            public string Senha { get; set; }
            public string SenhaConfirmada { get; set; }
            public string Token { get; set; }
        }

        public class EsqueciMinhaSenhaValidator : AbstractValidator<EsqueciMinhaSenha>
        {
            public EsqueciMinhaSenhaValidator()
            {
                RuleFor(u => u.Senha).Must(RequisitosSenha).WithMessage("senha_requisitos");
            }

            public bool RequisitosSenha(string senha)
            {
                if (string.IsNullOrEmpty(senha)) return false;

                if (senha.Length < 8 || !UtilBase.HasDigit(senha) || !UtilBase.HasLetter(senha)) return false;

                return true;
            }
        }

        public class TokenEsqueciMinhaSenha
        {
            public string IdUsuario { get; set; }
            public string Jti { get; set; }
            public string DataExpira { get; set; }
        }
    }
}
