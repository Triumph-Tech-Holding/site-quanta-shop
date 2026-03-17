using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MMN.Dominio.Excecao;
using MMN.Dominio.Model;
using MMN.Dominio.ViewModel;
using MMN.INegocio.Negocio;
using MMN.Integracoes.Afilio;
using MMN.IRepositorio.Repositorio;
using MMN.Negocio.Base;
using MMN.Util.Cache;
using MMN.Util.Enum;
using MMN.Util.Extensions;
using MMN.Util.Model;
using MMN.Util.Util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MMN.Negocio.Negocio
{
    public class AnuncianteNegocio : BaseNegocio<AnuncianteViewModel, Anunciante>, IAnuncianteNegocio
    {
        private readonly IAnuncianteRepositorio _repositorio;
        private readonly ICredenciamentoNegocio _credenciamentoNegocio;
        public IOrdenacaoAnuncioRepositorio _ordenacaoAnuncioRepositorio { get; set; }
        private readonly IMapper _mapper;
        private readonly ICache _cache;
        private readonly AppSettings _appSettings;
        public AnuncianteNegocio(
            IAnuncianteRepositorio repositorio,
            ICredenciamentoNegocio credenciamentoNegocio,
            IOrdenacaoAnuncioRepositorio ordenacaoAnuncioRepositorio,
            IMapper mapper,
            IOptions<AppSettings> appSettings,
            ICache cache) : base(repositorio, mapper)

        {
            _repositorio = repositorio;
            _credenciamentoNegocio = credenciamentoNegocio;
            _ordenacaoAnuncioRepositorio = ordenacaoAnuncioRepositorio;
            _mapper = mapper;
            _cache = cache;
            _appSettings = appSettings.Value;
        }

        public async Task<object> BuscarAnunciantes(FiltroViewModel.AnunciantePaginado filtro)
        {
            IEnumerable<AnuncianteViewModel> anunciantes;
            var cupons = await GetCuponsFromCache();
            anunciantes = new List<AnuncianteViewModel>();

            if (filtro.TipoAnunciante.Count == 0 ||
                (filtro.TipoAnunciante.Any(f => f == Util.Enum.TipoAnuncianteEnum.LojaOnline) &&
                !filtro.TipoAnunciante.Any(f => f == Util.Enum.TipoAnuncianteEnum.LojaFisica)))
            {
                // Busca pelos anunciantes da Awin e Afilio
                if (filtro.IdCategoria.HasValue)
                {
                    anunciantes = GetAll("AnuncianteCashBack",
                        "CategoriaAnunciante");

                    anunciantes = anunciantes.Where(w =>
                        (!string.IsNullOrEmpty(filtro.Descricao) ? w.Nome.Contains(filtro.Descricao) : true)
                        && w.Ativo
                        && w.AnuncianteCashBack.Count > 0
                        && w.CategoriaAnunciante.Any(a => a.IdCategoria == filtro.IdCategoria.Value)
                        && (!string.IsNullOrEmpty(w.IdAwin)))
                        .AsEnumerable();
                }
                else
                {
                    anunciantes = Get(w =>
                    (!string.IsNullOrEmpty(filtro.Descricao) ? w.Nome.Contains(filtro.Descricao) : true)
                    && w.Ativo
                    && (!string.IsNullOrEmpty(w.IdAwin))
                    && w.AnuncianteCashBack.Count > 0,
                    "AnuncianteCashBack",
                    "CategoriaAnunciante")
                    .AsEnumerable()
                    .OrderByDescending(x => x.Prioridade).ThenBy(x => x.Nome);
                }

                anunciantes = anunciantes.Where(w => (!string.IsNullOrEmpty(w.IdAfilio) ? cupons.Any(a => a.creative_id == w.IdAfilio) : true));
                foreach (var a in anunciantes)
                {
                    a.Tipo = Util.Enum.TipoAnuncianteEnum.LojaOnline;
                }
            }

            // Busca pelos credenciamentos
            //if ((!filtro.TipoAnunciante.HasValue) || (filtro.TipoAnunciante.HasValue && filtro.TipoAnunciante.Value == Util.Enum.TipoAnuncianteEnum.LojaFisica))
            if (filtro.TipoAnunciante.Count == 0 || (filtro.TipoAnunciante.Any(f => f == Util.Enum.TipoAnuncianteEnum.LojaFisica) && !filtro.TipoAnunciante.Any(f => f == Util.Enum.TipoAnuncianteEnum.LojaOnline)))
            {
                var credenciados = GetAnunciantesCredenciadosFromCache();

                credenciados = credenciados.Where(c =>
                     !string.IsNullOrEmpty(filtro.Descricao) ? c.Nome.ToLower().Contains(filtro.Descricao.ToLower()) : true &&
                     filtro.IdCategoria != null ? c.IdCategoria == filtro.IdCategoria.Value : true).ToList();

                foreach (var cred in credenciados)
                {
                    anunciantes = anunciantes.Append(new AnuncianteViewModel
                    {
                        IdCredenciado = cred.IdAnunciante,
                        Ativo = cred.Ativo,
                        Nome = cred.Nome,
                        ImagemUrl = cred.ImagemUrl,
                        Cashback = cred.Cashback,
                        CashbackMin = cred.Cashback,
                        CashbackMax = cred.Cashback,
                        Tipo = cred.Tipo
                    });
                }
            }

            // TODO: Implementar busca de lojas fisicas futuramente

            var totalPaginas = (int)Math.Ceiling((double)anunciantes.Count() / filtro.Quantidade);

            if (totalPaginas < filtro.Page)
                filtro.Page = 1;

            anunciantes = anunciantes.Skip(filtro.Quantidade * (filtro.Page - 1)).Take(filtro.Quantidade);

            //anunciantes = BuscaCashBackRecursivo(anunciantes.ToList(), 0, anunciantes.Count());
            anunciantes = BuscarCashback(anunciantes.ToList());
            return new { totalPaginas, filtro.Quantidade, filtro.Page, anunciantes };
        }

        public List<AnuncianteViewModel> BuscarCashback(List<AnuncianteViewModel> anunciantes)
        {
            foreach (var anunciante in anunciantes)
            {
                decimal? minimo = null;
                decimal? maximo = null;
                string tipo = string.Empty;
                string moeda = string.Empty;

                if (anunciante.AnuncianteCashBack != null)
                {
                    foreach (var cashback in anunciante.AnuncianteCashBack)
                    {
                        moeda = cashback.Moeda;
                        if (cashback.Descricao.Equals("Máximo"))
                        {
                            maximo = cashback.Percentual;
                            tipo = cashback.Tipo;
                            continue;
                        }

                        if (cashback.Descricao.Equals("Mínimo"))
                        {
                            minimo = cashback.Percentual;
                            tipo = cashback.Tipo;
                            continue;
                        }

                        if (cashback.Descricao.Equals("Afilio"))
                        {
                            minimo = cashback.Percentual;
                            maximo = cashback.Percentual;
                            tipo = cashback.Tipo;
                            continue;
                        }
                    }
                }
                anunciante.CashbackMin = minimo;
                anunciante.CashbackMax = maximo;
                anunciante.TipoCashback = tipo;
                anunciante.Moeda = moeda;
            }

            return anunciantes;
        }

        public AnuncianteViewModel BuscarCashbackAnunciante(AnuncianteViewModel anunciante)
        {


            decimal? minimo = null;
            decimal? maximo = null;
            string tipo = string.Empty;
            string moeda = string.Empty;

            if (anunciante.AnuncianteCashBack != null)
            {
                foreach (var cashback in anunciante.AnuncianteCashBack)
                {
                    moeda = cashback.Moeda;
                    if (cashback.Descricao.Equals("Máximo"))
                    {
                        maximo = cashback.Percentual;
                        tipo = cashback.Tipo;
                        continue;
                    }

                    if (cashback.Descricao.Equals("Mínimo"))
                    {
                        minimo = cashback.Percentual;
                        tipo = cashback.Tipo;
                        continue;
                    }

                    //if (cashback.Descricao.Equals("Afilio"))
                    //{
                    //    minimo = cashback.Percentual;
                    //    maximo = cashback.Percentual;
                    //    tipo = cashback.Descricao;
                    //    continue;
                    //}
                }
            }
            anunciante.CashbackMin = minimo;
            anunciante.CashbackMax = maximo;
            anunciante.TipoCashback = tipo;
            anunciante.Moeda = moeda;


            return anunciante;
        }

        public IEnumerable<AnuncianteViewModel> BuscarAnunciantesAleatorio()
        {
            var anunciantes = Get(w =>
            w.Ativo &&
            (!string.IsNullOrEmpty(w.IdAwin)))
            .OrderBy(o => Guid.NewGuid()).Take(12).AsEnumerable();

            return anunciantes;
        }

        private static IList<AnuncianteViewModel> BuscaCashBackRecursivo(IList<AnuncianteViewModel> anunciantes, int index, int total)
        {
            if (index == total)
                return anunciantes;
            else
            {
                anunciantes[index].PercentualMenor = anunciantes[index].AnuncianteCashBack.Where(w => w.Ativo && w.Percentual != 0).Min(m => m.Percentual);
                anunciantes[index].PercentualMaior = anunciantes[index].AnuncianteCashBack.Where(w => w.Ativo && w.Percentual != 0).Max(m => m.Percentual);
                anunciantes[index].ValorFixoMenor = anunciantes[index].AnuncianteCashBack.Where(w => w.Ativo && w.ValorFixo != 0).Min(m => m.ValorFixo);
                anunciantes[index].ValorFixoMaior = anunciantes[index].AnuncianteCashBack.Where(w => w.Ativo && w.ValorFixo != 0).Max(m => m.ValorFixo);
                anunciantes[index].AnuncianteCashBack = null;

                anunciantes[index].MostrarUmaPorcentagem = (anunciantes[index].PercentualMenor == anunciantes[index].PercentualMaior)
                    && (!anunciantes[index].ValorFixoMenor.HasValue);

                anunciantes[index].MostrarDuasPorcentagens = (anunciantes[index].PercentualMenor != anunciantes[index].PercentualMaior)
                    && (!anunciantes[index].ValorFixoMenor.HasValue);

                anunciantes[index].MostrarUmValorFixo = (anunciantes[index].ValorFixoMenor == anunciantes[index].ValorFixoMaior)
                    && (!anunciantes[index].PercentualMenor.HasValue);

                anunciantes[index].MostrarDoisValores = (anunciantes[index].ValorFixoMenor != anunciantes[index].ValorFixoMaior)
                    && (!anunciantes[index].PercentualMenor.HasValue);

                anunciantes[index].MostrarUmPercEUmValor = (anunciantes[index].ValorFixoMenor.HasValue && anunciantes[index].PercentualMenor.HasValue)
                    && (anunciantes[index].PercentualMenor == anunciantes[index].PercentualMaior && anunciantes[index].ValorFixoMenor == anunciantes[index].ValorFixoMaior);

                anunciantes[index].MostrarUmPercEDoisValores = (anunciantes[index].ValorFixoMenor.HasValue && anunciantes[index].PercentualMenor.HasValue)
                    && (anunciantes[index].PercentualMenor == anunciantes[index].PercentualMaior && anunciantes[index].ValorFixoMenor != anunciantes[index].ValorFixoMaior);

                anunciantes[index].MostrarDoisPercEUmValor = (anunciantes[index].ValorFixoMenor.HasValue && anunciantes[index].PercentualMenor.HasValue)
                    && (anunciantes[index].PercentualMenor != anunciantes[index].PercentualMaior && anunciantes[index].ValorFixoMenor == anunciantes[index].ValorFixoMaior);

                anunciantes[index].MostrarDoisPercEDoisValores = (anunciantes[index].ValorFixoMenor.HasValue && anunciantes[index].PercentualMenor.HasValue)
                    && (anunciantes[index].PercentualMenor != anunciantes[index].PercentualMaior && anunciantes[index].ValorFixoMenor != anunciantes[index].ValorFixoMaior);

                return BuscaCashBackRecursivo(anunciantes, index + 1, anunciantes.Count);
            }
        }

        public IList<CredenciamentoCache> GetAnunciantesCredenciadosFromCache()
        {
            var credenciamentos = (List<CredenciamentoCache>)_cache.GetItem(CacheKeys.Credenciamentos);
            if (credenciamentos != null && credenciamentos.Count > 0) return credenciamentos;

            var ret = _credenciamentoNegocio.Get(c => c.Usuario.Ativo, "Usuario");
            credenciamentos = new List<CredenciamentoCache>();
            foreach (var cred in ret)
            {
                if (cred.UsuarioViewModel.Ativo)
                {
                    credenciamentos.Add(new CredenciamentoCache
                    {
                        Ativo = true,
                        IdAnunciante = cred.IdCredenciamento,
                        Tipo = Util.Enum.TipoAnuncianteEnum.LojasNoApp,
                        Cashback = cred.PercentualCashback.HasValue ? cred.PercentualCashback.Value : 0,
                        ImagemUrl = cred.LogoUrl,
                        Nome = cred.Estabelecimento,
                        IdCategoria = cred.IdCategoria
                    });
                }
            }

            _cache.SetItem(CacheKeys.Credenciamentos, credenciamentos);

            return credenciamentos;
        }

        public async Task<IList<Cupom>> GetCuponsFromCache()
        {
            var cupons = (List<Cupom>)_cache.GetItem(CacheKeys.Cupom);
            if (cupons != null && cupons.Count > 0) return cupons;

            var afilio = new AfilioBLL();
            var ret = await afilio.GetCupons();

            _cache.SetItem(CacheKeys.Cupom, ret);
            cupons = (List<Cupom>)_cache.GetItem(CacheKeys.Cupom);

            return cupons;
        }

        public void EditarAnunciante(AnuncianteViewModel editar, Guid IdUsuarioLogado)
        {


            var dadosAnunciante = _repositorio.GetById(editar.IdAnunciante, "OrdenacaoAnuncio");

            var apagarImagem = editar.ImagemUrl;

            var logoUrl = dadosAnunciante.ImagemUrl;

            if (logoUrl == null)
            {
                logoUrl = "";
            }
            if (string.IsNullOrEmpty(apagarImagem) && logoUrl != "")
            {
                logoUrl = "";
            }



            else if (!string.IsNullOrEmpty(editar.Base64))
            {
                var image = Convert.FromBase64String(editar.Base64);
                using (var ms = new MemoryStream(image))
                {
                    var img = Image.FromStream(ms);

                    if (img.Width != img.Height)
                    {
                        throw new PadraoException("imagem_app_resolucao_maxima");
                    }
                }

                logoUrl = AzureStorage.CreateBlob(
                    editar.Base64,
                    new Guid(),
                    _appSettings.StorageAccountConnectionString,
                    "logos-empresas",
                    "image-" + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss"),
                    true
                ).Result;

            }
            if (dadosAnunciante.Ativo != editar.Ativo)
            {
                dadosAnunciante.EditadoUsuario = true;
            }

            dadosAnunciante.Ativo = editar.Ativo;
            //dadosAnunciante.OrdenacaoAnuncio.Ordenacao = editar.Prioridade;
            dadosAnunciante.DataAtualizacao = DateTime.UtcNow.HorarioBrasilia();
            dadosAnunciante.ImagemUrl = logoUrl;

            _repositorio.Update(dadosAnunciante);
            _repositorio.SaveChanges();
        }

        public async Task<IEnumerable<OrdenacaoAnuncioViewModel>> ObterOrdenacaoAnunciosFromCashAsync()
        {
            try
            {
                //OrdenacaoAnuncioViewModel[] resultado;

                var resultado = (OrdenacaoAnuncioViewModel[])_cache.GetItem("OrdenacaoAnuncios");
                if (resultado != null && resultado.Length > 0)
                {
                    return resultado;
                }

                var ordenacao = await _ordenacaoAnuncioRepositorio.GetAll()
                    .Include(i => i.Anunciante)
                        .ThenInclude(t => t.AnuncianteCashBack)
                    .Include(i => i.Anunciante)
                        .ThenInclude(i => i.CategoriaAnunciante)
                    .Include(i => i.Credenciamento)
                    .Where(w => w.Anunciante.Ativo ||
                        (w.Credenciamento.Usuario.Ativo && w.Credenciamento.Status == StatusCredenciamento.Aprovado))
                    .OrderByDescending(o => o.Ordenacao)
                    .ThenBy(x => x.Anunciante.Nome)
                    .ToArrayAsync();

                var viewModel = ordenacao.Select(s => new OrdenacaoAnuncioViewModel
                {
                    IdAnunciante = s.IdAnunciante,
                    IdCredenciamento = s.IdCredenciamento,
                    IdCategoria = s.Credenciamento != null ? s.Credenciamento.IdCategoria : s.Anunciante.CategoriaAnunciante.FirstOrDefault()?.IdCategoria,
                    Cashback = s.Credenciamento != null ? s.Credenciamento.PercentualCashback : null,
                    CashbackMax = s.Anunciante != null ?
                        s.Anunciante.AnuncianteCashBack
                            .Where(w => w.Ativo == true && (w.Descricao.Equals("Máximo") || w.Descricao.Equals("Afilio")))
                            .Select(se => se.Percentual)
                            .DefaultIfEmpty()
                            .Max() :
                        null,
                    CashbackMin = s.Anunciante != null ?
                        s.Anunciante.AnuncianteCashBack
                            .Where(w => w.Ativo == true && (w.Descricao.Equals("Mínimo") || w.Descricao.Equals("Afilio")))
                            .Select(se => se.Percentual)
                            .DefaultIfEmpty()
                            .Min() :
                        null,
                    ImagemUrl = s.Anunciante != null ? s.Anunciante.ImagemUrl : s.Credenciamento.LogoUrl,
                    Nome = s.Anunciante != null ? s.Anunciante.Nome : s.Credenciamento.Estabelecimento,
                    Ordenacao = s.Ordenacao,
                    ParceiroOnline = s.ParceiroOnline,
                    TipoCashback = s.Anunciante != null ?
                        s.Anunciante.AnuncianteCashBack
                            .Where(w => w.Ativo == true && (w.Descricao.Equals("Máximo") || w.Descricao.Equals("Mínimo") || w.Descricao.Equals("Afilio")))
                            .Select(se => se.Tipo)
                            .LastOrDefault() :
                        "percentage",
                    UrlAnuncio = GerarUrlAsync(s.Anunciante).Result,
                });

                resultado = viewModel.ToArray();
                _cache.SetItem("OrdenacaoAnuncios", resultado, DateTime.Now.AddHours(2));

                return resultado;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<string> GerarUrlAsync(Anunciante anunciante)
        {
            if (anunciante == null)
            {
                return null;
            }

            string url = null;

            if (!string.IsNullOrEmpty(anunciante?.IdAwin))
            {
                url = $"https://www.awin1.com/cread.php?awinmid={anunciante.IdAwin}&awinaffid={anunciante.AccountId}&clickref=[IdUsuario]";
            }
            else if (!string.IsNullOrEmpty(anunciante?.IdAfilio))
            {
                //Afilio
                try
                {
                    var cupons = await GetCuponsFromCache();
                    var cupom = cupons.FirstOrDefault(f => f.id_campaign == anunciante.IdAfilio);

                    if (cupom != null)
                        url = $"{cupom.site_name}&aff_xtra=[IdUsuario]";
                }
                catch
                {
                    return null;
                }
            }

            return url;
        }

        public async Task<string> GerarUrlAsync(long? anuncianteId)
        {
            if (anuncianteId == null)
            {
                return null;
            }

            var anunciante = _repositorio.First(f => f.Ativo && f.IdAnunciante == anuncianteId);

            return await GerarUrlAsync(anunciante);
        }
    }
}
