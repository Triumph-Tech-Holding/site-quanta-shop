using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using AutoMapper;
using MMN.Dominio.Model;
using MMN.Dominio.ViewModel;
using MMN.INegocio.Base;
using MMN.INegocio.Negocio;
using MMN.IRepositorio.Repositorio;
using MMN.Negocio.Base;
using MMN.Util.Util;

namespace MMN.Negocio.Negocio
{
    public class GrupoNegocio : BaseNegocio<GrupoViewModel, Grupo>, IGrupoNegocio
    {
        private readonly IGrupoRepositorio _repositorio;
        private readonly IMapper _mapper;

        public GrupoNegocio(IGrupoRepositorio repositorio, IMapper mapper) : base(repositorio, mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
        }

        public GrupoViewModel GetByName(string nome)
        {
            return _mapper.Map<GrupoViewModel>(_repositorio.GetByName(nome));
        }
    }
}
