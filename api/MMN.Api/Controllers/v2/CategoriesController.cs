using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MMN.INegocio.Negocio;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using MMN.Api.Services;
using MMN.Api.Models.Response;
using MMN.Api.Models.Request;
using MMN.Util.Cache;
using Microsoft.Extensions.Caching.Memory;

namespace MMN.Api.Controllers.v2;

[Authorize(Roles = "Admin"), Route("api/v2/categories"), ApiController]
public class CategoriesController : ControllerBase
{
    private readonly CacheManager _cache;
    private readonly ICategoriesService _categoriesService;

    public CategoriesController(ICategoriesService categoriesService, IMemoryCache cache)
    {
        _categoriesService = categoriesService;
        _cache = new CacheManager(cache);
    }

    [HttpGet("get-featured-categories"), AllowAnonymous]
    public IActionResult GetFeaturedCategories()
    {
        try
        {
            var cacheKey = "featured-categories";
            var cachedResult = _cache.GetItem(cacheKey);

            if (cachedResult != null)
            {
                return Ok(new ApiResponse<object>().SuccessResponse(cachedResult, "Consulta de categorias realizada com sucesso a partir do cache."));
            }

            var categories = _categoriesService.GetFeaturedCategories();

            _cache.SetItem(cacheKey, categories);

            return Ok(new ApiResponse<object>().SuccessResponse(categories, "Consulta de categorias em destaque realizada com sucesso."));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<object>().ErrorResponse($"Erro ao buscar categorias em destaque: {ex.Message}"));
        }
    }
}
