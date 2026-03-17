using AutoMapper;
using MMN.Dominio.Excecao;
using MMN.Dominio.Model;
using MMN.Dominio.ViewModel;
using MMN.INegocio.Negocio;
using MMN.Integracoes.Afilio;
using MMN.IRepositorio.Repositorio;
using MMN.Negocio.Base;
using MMN.Util.Cache;
using MMN.Util.Extensions;
using MMN.Util.Translation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace MMN.Negocio.Negocio
{
    public class CategoriaNegocio : BaseNegocio<CategoriaViewModel, Categoria>, ICategoriaNegocio
    {
        private readonly ICategoriaRepositorio _repositorio;
        private readonly ICategoriaAnuncianteRepositorio _repositorioCategoriaAnunciante;
        private readonly ICategoriaAnuncianteNegocio _mapNegocio;
        private readonly IMapper _mapper;
        private readonly ILocation _location;
        private readonly ICache _cache;

        public CategoriaNegocio(ICategoriaRepositorio repositorio, ICategoriaAnuncianteRepositorio repositorioCategoriaAnunciante, ICache cache, ICategoriaAnuncianteNegocio mapNegocio, IMapper mapper, ILocation location) : base(repositorio, mapper)
        {
            _repositorio = repositorio;
            _repositorioCategoriaAnunciante = repositorioCategoriaAnunciante;
            _mapNegocio = mapNegocio;
            _mapper = mapper;
            _location = location;
            _cache = cache;
        }

        public Categoria AtualizarCategoria(CriarCategoriaViewModel categoria)
        {
            var categorias = _repositorio.GetAll();
            var categoriaBanco = _repositorio.First(c => c.IdCategoria == categoria.IdCategoria);

            if (categoriaBanco.Nome != categoria.Nome && categorias.Any(c => c.Nome == categoria.Nome && c.IdCategoriaPai == categoria.IdCategoriaPai))
            {
                throw new PadraoException("categoria_em_uso");
            }

            if (categoria.IdCategoriaPai == categoria.IdCategoria)
            {
                throw new PadraoException("sub_categoria_nao_permitida");
            }

            var categoriaPai = _repositorio.First(c => c.IdCategoria == categoria.IdCategoriaPai);
            if (!categoriaPai.Ativo)
            {
                throw new NotFoundException("categoria_pai_inativa");
            }

            categoriaBanco.Nome = categoria.Nome;
            categoriaBanco.IdCategoriaPai = categoria.IdCategoriaPai;
            categoriaBanco.Ativo = categoria.Ativo;

            _repositorio.Update(categoriaBanco);
            _repositorio.SaveChanges();

            return categoriaBanco;
        }

        public List<CategoriaViewModel> BuscarAtivos(string chavePai)
        {
            var categorias = _mapper.Map<List<CategoriaViewModel>>(_repositorio.BuscarAtivos(chavePai));
            foreach (var item in categorias)
            {
                item.Nome = _location.GetTranslation(item.ChaveTraducao);
            }
            return categorias;
        }

        public List<Categoria> BuscarCategorias()
        {
            return _repositorio.BuscarCategorias();
        }

        public List<CategoriaViewModel> BuscarCategoriasAleatorias()
        {
            var categorias = Get(w => w.Ativo)
            .OrderBy(o => Guid.NewGuid()).Take(12).ToList();

            return categorias;
        }

        public Categoria CriarCategoria(CriarCategoriaViewModel categoria)
        {
            var categorias = _repositorio.GetAll();

            if (categorias.Any(c => c.Nome == categoria.Nome && c.IdCategoriaPai == categoria.IdCategoriaPai))
            {
                throw new PadraoException("categoria_em_uso");
            }

            if (categoria.IdCategoriaPai == null || categoria.IdCategoriaPai == 0)
            {
                categoria.IdCategoriaPai = 3;
            }

            _repositorio.Insert(new Categoria
            {
                Ativo = true,
                IdCategoriaPai = categoria.IdCategoriaPai,
                Nome = categoria.Nome,
                DataCriacao = DateTime.UtcNow.HorarioBrasilia(),
                DataAtualizacao = DateTime.UtcNow.HorarioBrasilia()
            });

            _repositorio.SaveChanges();

            return _repositorio.FirstNoTracking(c => c.Nome == categoria.Nome && c.IdCategoriaPai == categoria.IdCategoriaPai);
        }

        public void DeletarCategoria(int idCategoria, bool deleteTodos = false)
        {
            var categoria = _repositorio.First(c => c.IdCategoria == idCategoria);

            if (categoria == null)
            {
                throw new NotFoundException("categoria_nao_encontrada");
            }

            var categoriasFilhas = _repositorio.Get(c => c.IdCategoriaPai == idCategoria);

            if (deleteTodos)
            {
                foreach (var filha in categoriasFilhas)
                {
                    DeletarCategoria(filha.IdCategoria, true);
                }
            }
            else
            {
                foreach (var filha in categoriasFilhas)
                {
                    filha.IdCategoriaPai = null;
                    _repositorio.Update(filha);
                }
            }

            _repositorio.Delete(categoria.IdCategoria);

            _repositorio.SaveChanges();
        }

        public void DeletarCategoria(int idCategoria)
        {


            var mapeamentos = _mapNegocio.Get(m => m.IdCategoria == idCategoria);

            foreach (var map in mapeamentos)
            {
                _mapNegocio.Delete(map.IdCategoriaAnunciante);
            }

            _repositorio.Delete(idCategoria);

            _mapNegocio.SaveChanges();
            _repositorio.SaveChanges();
        }

        public CategoriaAnuncianteViewModel Map(CategoriaAnuncianteMapViewModel map)
        {
            var categoria = _repositorio.FirstNoTracking(c => c.IdCategoria == map.IdCategoria);

            if (categoria == null)
                throw new NotFoundException("categoria_nao_encontrada");

            if (_mapNegocio.GetAll().Any(m => m.IdAnunciante == map.IdAnunciante && m.IdCategoria == map.IdCategoria))
                throw new PadraoException("anunciante_categoria_mapeado");

            _mapNegocio.Insert(new CategoriaAnuncianteViewModel
            {
                IdAnunciante = map.IdAnunciante,
                IdCategoria = map.IdCategoria,
                Ativo = true
            });

            _mapNegocio.SaveChanges();

            return _mapNegocio.FirstNoTracking(m =>
            m.IdCategoria == map.IdCategoria &&
            m.IdAnunciante == map.IdAnunciante,
            "Categoria",
            "Anunciante");
        }

        public List<CategoriaAnunciante> ObterMapeamentos()
        {
            return _repositorio.ObterMapeamentos();
        }

        public bool RemoveMap(int idMap)
        {
            _mapNegocio.Delete(idMap);

            return true;
        }

        public List<CategoriaViewModel> GetCategorias(FiltroHomeCategoriaViewModel filtro)
        {
            return GetCategoriasFromCache(filtro).ToList();
        }

        private IList<CategoriaViewModel> GetCategoriasFromCache(FiltroHomeCategoriaViewModel filtro)
        {
            var categorias = (List<CategoriaViewModel>)_cache.GetItem(CacheKeys.Categorias);

            if (categorias == null)
            {
                categorias = _repositorio.ObterCategorias(filtro);

                _cache.SetItem(CacheKeys.Categorias, categorias);
            }

            return categorias;
        }

        private List<Cupom> GetCuponsFromCache()
        {
            var cupons = (List<Cupom>)_cache.GetItem(CacheKeys.Cupom);
            if (cupons != null && cupons.Count > 0) return cupons;

            var afilio = new AfilioBLL();
            var ret = afilio.GetCupons().Result;

            _cache.SetItem(CacheKeys.Cupom, ret);
            cupons = (List<Cupom>)_cache.GetItem(CacheKeys.Cupom);

            return cupons;
        }
    }
}
