using AutoMapper;
using MMN.Dominio.Model;
using MMN.Dominio.ViewModel;
using MMN.INegocio.Negocio;
using MMN.IRepositorio.Repositorio;
using MMN.Negocio.Base;


namespace MMN.Negocio.Negocio
{
    public class QuantaAmizadeNegocio : BaseNegocio<QuantaAmizadeViewModel, QuantaAmizade>, IQuantaAmizadeNegocio
    {
        private readonly IQuantaAmizadeRepositorio _repositorio;
        private readonly IMapper _mapper;

        public QuantaAmizadeNegocio(IQuantaAmizadeRepositorio repositorio, IMapper mapper) : base(repositorio, mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
        }
    }
}
