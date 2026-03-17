using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using MMN.Api.Controllers.v1;
using MMN.Api.Models.Response;
using MMN.Api.Services;
using MMN.Dominio.Model;
using MMN.Util.Cache;
using System;
using System.Linq;

namespace MMN.Api.Controllers.v2;

[Route("api/v2/carousels"), ApiController, Authorize(Roles = "Admin")]
public class CarouselsController : LoggedControllerBase
{
    private readonly CacheManager _cache;
    private readonly ICarouselsService _carouselsService;

    public CarouselsController(ICarouselsService carouselsService, IMemoryCache cache)
    {
        _carouselsService = carouselsService;
        _cache = new CacheManager(cache);
    }

    [HttpGet, AllowAnonymous]
    public IActionResult GetCarousels()
    {
        try
        {
            var cacheKey = "carrousels";
            var cachedResult = _cache.GetItem(cacheKey);

            if (cachedResult != null)
            {
                return Ok(new ApiResponse<object>().SuccessResponse(cachedResult, "Consulta de carrosséis ativos realizada com sucesso a partir do cache."));
            }

            var carrousels = _carouselsService.GetCarousels();
            
            _cache.SetItem(cacheKey, carrousels);

            return Ok(new ApiResponse<object>().SuccessResponse(carrousels, "Consulta de carrosséis ativos realizada com sucesso."));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<object>().ErrorResponse($"Erro ao buscar os carrosséis: {ex.Message}"));
        }
    }

    [HttpGet("{id}"), AllowAnonymous]
    public IActionResult GetCarrosselById(int id)
    {
        try
        {
            var carrossel =  _carouselsService.GetCarouselById(id);
            
            if (carrossel == null)
                return NotFound(new ApiResponse<object>().ErrorResponse("Carrossel não encontrado."));
            
            return Ok(new ApiResponse<object>().SuccessResponse(carrossel, "Consulta de carrossel realizada com sucesso."));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<object>().ErrorResponse($"Erro ao buscar o carrossel: {ex.Message}"));
        }
    }

    [HttpPost, Authorize(Roles = "Admin")]
    public IActionResult AddCarrossel([FromBody] Carrossel carrossel)
    {
        try
        {
            var addedCarrossel = _carouselsService.AddCarousel(carrossel);

            var cacheKey = "carrousels";
            var carrousels = _carouselsService.GetCarousels();

            _cache.SetItem(cacheKey, carrousels);

            return CreatedAtAction(nameof(GetCarrosselById), new { id = addedCarrossel.IdCarrossel }, new ApiResponse<object>().SuccessResponse(addedCarrossel, "Carrossel adicionado com sucesso."));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<object>().ErrorResponse($"Erro ao adicionar o carrossel: {ex.Message}"));
        }
    }

    [HttpPut("{id}"), Authorize(Roles = "Admin")]
    public IActionResult UpdateCarrossel(int id, [FromBody] Carrossel carrossel)
    {
        try
        {
            var existingCarrossel = _carouselsService.GetCarouselById(id);
            
            if (existingCarrossel == null)
                return NotFound(new ApiResponse<object>().ErrorResponse("Carrossel não encontrado."));

            carrossel.IdCarrossel = id;
            
            var updatedCarrossel = _carouselsService.UpdateCarousel(carrossel);

            var cacheKey = "carrousels";
            var carrousels = _carouselsService.GetCarousels();

            _cache.SetItem(cacheKey, carrousels);

            return Ok(new ApiResponse<object>().SuccessResponse(updatedCarrossel, "Carrossel atualizado com sucesso."));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<object>().ErrorResponse($"Erro ao atualizar o carrossel: {ex.Message}"));
        }
    }

    [HttpPatch("{id}"), Authorize(Roles = "Admin")]
    public IActionResult PatchCarrossel(int id, [FromBody] Carrossel carrossel)
    {
        try
        {
            var existingCarrossel = _carouselsService.GetCarouselById(id);

            if (existingCarrossel == null)
                return NotFound(new ApiResponse<object>().ErrorResponse("Carrossel não encontrado."));

            var updatedCarrossel = _carouselsService.UpdateCarousel(carrossel);
            var cacheKey = "carrousels";
            var carrousels = _carouselsService.GetCarousels();
            
            _cache.SetItem(cacheKey, carrousels);

            return Ok(new ApiResponse<object>().SuccessResponse(updatedCarrossel, "Carrossel atualizado com sucesso."));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<object>().ErrorResponse($"Erro ao atualizar o carrossel: {ex.Message}"));
        }
    }

    [HttpDelete("{id}"), Authorize(Roles = "Admin")]
    public IActionResult DeleteCarrossel(int id)
    {
        try
        {
            var success = _carouselsService.DeleteCarousel(id);
           
            if (!success)
                return NotFound(new ApiResponse<object>().ErrorResponse("Carrossel não encontrado."));

            var cacheKey = "carrousels";
            var carrousels = _carouselsService.GetCarousels();

            _cache.SetItem(cacheKey, carrousels);

            return Ok(new ApiResponse<object>().SuccessResponse(null, "Carrossel excluído com sucesso."));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<object>().ErrorResponse($"Erro ao excluir o carrossel: {ex.Message}"));
        }
    }
}