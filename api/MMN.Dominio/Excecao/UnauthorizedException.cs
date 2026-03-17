using System;

namespace MMN.Dominio.Excecao
{
    public class UnauthorizedException : PadraoException
    {
        public UnauthorizedException(string errorCode) : base(errorCode)
        {
        }
    }
}
