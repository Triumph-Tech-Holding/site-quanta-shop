using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MMN.IRepositorio.Base
{
    public interface IBaseRepositorio<T> where T : class
    {
        public IDbContextTransaction GetTransaction();
        IQueryable<T> GetAllNoTracking();
        IQueryable<T> GetAll(params string[] entities);
        IQueryable<T> Get(Expression<Func<T, bool>> predicate, params string[] entities);
        IQueryable<T> GetNoTracking(Expression<Func<T, bool>> predicate, params string[] entities);
        T GetById(int key, params string[] entities);
        T GetById(long key, params string[] entities);
        T First(Expression<Func<T, bool>> predicate, params string[] entities);
        T Last(Expression<Func<T, bool>> predicate, params string[] entities);
        T FirstNoTracking(Expression<Func<T, bool>> predicate, params string[] entities);
        void Insert(T entity);
        void Update(T entity);
        void UpdateRange(IList<T> entities);
        void Delete(int key);
        void DeleteRange(IList<T> entities);
        void SaveChanges();
        Task SaveChangesAsync();
        void Dispose();
        bool Any(Expression<Func<T, bool>> predicate, params string[] entities);
    }
}
