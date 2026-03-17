using AutoMapper;
using MMN.Dominio.Model;
using MMN.Dominio.ViewModel;
using MMN.INegocio.Negocio;
using MMN.IRepositorio.Repositorio;
using MMN.Negocio.Base;

namespace MMN.Negocio.Negocio
{
    public class MensagemGraduacaoNegocio : BaseNegocio<MensagemGraduacaoViewModel, MensagemGraduacao>, IMensagemGraduacaoNegocio
    {
        private readonly IMensagemGraduacaoRepositorio _repositorio;
        private readonly IMapper _mapper;

        public MensagemGraduacaoNegocio(IMensagemGraduacaoRepositorio repositorio, IMapper mapper) : base(repositorio, mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
        }
    }
}
