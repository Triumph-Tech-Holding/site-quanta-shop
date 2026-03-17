using MMN.Dominio.Model;
using MMN.IRepositorio.Base;
using System.Threading.Tasks;

namespace MMN.IRepositorio.Repositorio
{
    public interface IRefreshTokenRepositorio : IBaseRepositorio<RefreshToken>
    {
        Task<RefreshToken> AddNewToken(RefreshToken refreshToken);
    }
}
