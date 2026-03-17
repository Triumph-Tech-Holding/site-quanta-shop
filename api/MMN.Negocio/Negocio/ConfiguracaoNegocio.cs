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
    public class ConfiguracaoNegocio : BaseNegocio<ConfiguracaoViewModel, Configuracao>, IConfiguracaoNegocio
    {
        private readonly IConfiguracaoRepositorio _repositorio;
        private readonly IMapper _mapper;
        private readonly ILocation _location;
        private readonly ICache _cache;

        public ConfiguracaoNegocio(
            IConfiguracaoRepositorio repositorio,
            IMapper mapper,
            ILocation location,
            ICache cache
            ) : base(repositorio, mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
            _location = location;
            _cache = cache;
        }

        public ConfiguracaoViewModel BuscarPelaChave(string chave)
        {
            return _mapper.Map<ConfiguracaoViewModel>(_repositorio.GetByKey(chave));
        }

        public ConfiguracaoViewModel BuscarRootSite()
        {
            var configs = GetFromCache();
            return configs.FirstOrDefault(c => c.Chave.Equals("URL_BASE"));
        }

        public List<ConfiguracaoViewModel> GetFromCache()
        {
            var configuracoes = (List<ConfiguracaoViewModel>)_cache.GetItem(CacheKeys.Configurations);
            if (configuracoes == null || configuracoes.Count == 0)
            {
                _cache.SetItem(CacheKeys.Configurations, GetAll());
                configuracoes = (List<ConfiguracaoViewModel>)_cache.GetItem(CacheKeys.Configurations);
            }

            return configuracoes;
        }

        public void EditarConfig(ConfiguracaoViewModel editar)
        {
            var dadosConfig = _repositorio.GetById(editar.IdConfiguracao);


            dadosConfig.Valor = editar.Valor;


            _repositorio.Update(dadosConfig);
            _repositorio.SaveChanges();
        }
    }
}
