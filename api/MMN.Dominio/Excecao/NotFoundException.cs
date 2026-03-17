using System;

namespace MMN.Dominio.Excecao
{
    public class NotFoundException : PadraoException
    {
        public NotFoundException(string errorCode) : base(errorCode)
        {
        }
    }
}
