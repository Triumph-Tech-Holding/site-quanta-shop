using AutoMapper;
using MMN.Dominio.Model;
using MMN.Dominio.ViewModel;
using MMN.INegocio.Negocio;
using MMN.IRepositorio.Repositorio;
using MMN.Negocio.Base;

namespace MMN.Negocio.Negocio
{
    public class TransacaoNegocio : BaseNegocio<TransacaoViewModel, Transacao>, ITransacaoNegocio
    {
        private readonly ITransacaoRepositorio _repositorio;
        private readonly IMapper _mapper;
        public TransacaoNegocio(ITransacaoRepositorio repositorio, IMapper mapper) : base(repositorio, mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
        }
    }
}