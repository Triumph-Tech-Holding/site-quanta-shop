using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MMN.INegocio.Base;
using MMN.IRepositorio.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MMN.Negocio.Base
{
    public class BaseNegocioNovo<T> : IBaseNegocioNovo<T>, IDisposable where T : class
    {
        private readonly IBaseRepositorio<T> _baseRepositorio;

        public BaseNegocioNovo(IBaseRepositorio<T> baseRepositorio)
        {
            _baseRepositorio = baseRepositorio;
        }

        public void Dispose()
        {
            _baseRepositorio.Dispose();
        }

        public virtual async Task<IList<T>> GetNoTrackingAsync(Expression<Func<T, bool>> predicate, params string[] entities)
        {
            return await _baseRepositorio
                .GetNoTracking(predicate, entities)
                .ToListAsync();
        }

        public virtual async Task<IList<T>> GetAsync(Expression<Func<T, bool>> predicate, params string[] entities)
        {
            return await _baseRepositorio
                .Get(predicate, entities)
                .ToListAsync();
        }

        public virtual async Task<T> FirstAsync(Expression<Func<T, bool>> predicate, params string[] entities)
        {
            return await Task.Run(() => _baseRepositorio.First(predicate, entities));
        }

        public virtual async Task<T> FirstNoTrackingAsync(Expression<Func<T, bool>> predicate, params string[] entities)
        {
            return await Task.Run(() => _baseRepositorio.FirstNoTracking(predicate, entities));
        }
    }
}
