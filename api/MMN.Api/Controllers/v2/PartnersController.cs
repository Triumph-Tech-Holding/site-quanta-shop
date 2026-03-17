using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using MMN.Api.Helpers;
using MMN.Api.Models.Response;
using MMN.Api.Services;
using MMN.Integracoes.Zanox;
using System;

namespace MMN.Api.Controllers.v2;

[Route("api/v2/partners"), ApiController]
public class PartnersController : ControllerBase
{
    private readonly CacheManager _cache;
    private readonly RedisCacheManager _redis;
    private readonly IPartnersService _partnersService;

    public PartnersController(IPartnersService partnersService, IMemoryCache cache, IDistributedCache redis)
    {
        _partnersService = partnersService;
        _cache = new CacheManager(cache);
        _redis = new RedisCacheManager(redis);
    }

    [HttpGet("get-partners"), AllowAnonymous]
    public IActionResult GetPartners(string type = null, string name = null, string category = null, int page = 1, int limit = 12)
    {
        try
        {
            var partners = _partnersService.GetPartners(type, name, category, page, limit);

            return Ok(new ApiResponse<object>().SuccessResponse(partners, "Consulta de parceiros realizada com sucesso."));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<object>().ErrorResponse($"Erro ao buscar parceiros: {ex.Message}"));
        }
    }

    [HttpGet("get-partner/{id}"), AllowAnonymous]
    public IActionResult GetPartner(int id)
    {
        try
        {
            var partners = _partnersService.GetPartner(id);

            return Ok(new ApiResponse<object>().SuccessResponse(partners, "Consulta de parceiro realizada com sucesso."));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<object>().ErrorResponse($"Erro ao buscar parceiro: {ex.Message}"));
        }
    }

    [HttpGet("get-new-partners"), AllowAnonymous]
    public IActionResult GetNewPartners()
    {
        try
        {
            var cacheKey = "new_partners";
            var cachedResult = _cache.GetItem(cacheKey);

            if (cachedResult != null)
            {
                return Ok(new ApiResponse<object>().SuccessResponse(cachedResult, "Consulta de novos parceiros realizada com sucesso a partir do cache."));
            }

            var partners = _partnersService.GetNewPartners();

            _cache.SetItem(cacheKey, partners);

            return Ok(new ApiResponse<object>().SuccessResponse(partners, "Consulta de novos parceiros realizada com sucesso."));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<object>().ErrorResponse($"Erro ao buscar novos parceiros: {ex.Message}"));
        }
    }

    [HttpGet("get-featured-partners"), AllowAnonymous]
    public IActionResult GetFeaturedPartners()
    {
        try
        {
            var cacheKey = "featured_partners";
            var cachedResult = _cache.GetItem(cacheKey);

            if (cachedResult != null)
            {
                return Ok(new ApiResponse<object>().SuccessResponse(cachedResult, "Consulta de parceiros em destaque realizada com sucesso a partir do cache."));
            }

            var partners = _partnersService.GetFeaturedPartners();

            _cache.SetItem(cacheKey, partners);

            return Ok(new ApiResponse<object>().SuccessResponse(partners, "Consulta de parceiros em destaque realizada com sucesso."));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<object>().ErrorResponse($"Erro ao buscar parceiros em destaque: {ex.Message}"));
        }
    }

    [HttpGet("get-top-sellers-partners"), AllowAnonymous]
    public IActionResult GetTopSellersPartners()
    {
        try
        {
            var cacheKey = "top_sellers_partners";
            var cachedResult = _cache.GetItem(cacheKey);

            if (cachedResult != null)
            {
                return Ok(new ApiResponse<object>().SuccessResponse(cachedResult, "Consulta de parceiros que mais vendem realizada com sucesso a partir do cache."));
            }

            var partners = _partnersService.GetTopSellersPartners();

            _cache.SetItem(cacheKey, partners);

            return Ok(new ApiResponse<object>().SuccessResponse(partners, "Consulta de parceiros que mais vendem realizada com sucesso."));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<object>().ErrorResponse($"Erro ao buscar parceiros que mais vendem: {ex.Message}"));
        }
    }

    [HttpGet("get-local-partners"), AllowAnonymous]
    public IActionResult GetLocalPartners()
    {
        try
        {
            var cacheKey = "local_partners";
            var cachedResult = _cache.GetItem(cacheKey);

            if (cachedResult != null)
            {
                return Ok(new ApiResponse<object>().SuccessResponse(cachedResult, "Consulta de parceiros locais realizada com sucesso a partir do cache."));
            }

            var partners = _partnersService.GetLocalPartners();

            _cache.SetItem(cacheKey, partners);

            return Ok(new ApiResponse<object>().SuccessResponse(partners, "Consulta de parceiros locais realizada com sucesso."));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<object>().ErrorResponse($"Erro ao buscar parceiros locais: {ex.Message}"));
        }
    }

    [HttpGet("get-best-discounts-local-partners"), AllowAnonymous]
    public IActionResult GetBestDiscountsLocalPartners()
    {
        try
        {
            var cacheKey = "best_discounts_partners";
            var cachedResult = _cache.GetItem(cacheKey);

            if (cachedResult != null)
            {
                return Ok(new ApiResponse<object>().SuccessResponse(cachedResult, "Consulta de melhores ofertas de parceiros locais realizada com sucesso a partir do cache."));
            }

            var partners = _partnersService.GetBestDiscountsLocalPartners();

            _cache.SetItem(cacheKey, partners);

            return Ok(new ApiResponse<object>().SuccessResponse(partners, "Consulta de melhores ofertas de parceiros locais realizada com sucesso."));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<object>().ErrorResponse($"Erro ao buscar melhores ofertas de parceiros locais: {ex.Message}"));
        }
    }

    [HttpGet("get-featured-local-partners"), AllowAnonymous]
    public IActionResult GetFeaturedLocalPartners()
    {
        try
        {
            var cacheKey = "featured_local_partners";
            var cachedResult = _cache.GetItem(cacheKey);

            if (cachedResult != null)
            {
                return Ok(new ApiResponse<object>().SuccessResponse(cachedResult, "Consulta de parceiros locais em destaque realizada com sucesso a partir do cache."));
            }

            var partners = _partnersService.GetFeaturedLocalPartners();

            _cache.SetItem(cacheKey, partners);

            return Ok(new ApiResponse<object>().SuccessResponse(partners, "Consulta de parceiros locais em destaque realizada com sucesso."));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<object>().ErrorResponse($"Erro ao buscar parceiros locais em destaque: {ex.Message}"));
        }
    }

    [HttpGet("get-top-sellers-local-partners"), AllowAnonymous]
    public IActionResult GetTopSellersLocalPartners()
    {
        try
        {
            var cacheKey = "top_sellers_local_partners";
            var cachedResult = _cache.GetItem(cacheKey);

            if (cachedResult != null)
            {
                return Ok(new ApiResponse<object>().SuccessResponse(cachedResult, "Consulta de parceiros locais que mais vendem realizada com sucesso a partir do cache."));
            }

            var partners = _partnersService.GetTopSellersLocalPartners();

            _cache.SetItem(cacheKey, partners);

            return Ok(new ApiResponse<object>().SuccessResponse(partners, "Consulta de parceiros locais que mais vendem realizada com sucesso."));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<object>().ErrorResponse($"Erro ao buscar parceiros locais que mais vendem: {ex.Message}"));
        }
    }
}
