using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using MMN.Dominio.Model;
using MMN.Dominio.ViewModel;
using MMN.INegocio.Negocio;
using MMN.IRepositorio.Base;
using MMN.IRepositorio.Repositorio;
using MMN.Negocio.Base;

namespace MMN.Negocio.Negocio
{
    public class CidadeNegocio : BaseNegocio<CidadeViewModel, Cidade>, ICidadeNegocio
    {

        private readonly ICidadeRepositorio _repositorio;
        private readonly IMapper _mapper;

        public CidadeNegocio(ICidadeRepositorio repositorio, IMapper mapper) : base(repositorio, mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
        }

        public IList<CidadeViewModel> BuscarCidadeEstado(int idEstado)
        {
            return _mapper.Map<IList<CidadeViewModel>>(_repositorio.BuscarCidadeEstado(idEstado));
        }
    }
}
