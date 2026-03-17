using AutoMapper;
using MMN.Dominio.Model;
using MMN.Dominio.ViewModel;
using MMN.INegocio.Negocio;
using MMN.IRepositorio.Repositorio;
using MMN.Negocio.Base;

namespace MMN.Negocio.Negocio
{
    public class AnuncianteCashBackNegocio : BaseNegocio<AnuncianteCashBackViewModel, AnuncianteCashBack>, IAnuncianteCashBackNegocio
    {
        private readonly IAnuncianteCashBackRepositorio _repositorio;
        private readonly IMapper _mapper;

        public AnuncianteCashBackNegocio(IAnuncianteCashBackRepositorio repositorio, IMapper mapper) : base(repositorio, mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
        }
    }
}
