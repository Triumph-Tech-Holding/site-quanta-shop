using AutoMapper;
using MMN.Dominio.Model;
using MMN.Dominio.ViewModel;
using MMN.INegocio.Negocio;
using MMN.IRepositorio.Repositorio;
using MMN.Negocio.Base;
using System.Threading.Tasks;

namespace MMN.Negocio.Negocio
{
    public class RefreshTokenNegocio : BaseNegocio<RefreshTokenViewModel, RefreshToken>, IRefreshTokenNegocio
    {
        private readonly IRefreshTokenRepositorio _repositorio;
        private readonly IMapper _mapper;
        public RefreshTokenNegocio(IRefreshTokenRepositorio repositorio, IMapper mapper) : base(repositorio, mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
        }

        public async Task<RefreshTokenViewModel> AddNewToken(RefreshTokenViewModel refresh)
        {
            return _mapper.Map<RefreshTokenViewModel>(await _repositorio.AddNewToken(_mapper.Map<RefreshToken>(refresh)));
        }
    }
}
