using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MMN.INegocio.Base
{
    public interface IBaseNegocioNovo<T>
    {
        Task<IList<T>> GetAsync(Expression<Func<T, bool>> predicate, params string[] entities);
        Task<IList<T>> GetNoTrackingAsync(Expression<Func<T, bool>> predicate, params string[] entities);
        Task<T> FirstAsync(Expression<Func<T, bool>> predicate, params string[] entities);
        Task<T> FirstNoTrackingAsync(Expression<Func<T, bool>> predicate, params string[] entities);
        void Dispose();
    }
}
