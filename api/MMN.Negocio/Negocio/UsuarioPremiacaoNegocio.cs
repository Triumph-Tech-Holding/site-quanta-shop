using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using MMN.Dominio.Model;
using MMN.Dominio.ViewModel;
using MMN.INegocio.Negocio;
using MMN.IRepositorio.Base;
using MMN.IRepositorio.Repositorio;
using MMN.Negocio.Base;
using MMN.Util.Extensions;

namespace MMN.Negocio.Negocio
{
    public class UsuarioPremiacaoNegocio : BaseNegocio<UsuarioPremiacaoViewModel, UsuarioPremiacao>, IUsuarioPremiacaoNegocio
    {
        private readonly IUsuarioPremiacaoRepositorio _repositorio;
        private readonly IMapper _mapper;
        private readonly IGraduacaoNegocio _graduacaoNegocio;

        public UsuarioPremiacaoNegocio(IUsuarioPremiacaoRepositorio repositorio, IMapper mapper, IGraduacaoNegocio graduacaoNegocio) : base(repositorio, mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
            _graduacaoNegocio = graduacaoNegocio;
        }
    }
}
