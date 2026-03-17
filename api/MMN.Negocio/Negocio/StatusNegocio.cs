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
using MMN.Util.Cache;
using MMN.Util.Translation;

namespace MMN.Negocio.Negocio
{
    public class StatusNegocio : BaseNegocio<StatusViewModel, Status>, IStatusNegocio
    {
        private readonly IStatusRepositorio _repositorio;
        private readonly IMapper _mapper;
        private readonly ILocation _location;
        private readonly ICache _cache;

        public StatusNegocio(IStatusRepositorio repositorio, IMapper mapper, ILocation location, ICache cache) : base(repositorio, mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
            _location = location;
            _cache = cache;
        }

        public List<StatusViewModel> GetFromCache()
        {
            var status = (List<StatusViewModel>)_cache.GetItem(CacheKeys.Status);
            if (status != null && status.Count != 0) return status;

            _cache.SetItem(CacheKeys.Status, GetAll());
            status = (List<StatusViewModel>)_cache.GetItem(CacheKeys.Status);

            return status;
        }
    }
}
