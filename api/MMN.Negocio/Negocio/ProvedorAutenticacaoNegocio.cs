using MMN.Dominio.Model;
using MMN.INegocio.Negocio;
using MMN.IRepositorio.Repositorio;
using MMN.Negocio.Base;

namespace MMN.Negocio.Negocio
{
    public class ProvedorAutenticacaoNegocio : BaseNegocioNovo<ProvedorAutenticacao>, IProvedorAutenticacaoNegocio
    {
        public ProvedorAutenticacaoNegocio(IProvedorAutenticacaoRepositorio baseRepositorio)
            : base(baseRepositorio)
        {
        }
    }
}
