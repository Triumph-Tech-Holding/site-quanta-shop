using MMN.Dominio.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MMN.Api.Helpers
{
    public interface ITokenUtil
    {
        string ConstruirToken(UsuarioViewModel userInfo);
        TokenViewModel ValidarToken(string token);
    }
}
