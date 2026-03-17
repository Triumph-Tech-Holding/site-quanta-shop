using System;
using System.Collections.Generic;
using System.Linq;
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
    public class ProdutoNivelNegocio : BaseNegocio<ProdutoNivelViewModel, ProdutoNivel>, IProdutoNivelNegocio
    {
        private readonly IProdutoNivelRepositorio _repositorio;
        private readonly IMapper _mapper;

        public ProdutoNivelNegocio(IProdutoNivelRepositorio repositorio, IMapper mapper) : base(repositorio, mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
        }
    }
}
