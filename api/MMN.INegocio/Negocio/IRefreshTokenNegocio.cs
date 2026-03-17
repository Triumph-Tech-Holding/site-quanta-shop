using MMN.Dominio.Model;
using MMN.Dominio.ViewModel;
using MMN.INegocio.Base;
using System.Threading.Tasks;

namespace MMN.INegocio.Negocio
{
    public interface IRefreshTokenNegocio : IBaseNegocio<RefreshTokenViewModel, RefreshToken>
    {
        Task<RefreshTokenViewModel> AddNewToken(RefreshTokenViewModel refresh);
    }
}
