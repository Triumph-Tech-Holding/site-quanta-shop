using AutoMapper;
using MMN.Dominio.Model;
using MMN.Dominio.ViewModel;
using MMN.INegocio.Negocio;
using MMN.IRepositorio.Repositorio;
using MMN.Negocio.Base;


namespace MMN.Negocio.Negocio
{
    public class BancoNegocio : BaseNegocio<BancoViewModel, Banco>, IBancoNegocio
    {
        private readonly IBancoRepositorio _repositorio;
        private readonly IMapper _mapper;

        public BancoNegocio(IBancoRepositorio repositorio, IMapper mapper) : base(repositorio, mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
        }
    }
}
