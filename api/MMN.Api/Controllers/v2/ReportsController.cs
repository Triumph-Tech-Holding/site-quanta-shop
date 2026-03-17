using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MMN.Api.Controllers.v1;
using MMN.Api.Models.Response;
using MMN.Api.Services;
using MMN.Integracoes.Zanox;
using System;
using System.Threading.Tasks;

namespace MMN.Api.Controllers.v2;

[Route("api/v2/admin/reports"), ApiController, Authorize(Roles = "Admin")]
public class ReportsController : LoggedControllerBase
{
    private readonly IReportsService _reportsService;

    public ReportsController(IReportsService reportsService)
    {
        _reportsService = reportsService;
    }

    [HttpGet("get-sales-waiting-for-approval")]
    public async Task<IActionResult> GetSalesWaitingForApproval([FromQuery] string seller, [FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 50)
    {
        try
        {
            var result = await _reportsService.GetSalesWaitingForApproval(seller, startDate, endDate, pageNumber, pageSize);
            return Ok(new ApiResponse<object>().SuccessResponse(result, "Consulta de vendas aguardando aprovação realizada com sucesso."));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<object>().ErrorResponse($"Erro ao buscar vendas aguardando aprovação: {ex.Message}"));
        }
    }

    [HttpGet("get-invoices-awaiting-payment")]
    public async Task<IActionResult> GetInvoicesAwaitingPayment([FromQuery] string seller, [FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 50)
    {
        try
        {
            var result = await _reportsService.GetInvoicesAwaitingPayment(seller, startDate, endDate);
            return Ok(new ApiResponse<object>().SuccessResponse(result, "Consulta de faturas aguardando pagamento realizada com sucesso."));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<object>().ErrorResponse($"Erro ao buscar faturas aguardando pagamento: {ex.Message}"));
        }
    }

    [HttpGet("get-installments-awaiting-payment")]
    public async Task<IActionResult> GetInstallmentsAwaitingPayment([FromQuery] string login, [FromQuery] string name, [FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 50)
    {
        try
        {
            var result = await _reportsService.GetInstallmentsAwaitingPayment(login, name, startDate, endDate, pageNumber, pageSize);
            return Ok(new ApiResponse<object>().SuccessResponse(result, "Consulta de parcelas aguardando pagamento realizada com sucesso."));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<object>().ErrorResponse($"Erro ao buscar parcelas aguardando pagamento: {ex.Message}"));
        }
    }

    [HttpGet("get-installments-by-order/{orderId}")]
    public async Task<IActionResult> GetInstallmentsByOrder(int orderId)
    {
        try
        {
            var result = await _reportsService.GetInstallmentsByOrder(orderId);
            return Ok(new ApiResponse<object>().SuccessResponse(result, "Consulta de parcelas realizada com sucesso."));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<object>().ErrorResponse($"Erro ao buscar parcelas : {ex.Message}"));
        }
    }
}
