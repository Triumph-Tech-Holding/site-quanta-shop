using AutoMapper;
using MMN.Dominio.Model;
using MMN.Dominio.ViewModel;
using MMN.INegocio.Negocio;
using MMN.IRepositorio.Repositorio;
using MMN.Negocio.Base;

namespace MMN.Negocio.Negocio
{
    public class PremiacaoDownlineNegocio : BaseNegocio<PremiacaoDownlineViewModel, PremiacaoDownline>, IPremiacaoDownlineNegocio
    {
        private readonly IPremiacaoDownlineRepositorio _repositorio;
        private readonly Mapper _mapper;
        public PremiacaoDownlineNegocio(IPremiacaoDownlineRepositorio repositorio, Mapper mapper) : base(repositorio, mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
        }
    }
}
