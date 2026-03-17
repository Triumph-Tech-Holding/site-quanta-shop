using AutoMapper;
using MMN.Dominio.Model;
using MMN.Dominio.ViewModel;
using MMN.INegocio.Negocio;
using MMN.IRepositorio.Repositorio;
using MMN.Negocio.Base;


namespace MMN.Negocio.Negocio
{
    public class PromocaoNegocio : BaseNegocio<PromocaoViewModel, Promocao>, IPromocaoNegocio
    {
        private readonly IPromocaoRepositorio _repositorio;
        private readonly IMapper _mapper;

        public PromocaoNegocio(IPromocaoRepositorio repositorio, IMapper mapper) : base(repositorio, mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
        }
    }
}
