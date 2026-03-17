using System.Collections.Generic;
using AutoMapper;
using MMN.Dominio.Model;
using MMN.Dominio.ViewModel;
using MMN.INegocio.Negocio;
using MMN.IRepositorio.Repositorio;
using MMN.Negocio.Base;
using MMN.Util.Translation;

namespace MMN.Negocio.Negocio
{
    public class CategoriaAnuncianteNegocio : BaseNegocio<CategoriaAnuncianteViewModel, CategoriaAnunciante>, ICategoriaAnuncianteNegocio
    {
        private readonly ICategoriaAnuncianteRepositorio _repositorio;
        private readonly IMapper _mapper;
        private readonly ILocation _location;

        public CategoriaAnuncianteNegocio(ICategoriaAnuncianteRepositorio repositorio, IMapper mapper, ILocation location) : base(repositorio, mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
            _location = location;
        }
    }
}
