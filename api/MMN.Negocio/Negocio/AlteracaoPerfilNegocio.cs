using AutoMapper;
using MMN.Dominio.Model;
using MMN.Dominio.ViewModel;
using MMN.INegocio.Negocio;
using MMN.IRepositorio.Repositorio;
using MMN.Negocio.Base;
using MMN.Util.Cache;

namespace MMN.Negocio.Negocio
{
    public class AlteracaoPerfilNegocio : BaseNegocio<AlteracaoPerfilViewModel, AlteracaoPerfil>, IAlteracaoPerfilNegocio
    {
        private readonly IAlteracaoPerfilRepositorio _repositorio;
        private readonly IMapper _mapper;
        private readonly ICache _cache;
        public AlteracaoPerfilNegocio(IAlteracaoPerfilRepositorio repositorio, IMapper mapper, ICache cache) : base(repositorio, mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
            _cache = cache;
        }
    }
}
