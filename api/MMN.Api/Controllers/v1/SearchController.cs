using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MMN.Repositorio.Contexto;

namespace MMN.Api.Controllers.v1
{
    [Route("api/busca-inteligente")]
    [ApiController]
    [AllowAnonymous]
    public class SearchController : LoggedControllerBase
    {
        private readonly DatabaseContext _context;

        public SearchController(DatabaseContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Buscar(
            [FromQuery] string q = null,
            [FromQuery] decimal minCashback = 0,
            [FromQuery] decimal maxDistance = 25,
            [FromQuery] string categoria = null,
            [FromQuery] string sort = "cashback",
            [FromQuery] decimal? lat = null,
            [FromQuery] decimal? lng = null,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 30)
        {
            try
            {
                var palette = new[] { "#FF6B35", "#0086FF", "#00A859", "#E50914", "#225F6B", "#E4002B", "#7B3F00", "#000000", "#FF8C00", "#8B4513" };

                var anuncQry = _context.Anunciante
                    .Where(a => a.Ativo)
                    .Include(a => a.AnuncianteCashBack)
                    .Include(a => a.CategoriaAnunciante).ThenInclude(ca => ca.Categoria)
                    .AsNoTracking();

                if (!string.IsNullOrWhiteSpace(q))
                {
                    var ql = q.ToLower();
                    anuncQry = anuncQry.Where(a =>
                        a.Nome != null && a.Nome.ToLower().Contains(ql));
                }

                var anunciantes = anuncQry.Take(500).ToList();

                var raw = anunciantes.Select(a => new
                {
                    id = (long)a.IdAnunciante,
                    nome = a.Nome ?? "—",
                    descricao = string.Empty,
                    categorias = a.CategoriaAnunciante?
                        .Where(ca => ca.Ativo && ca.Categoria != null)
                        .Select(ca => new { id = ca.Categoria.IdCategoria, nome = ca.Categoria.Nome ?? "" })
                        .ToList() ?? new List<dynamic>().Cast<dynamic>().Select(x => new { id = 0, nome = "" }).ToList(),
                    cashback = (decimal)a.Cashback,
                    rating = 4.5m,
                    transacoes = 0,
                    preco = (decimal?)null,
                    tipo = "loja",
                    distancia = 0m,
                });

                var list = raw.Select(r => new SearchResultDto
                {
                    Id = r.id,
                    Nome = r.nome,
                    Descricao = r.descricao,
                    Categoria = SlugFromCategoria(r.categorias?.FirstOrDefault()?.nome),
                    Cashback = r.cashback,
                    Distancia = r.distancia,
                    Rating = r.rating,
                    Transacoes = r.transacoes,
                    Preco = r.preco,
                    Tipo = r.tipo,
                    Color = palette[Math.Abs(r.nome.GetHashCode()) % palette.Length],
                }).ToList();

                if (!string.IsNullOrWhiteSpace(categoria))
                {
                    list = list.Where(x => string.Equals(x.Categoria, categoria, StringComparison.OrdinalIgnoreCase)).ToList();
                }
                if (minCashback > 0)
                {
                    list = list.Where(x => x.Cashback >= minCashback).ToList();
                }

                if (lat.HasValue && lng.HasValue)
                {
                    foreach (var item in list)
                    {
                        item.Distancia = EstimateDistanceFor(item.Id, lat.Value, lng.Value);
                    }
                    list = list.Where(x => x.Distancia <= maxDistance).ToList();
                }

                list = (sort ?? "cashback").ToLowerInvariant() switch
                {
                    "distance" => list.OrderBy(x => x.Distancia).ThenByDescending(x => x.Cashback).ToList(),
                    "popular" => list.OrderByDescending(x => x.Transacoes).ThenByDescending(x => x.Cashback).ToList(),
                    "price-asc" => list.OrderBy(x => x.Preco ?? decimal.MaxValue).ToList(),
                    "price-desc" => list.OrderByDescending(x => x.Preco ?? 0m).ToList(),
                    _ => list.OrderByDescending(x => x.Cashback).ThenBy(x => x.Distancia).ToList(),
                };

                var total = list.Count;
                var paged = list.Skip(Math.Max(0, (page - 1) * pageSize)).Take(Math.Max(1, pageSize)).ToList();

                return Ok(new
                {
                    items = paged,
                    total,
                    page,
                    pageSize,
                    fonte = "anunciante+cashback",
                    geradoEm = DateTime.UtcNow,
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "erro_busca_inteligente", detail = ex.Message });
            }
        }

        private decimal EstimateDistanceFor(long idAnunciante, decimal lat, decimal lng)
        {
            try
            {
                var cred = _context.Credenciamento
                    .AsNoTracking()
                    .Where(c => c.IdCredenciamento == idAnunciante &&
                                !string.IsNullOrEmpty(c.Latitude) && !string.IsNullOrEmpty(c.Longitude))
                    .Select(c => new { c.Latitude, c.Longitude })
                    .FirstOrDefault();

                if (cred == null) return 0m;
                if (!decimal.TryParse(cred.Latitude, NumberStyles.Any, CultureInfo.InvariantCulture, out var clat)) return 0m;
                if (!decimal.TryParse(cred.Longitude, NumberStyles.Any, CultureInfo.InvariantCulture, out var clng)) return 0m;
                return Haversine(lat, lng, clat, clng);
            }
            catch
            {
                return 0m;
            }
        }

        private static decimal Haversine(decimal lat1, decimal lon1, decimal lat2, decimal lon2)
        {
            const double R = 6371d;
            var dLat = ToRad((double)(lat2 - lat1));
            var dLon = ToRad((double)(lon2 - lon1));
            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Cos(ToRad((double)lat1)) * Math.Cos(ToRad((double)lat2)) *
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return (decimal)Math.Round(R * c, 2);
        }

        private static double ToRad(double v) => v * Math.PI / 180d;

        private static string SlugFromCategoria(string nome)
        {
            if (string.IsNullOrWhiteSpace(nome)) return "outros";
            var n = nome.ToLowerInvariant();
            if (n.Contains("merc")) return "mercado";
            if (n.Contains("farm")) return "farmacia";
            if (n.Contains("rest") || n.Contains("food") || n.Contains("comida")) return "restaurante";
            if (n.Contains("moda") || n.Contains("vest")) return "moda";
            if (n.Contains("eletr") || n.Contains("tech")) return "eletronicos";
            if (n.Contains("bele") || n.Contains("cosm")) return "beleza";
            if (n.Contains("casa") || n.Contains("decor")) return "casa";
            return n.Trim().Replace(" ", "-");
        }

        private class SearchResultDto
        {
            public long Id { get; set; }
            public string Nome { get; set; }
            public string Descricao { get; set; }
            public string Categoria { get; set; }
            public decimal Cashback { get; set; }
            public decimal Distancia { get; set; }
            public decimal Rating { get; set; }
            public int Transacoes { get; set; }
            public decimal? Preco { get; set; }
            public string Tipo { get; set; }
            public string Color { get; set; }
        }
    }
}
