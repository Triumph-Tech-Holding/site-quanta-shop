using System;
using System.Linq;
using System.Linq.Expressions;
using MMN.IRepositorio.Base;
using MMN.Repositorio.Contexto;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;
using Dapper;
using Microsoft.Data.SqlClient;

namespace MMN.Repositorio.Base
{
    public class BaseRepositorio<T> : IBaseRepositorio<T>, IDisposable where T : class
    {
        protected readonly DatabaseContext _ctx;
        private readonly object _transactionLock = new object();
        public BaseRepositorio(DatabaseContext ctx)
        {
            _ctx = ctx;
        }

        public IDbContextTransaction GetTransaction()
        {
            lock (_transactionLock)
            {
                var transaction = _ctx.Database.CurrentTransaction;

                if (transaction == null)
                {
                    return _ctx.Database.BeginTransaction();
                }
                else
                {
                    return transaction;
                }
            }
        }

        public void SaveChanges()
        {
            _ctx.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await _ctx.SaveChangesAsync();
        }

        public void Delete(int key)
        {
            var entity = _ctx.Set<T>().Find(key);
            _ctx.Remove(entity);
        }

        public bool Any(Expression<Func<T, bool>> predicate, params string[] entities)
        {
            var exists = _ctx.Set<T>().Any(predicate);
            return exists;
        }

        public void DeleteRange(IList<T> entities)
        {
            _ctx.RemoveRange(entities);
        }

        public void Dispose()
        {
            if (_ctx != null)
                _ctx.Dispose();

            GC.SuppressFinalize(this);
        }

        public T First(Expression<Func<T, bool>> predicate, params string[] entities)
        {
            var query = _ctx.Set<T>().Where(predicate);
            foreach (var include in entities)
                query = query.Include(include);

            return query.FirstOrDefault();
        }

        public T Last(Expression<Func<T, bool>> predicate, params string[] entities)
        {
            var query = _ctx.Set<T>().Where(predicate);
            foreach (var include in entities)
                query = query.Include(include);

            return query.ToArray().LastOrDefault();
        }

        public T FirstNoTracking(Expression<Func<T, bool>> predicate, params string[] entities)
        {
            var query = _ctx.Set<T>().Where(predicate).AsNoTracking();
            foreach (var include in entities)
                query = query.Include(include);

            return query.FirstOrDefault();
        }

        public T GetById(long key, params string[] entities)
        {
            var query = _ctx.Set<T>().Find(key);
            foreach (string include in entities)
                _ctx.Entry(query).Reference(include).Load();

            return _ctx.Set<T>().Find(key);
        }

        public T GetById(int key, params string[] entities)
        {
            var query = _ctx.Set<T>().Find(key);
            foreach (string include in entities)
                _ctx.Entry(query).Reference(include).Load();

            return _ctx.Set<T>().Find(key);
        }

        public IQueryable<T> GetNoTracking(Expression<Func<T, bool>> predicate, params string[] entities)
        {
            var query = _ctx.Set<T>().Where(predicate);
            foreach (var include in entities)
                query = query.Include(include);

            return query.AsNoTracking();
        }

        public IQueryable<T> Get(Expression<Func<T, bool>> predicate, params string[] entities)
        {
            var query = _ctx.Set<T>().Where(predicate);
            foreach (var include in entities)
                query = query.Include(include);
            return query;
        }

        public IQueryable<T> GetAll(params string[] entities)
        {
            var query = _ctx.Set<T>().AsQueryable();
            foreach (var include in entities)
                query = query.Include(include);

            return query;
        }

        public T First(Expression<Func<T, bool>> predicate)
        {
            return _ctx.Set<T>().Where(predicate).FirstOrDefault();
        }

        public IQueryable<T> Get(Expression<Func<T, bool>> predicate)
        {
            return _ctx.Set<T>().Where(predicate);
        }

        public IQueryable<T> GetAllNoTracking()
        {
            return _ctx.Set<T>().AsNoTracking();
        }

        public void Insert(T entity)
        {
            _ctx.Set<T>().Add(entity);
        }

        public void Update(T entity)
        {
            _ctx.Entry(entity).State = EntityState.Modified;
        }

        public void UpdateRange(IList<T> entities)
        {
            _ctx.UpdateRange(entities);
        }
    }
}
