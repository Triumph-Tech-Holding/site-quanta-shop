using AutoMapper;
using MMN.Dominio.Model;
using MMN.Dominio.ViewModel;
using MMN.INegocio.Negocio;
using MMN.IRepositorio.Repositorio;
using MMN.Negocio.Base;
using MMN.Util.Cache;
using MMN.Util.Translation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MMN.Negocio.Negocio
{
    public class GraduacaoNegocio : BaseNegocio<GraduacaoViewModel, Graduacao>, IGraduacaoNegocio
    {
        private readonly IGraduacaoRepositorio _repositorio;
        private readonly IProceduresRepositorio _repositorioProcedure;
        private readonly IMapper _mapper;
        private readonly ILocation _location;
        private readonly ICache _cache;

        public GraduacaoNegocio(
              IGraduacaoRepositorio repositorio,
              IMapper mapper,
              ILocation location,
              IProceduresRepositorio repositorioProcedure,
              ICache cache) : base(repositorio, mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
            _location = location;
            _repositorioProcedure = repositorioProcedure;
            _cache = cache;
        }

        /// <summary>
        /// Retorna a fraduação inicial (de menor nível)
        /// </summary>
        /// <returns></returns>
        public GraduacaoViewModel ObterMenorNivel()
        {
            return _mapper.Map<GraduacaoViewModel>(_repositorio.ObterMenorNivel());
        }

        public GraduacaoViewModel ObterPorNivel(int nivel)
        {
            return _mapper.Map<GraduacaoViewModel>(_repositorio.ObterPorNivel(nivel));
        }

        public List<GraduacaoViewModel> GetFromCache()
        {
            var graduacao = (List<GraduacaoViewModel>)_cache.GetItem(CacheKeys.Graduacao);
            if (graduacao != null && graduacao.Count != 0)
            {
                return graduacao;
            }

            graduacao = GetAll().ToList();
            _cache.SetItem(CacheKeys.Graduacao, graduacao);

            return graduacao;
        }
    }
}
