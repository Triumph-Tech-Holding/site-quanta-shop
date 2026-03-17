using MMN.Dominio.Model;
using MMN.Dominio.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMN.INegocio.Negocio
{
    public interface IPreCadastroNegocio
    {
        Usuario RegistrarFacilitado(UsuarioCadastroFacilitadoViewModel model);
    }
}
