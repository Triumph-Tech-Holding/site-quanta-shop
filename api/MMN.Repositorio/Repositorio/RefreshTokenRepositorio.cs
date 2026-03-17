using MMN.Dominio.Model;
using MMN.IRepositorio.Repositorio;
using MMN.Repositorio.Base;
using MMN.Repositorio.Contexto;
using System.Threading.Tasks;

namespace MMN.Repositorio.Repositorio
{
    public class RefreshTokenRepositorio : BaseRepositorio<RefreshToken>, IRefreshTokenRepositorio
    {
        public RefreshTokenRepositorio(DatabaseContext ctx) : base(ctx)
        {
        }

        public async Task<RefreshToken> AddNewToken(RefreshToken refreshToken)
        {
            _ctx.RefreshToken.Add(refreshToken);
            await _ctx.SaveChangesAsync();
            return refreshToken;
        }
    }
}
