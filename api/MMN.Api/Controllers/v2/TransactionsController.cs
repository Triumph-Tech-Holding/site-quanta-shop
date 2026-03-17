using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MMN.INegocio.Negocio;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using MMN.Api.Services;
using MMN.Api.Models.Response;
using MMN.Api.Models.Request;

namespace MMN.Api.Controllers.v2;

[Authorize(Roles = "Admin"), Route("api/v2/transactions"), ApiController]
public class TransactionsController : ControllerBase
{
    private readonly ITransactionsService _transactionService;

    public TransactionsController(ITransactionsService transactionService)
    {
        _transactionService = transactionService;
    }

    [HttpGet("get-transactions")]
    public async Task<IActionResult> GetTransactions([FromQuery] GetTransactionsSearchParameters parameters)
    {
        try
        {
            var (transactions, totalCount) = await _transactionService.GetTransactions(parameters);
            
            var response = new GetTransactionsResponse
            {
                Transactions = transactions,
                TotalCount = totalCount
            };

            return Ok(new ApiResponse<object>().SuccessResponse(response, "Consulta de transações realizada com sucesso."));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<object>().ErrorResponse($"Erro ao buscar transações: {ex.Message}"));
        }
    }

    [HttpGet("get-releases/{idTransaction}")]
    public async Task<IActionResult> GetReleases(long idTransaction)
    {
        try
        {
            var result = await _transactionService.GetReleases(idTransaction);
            return Ok(new ApiResponse<object>().SuccessResponse(result, "Consulta de lançamentos realizada com sucesso."));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<object>().ErrorResponse($"Erro ao buscar lançamentos: {ex.Message}"));
        }
    }

	[HttpDelete("delete-order-with-cashback-coupon/{orderId}")]
	public async Task<IActionResult> DeleteOrderWithCashbackCoupon(int orderId)
	{
		try
		{
			await _transactionService.DeleteOrderWithCashbackCoupon(orderId);
			return Ok(new ApiResponse<object>().SuccessResponse(null, "Exclusão do pedido realizada com sucesso"));
		}
		catch (Exception ex)
		{
			return StatusCode(500, new ApiResponse<object>().ErrorResponse($"Erro ao excluir pedido: {ex.Message}"));
		}
	}
}
