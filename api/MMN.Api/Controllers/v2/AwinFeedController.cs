using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Caching.Memory;
using MMN.Api.Models.Response;
using MMN.Api.Services;
using MMN.Dominio.Model;
using MMN.Integracoes.Zanox;
using MMN.Util.Cache;
using NuGet.Protocol.Core.Types;

namespace MMN.Api.Controllers.v2
{
    [Route("api/v2/awin-feed"), ApiController]
    public class AwinFeedController : ControllerBase
    {
        private readonly CacheManager _cache;
        private readonly IAwinFeedService _awinFeedService;

        public AwinFeedController(IAwinFeedService awinFeedService, IMemoryCache cache)
        {
            _awinFeedService = awinFeedService;
            _cache = new CacheManager(cache);
        }

        [HttpGet("get-product")]
        public IActionResult GetProduct(string id)
        {
            try
            {
                var product = _awinFeedService.GetProduct(id);

                return Ok(new ApiResponse<object>().SuccessResponse(product, "Consulta de produto realizada com sucesso."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>().ErrorResponse($"Erro ao buscar o produto: {ex.Message}"));
            }
            finally
            {
            }
        }

        [HttpGet("get-products")]
        public IActionResult GetProducts(int quantity, int page, decimal? minPrice, decimal? maxPrice, string order, string category)
        {
            try
            {
                //var cacheKey = "awin_feed_products";
                //var cachedResult = _cache.GetItem(cacheKey);

                //if (cachedResult != null)
                //{
                //    return Ok(new ApiResponse<object>().SuccessResponse(cachedResult, "Consulta de produtos realizada com sucesso a partir do cache."));
                //}

                var result = _awinFeedService.GetProducts(quantity, page, minPrice ?? 0, maxPrice ?? decimal.MaxValue, order, category);
                //_cache.SetItem(cacheKey, result);

                return Ok(new ApiResponse<object>().SuccessResponse(new { result.Products, result.TotalCount, result.PriceInterval }, "Consulta de produtos realizada com sucesso."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>().ErrorResponse($"Erro ao buscar os produtos: {ex.Message}"));
            }
            finally
            {
            }
        }

        [HttpGet("get-searched-products")]
        public IActionResult GetSearchedProducts(int quantity, int page, decimal? minPrice, decimal? maxPrice, string order, string searchText)
        {
            try
            {
                var result = _awinFeedService.GetSearchedProducts(quantity, page, minPrice ?? 0, maxPrice ?? decimal.MaxValue, order, searchText.Trim());

                return Ok(new ApiResponse<object>().SuccessResponse(new { result.Products, result.TotalCount, result.PriceInterval }, "Consulta de produtos realizada com sucesso."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>().ErrorResponse($"Erro ao buscar os produtos: {ex.Message}"));
            }
            finally
            {
            }
        }

        [HttpGet("get-categories")]
        public IActionResult GetCategories()
        {
            try
            {
                var cacheKey = "awin_feed_categories";
                var cachedResult = _cache.GetItem(cacheKey);

                if (cachedResult != null)
                {
                    return Ok(new ApiResponse<object>().SuccessResponse(cachedResult, "Consulta de categorias realizada com sucesso a partir do cache."));
                }

                var categories = _awinFeedService.GetCategories();
                _cache.SetItem(cacheKey, categories);

                return Ok(new ApiResponse<object>().SuccessResponse(categories, "Consulta de categorias realizada com sucesso."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>().ErrorResponse($"Erro ao buscar as categorias: {ex.Message}"));
            }
            finally
            {
            }
        }

        [HttpGet("get-best-brands")]
        public IActionResult GetBestBrands()
        {
            try
            {
                var cacheKey = "awin_feed_best_brands";
                var cachedResult = _cache.GetItem(cacheKey);

                if (cachedResult != null)
                {
                    return Ok(new ApiResponse<object>().SuccessResponse(cachedResult, "Consulta de melhores marcas realizada com sucesso a partir do cache."));
                }

                var brands = _awinFeedService.GetBestBrands();
                _cache.SetItem(cacheKey, brands);

                return Ok(new ApiResponse<object>().SuccessResponse(brands, "Consulta de melhores marcas realizada com sucesso."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>().ErrorResponse($"Erro ao buscar as melhores marcas: {ex.Message}"));
            }
            finally
            {
            }
        }
    }
}