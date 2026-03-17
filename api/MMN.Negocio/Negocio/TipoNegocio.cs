using AutoMapper;
using MMN.Dominio.Model;
using MMN.Dominio.ViewModel;
using MMN.INegocio.Negocio;
using MMN.IRepositorio.Repositorio;
using MMN.Negocio.Base;
using MMN.Util.Cache;
using System.Collections.Generic;

namespace MMN.Negocio.Negocio
{
    public class TipoNegocio : BaseNegocio<TipoViewModel, Tipo>, ITipoNegocio
    {
        private readonly ITipoRepositorio _repositorio;
        private readonly IMapper _mapper;
        private readonly ICache _cache;

        public TipoNegocio(ITipoRepositorio repositorio, IMapper mapper, ICache cache) : base(repositorio, mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
            _cache = cache;
        }

        public IList<TipoViewModel> ObterTipoPorChave(string chave)
        {
            return _mapper.Map<List<TipoViewModel>>(_repositorio.ObterTipoPorChave(chave));
        }

        public List<TipoViewModel> GetFromCache()
        {
            var tipo = (List<TipoViewModel>)_cache.GetItem(CacheKeys.Tipo);
            if (tipo != null && tipo.Count != 0) return tipo;

            _cache.SetItem(CacheKeys.Tipo, GetAll());
            tipo = (List<TipoViewModel>)_cache.GetItem(CacheKeys.Tipo);

            return tipo;
        }
    }
}
