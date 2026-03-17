using AutoMapper;
using MMN.Dominio.Model;
using MMN.Dominio.ViewModel;
using MMN.INegocio.Negocio;
using MMN.IRepositorio.Repositorio;
using MMN.Negocio.Base;


namespace MMN.Negocio.Negocio
{
    public class QuantaAmizadeHistoricoNegocio : BaseNegocio<QuantaAmizadeHistoricoViewModel, QuantaAmizadeHistorico>, IQuantaAmizadeHistoricoNegocio
    {
        private readonly IQuantaAmizadeHistoricoRepositorio _repositorio;
        private readonly IMapper _mapper;

        public QuantaAmizadeHistoricoNegocio(IQuantaAmizadeHistoricoRepositorio repositorio, IMapper mapper) : base(repositorio, mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
        }
    }
}
