using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using MMN.Dominio.Model;
using MMN.Dominio.ViewModel;
using MMN.INegocio.Negocio;
using MMN.IRepositorio.Repositorio;
using MMN.Negocio.Base;
using MMN.Util.Translation;

namespace MMN.Negocio.Negocio
{
    public class MenuNegocio : BaseNegocio<MenuViewModel, Menu>, IMenuNegocio
    {
        private readonly IMenuRepositorio _repositorio;
        private readonly IMapper _mapper;
        private readonly ILocation _location;

        public MenuNegocio(IMenuRepositorio repositorio, IMapper mapper, ILocation location) : base(repositorio, mapper)
        {
            _location = location;
            _repositorio = repositorio;
            _mapper = mapper;
        }
        public IList<MenuViewModel> ObterMenuPorGrupo(int idGrupo)
        {
            var menuModel = _repositorio.ObterMenuPorGrupo(idGrupo).ToList();
            var menu = _mapper.Map<List<MenuViewModel>>(menuModel);
            return menu;
        }
    }
}
