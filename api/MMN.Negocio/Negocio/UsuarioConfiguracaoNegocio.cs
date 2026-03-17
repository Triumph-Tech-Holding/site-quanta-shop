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
    public class UsuarioConfiguracaoNegocio : BaseNegocio<UsuarioConfiguracaoViewModel, UsuarioConfiguracao>, IUsuarioConfiguracaoNegocio
    {
        private readonly IUsuarioConfiguracaoRepositorio _repositorio;
        private readonly IMapper _mapper;
        public UsuarioConfiguracaoNegocio(IUsuarioConfiguracaoRepositorio repositorio, IMapper mapper) : base(repositorio, mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
        }
    }
}
