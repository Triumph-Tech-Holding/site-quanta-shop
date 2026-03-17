using AutoMapper;
using MMN.INegocio.Base;
using MMN.IRepositorio.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace MMN.Negocio.Base
{
    public class BaseNegocio<TViewModel, T> : IBaseNegocio<TViewModel,T>, IDisposable where T : class
    {
        private readonly IBaseRepositorio<T> _baseRepositorio;
        private readonly IMapper _mapper;

        public BaseNegocio(IBaseRepositorio<T> baseRepositorio, IMapper mapper)
        {
            _baseRepositorio = baseRepositorio;
            _mapper = mapper;
        }

        public void SaveChanges()
        {
            _baseRepositorio.SaveChanges();
        }

        public virtual void Delete(int key)
        {
            _baseRepositorio.Delete(key);
            SaveChanges();
        }

        public virtual void DeleteRange(IList<TViewModel> entities)
        {
            var dados = _mapper.Map<IList<T>>(entities);
            _baseRepositorio.DeleteRange(dados);
            SaveChanges();
        }

        public void Dispose()
        {
            _baseRepositorio.Dispose();
        }
        public virtual TViewModel GetById(int key, params string[] entities)
        {
            var retorno = _baseRepositorio.GetById(key, entities);
            return _mapper.Map<TViewModel>(retorno);
        }

        public virtual IList<TViewModel> GetNoTracking(Expression<Func<T, bool>> predicate, params string[] entities)
        {
            var retorno = _baseRepositorio.GetNoTracking(predicate, entities);
            return _mapper.Map<IList<TViewModel>>(retorno);
        }

        public virtual IList<TViewModel> Get(Expression<Func<T, bool>> predicate, params string[] entities)
        {
            var retorno = _baseRepositorio.Get(predicate, entities);
            return _mapper.Map<IList<TViewModel>>(retorno);
        }

        public virtual IQueryable<T> GetIEnumerable(Expression<Func<T, bool>> predicate, params string[] entities)
        {
            return _baseRepositorio.Get(predicate, entities);
        }

        public virtual IList<TViewModel> GetAll(params string[] entities)
        {
            var retorno = _baseRepositorio.GetAll(entities);
            return _mapper.Map<IList<TViewModel>>(retorno);
        }

        public virtual TViewModel First(Expression<Func<T, bool>> predicate, params string[] entities)
        {
            var dados = _mapper.Map<Expression<Func<T, bool>>>(predicate);
            var retorno = _baseRepositorio.First(dados, entities);
            return _mapper.Map<TViewModel>(retorno);
        }
        public virtual bool Exists(Expression<Func<T, bool>> predicate, params string[] entities)
        {
            var dados = _mapper.Map<Expression<Func<T, bool>>>(predicate);
            var retorno = _baseRepositorio.Any(predicate,entities);
            return retorno;
        }
        public virtual TViewModel Last(Expression<Func<T, bool>> predicate, params string[] entities)
        {
            var dados = _mapper.Map<Expression<Func<T, bool>>>(predicate);
            var retorno = _baseRepositorio.Last(dados, entities);
            return _mapper.Map<TViewModel>(retorno);
        }

        public virtual TViewModel FirstNoTracking(Expression<Func<T, bool>> predicate, params string[] entities)
        {
            var dados = _mapper.Map<Expression<Func<T, bool>>>(predicate);
            var retorno = _baseRepositorio.FirstNoTracking(dados, entities);
            return _mapper.Map<TViewModel>(retorno);
        }

        public virtual IList<TViewModel> GetAllNoTracking()
        {
            var retorno = _baseRepositorio.GetAllNoTracking();
            return _mapper.Map<IList<TViewModel>>(retorno);
        }

        public virtual void Insert(TViewModel entity)
        {
            var dados = _mapper.Map<T>(entity);
            _baseRepositorio.Insert(dados);
            SaveChanges();
        }

        public virtual void Update(TViewModel entity)
        {
            var dados = _mapper.Map<T>(entity);
            _baseRepositorio.Update(dados);
            SaveChanges();
        }

        public virtual void UpdateRange(IList<TViewModel> entities)
        {
            var dados = _mapper.Map<IList<T>>(entities);
            _baseRepositorio.UpdateRange(dados);
            SaveChanges();
        }
    }
}
