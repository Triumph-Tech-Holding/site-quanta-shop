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
    [Route("api/v2/awin-webhook"), ApiController]
    public class AwinWebhookController : ControllerBase
    {
        private readonly CacheManager _cache;
        private readonly IAwinWebhookService _awinWebhookService;

        public AwinWebhookController(IAwinWebhookService awinWebhookService, IMemoryCache cache)
        {
            _awinWebhookService = awinWebhookService;
            _cache = new CacheManager(cache);
        }

        [HttpPost]
        public async Task<IActionResult> Post(AwinWebhookRequest request)
        {
            try
            {
                await _awinWebhookService.CreateAsync(request);

                return Ok(new ApiResponse<object>().SuccessResponse(request, "Consulta de webhook realizada com sucesso."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>().ErrorResponse($"Erro ao buscar o produto: {ex.Message}"));
            }
            finally
            {
            }
        }
    }

    public class AwinWebhookRequest
    {
        public string TransactionId { get; set; }
        public string TransactionDate { get; set; }
        public string TransactionCurrency { get; set; }
        public string TransactionAmount { get; set; }
        public string AffiliateId { get; set; }
        public string MerchantId { get; set; }
        public string BannerId { get; set; }
        public string ClickRef { get; set; }
        public string ClickThroughTime { get; set; }
        public string Commission { get; set; }
    }
}