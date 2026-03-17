using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using MMN.Dominio.Model;
using MMN.Dominio.ViewModel;
using MMN.INegocio.Negocio;
using MMN.IRepositorio.Repositorio;
using MMN.Negocio.Base;

namespace MMN.Negocio.Negocio
{
    public class EstadoNegocio : BaseNegocio<EstadoViewModel, Estado>, IEstadoNegocio
    {
        private readonly IEstadoRepositorio _repositorio;
        private readonly IMapper _mapper;

        public EstadoNegocio(IEstadoRepositorio repositorio, IMapper mapper) : base(repositorio, mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
        }

        public IList<EstadoViewModel> BuscarEstado()
        {
            var lst = _repositorio.GetAll();

            return _mapper.Map<IList<EstadoViewModel>>(lst);
        }
    }
}
