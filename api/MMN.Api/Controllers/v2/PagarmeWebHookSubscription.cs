using Microsoft.AspNetCore.Mvc;
using MMN.Api.Models.Response;
using MMN.Api.Services;
using MMN.Dominio.Model;
using MMN.Dominio.WebHook;
using MMN.INegocio.Negocio;
using MMN.IRepositorio.Repositorio;
using MMN.Util.Enum;
using MMN.Util.Extensions;
using System;
using System.Linq;

namespace MMN.Api.Controllers.v2;
[Route("api/v2/[controller]")]
[ApiController]
public class PagarmeWebHookSubscription : ControllerBase
{
	readonly IPedidoNegocio _pedidoNegocio;
	readonly IPedidoRepositorio _pedidoRepositorio;
	readonly IPedidoDetalheRepositorio _pedidoDetalheRepositorio;
	readonly ITransacaoRepositorio _transacaoRepositorio;
	readonly IUsuarioRepositorio _usuarioRepositorio;
	readonly IUsuarioProdutoNegocio _usuarioProdutoNegocio;
	readonly ISubscriptionsService _subscriptionService;

	public PagarmeWebHookSubscription(IPedidoNegocio pedidoNegocio, IPedidoRepositorio pedidoRepositorio, IPedidoDetalheRepositorio pedidoDetalheRepositorio, ITransacaoRepositorio transacaoRepositorio, IUsuarioRepositorio usuarioRepositorio, IUsuarioProdutoNegocio usuarioProdutoNegocio, ISubscriptionsService subscriptionService)
	{
		_pedidoNegocio = pedidoNegocio;
		_pedidoRepositorio = pedidoRepositorio;
		_pedidoDetalheRepositorio = pedidoDetalheRepositorio;
		_transacaoRepositorio = transacaoRepositorio;
		_usuarioRepositorio = usuarioRepositorio;
		_usuarioProdutoNegocio = usuarioProdutoNegocio;
		_subscriptionService = subscriptionService;
	}

	[HttpPost]
	public IActionResult WebHookSubscription(WebHookSubscription model)
	{
		try
		{
			if (model != null)
			{
				if (model.data.invoice == null)
					throw new Exception("Invoice not found");

				var subscriptionId = model.data.invoice.subscriptionId;

				if (model.type == "subscription.canceled" || model.type == "charge.payment_failed" || model.type == "invoice.payment_failed")
				{
					var pedido = _pedidoNegocio.FirstNoTracking(x => x.CodigoReferenciaAssinatura == subscriptionId);

					if (pedido != null)
					{
						var idUsuario = pedido.IdUsuario;

						_pedidoNegocio.CancelarAssinatura(idUsuario, "pagar.me");

						//var pedidoDetalhe = new PedidoDetalhe
						//{
						//    IdPedido = pedido.IdPedido,
						//    DataAtualizacao = DateTime.UtcNow.HorarioBrasilia(),
						//    Ativo = true,
						//    Descricao = $"Assinatura do plano cancelada: " + string.Join(", ", model.data.last_transaction.gateway_response.errors.Select(x => x.message)),
						//    IdStatus = (int)StatusTransacaoEnum.Aprovado,
						//    IdUsuario = pedido.IdUsuario,
						//    DataAssinatura = pedido.DataPedido,
						//};

						//_pedidoDetalheRepositorio.Insert(pedidoDetalhe);
						//_pedidoDetalheRepositorio.SaveChanges();
					}

				}

				if (model.type == "charge.paid")
				{
					if (model.data.recurrence_cycle == "first")
					{
						if (!_pedidoNegocio.Get(x => x.CodigoReferenciaAssinatura == subscriptionId).Any())
						{
							var usuario = _usuarioRepositorio.FirstNoTracking(x => x.Documento == model.data.customer.document);

							// Transação
							var transacao = new Transacao();
							transacao.IdUsuario = usuario.IdUsuario;
							transacao.IdTipo = 57;
							transacao.ValorPrincipal = model.data.amount / 100m;
							transacao.DataTransacao = model.created_at;
							transacao.Descricao = "Assinatura mensal [Vision Plus]";
							transacao.IdStatus = 2;
							transacao.Ativo = true;

							_transacaoRepositorio.Insert(transacao);
							_transacaoRepositorio.SaveChanges();

							// Pedido
							var pedido = new Pedido();
							pedido.IdUsuario = usuario.IdUsuario;
							pedido.IdTransacao = transacao.IdTransacao;
							pedido.DataPedido = model.data.created_at;
							pedido.Codigo = $"PED{DateTime.UtcNow.HorarioBrasilia().ToString("yyMMddhhmmss")}_RN{IntExtensions.GetRandom(1000)}";
							pedido.ValorTaxa = 0;
							pedido.ValorPedido = model.data.amount / 100m;
							pedido.DataPagamento = model.data.created_at;
							pedido.Pago = true;
							pedido.Ativo = true;
							pedido.MeioPagamento = 14;
							pedido.Quantidade = 0;
							pedido.Cotacao = 0;
							pedido.NumeroParcelas = 0;
							pedido.ValorProduto = model.data.amount / 100m;
							pedido.Cancelado = false;
							pedido.ContabilizarPontuacao = false;
							pedido.GeradoManualmente = false;
							pedido.Status = 2;
							pedido.Tipo = 57;
							pedido.CodigoReferenciaAssinatura = model.data.invoice.subscriptionId;

							_pedidoRepositorio.Insert(pedido);
							_pedidoRepositorio.SaveChanges();

							DateTime assinaturaAte = pedido.DataPedido.AddMonths(1).AddDays(-1);
							DateTime ultimoDiaDoProximoMes = assinaturaAte.AddDays(-1);

							// PedidoDetalhe
							var pedidoDetalhe = new PedidoDetalhe();
							pedidoDetalhe.Descricao = "Assinatura do plano realizada";
							pedidoDetalhe.DataAtualizacao = model.data.created_at;
							pedidoDetalhe.Ativo = true;
							pedidoDetalhe.IdPedido = pedido.IdPedido;
							pedidoDetalhe.IdStatus = 6;
							pedidoDetalhe.IdUsuario = usuario.IdUsuario;
							pedidoDetalhe.DataAssinatura = pedido.DataPedido;
							pedidoDetalhe.AssinaturaDe = new DateTime(pedido.DataPedido.Year, pedido.DataPedido.Month, pedido.DataPedido.Day, 0, 0, 0);
							pedidoDetalhe.AssinaturaAte = new DateTime(assinaturaAte.Year, assinaturaAte.Month, assinaturaAte.Day, 23, 59, 59);
							pedidoDetalhe.AssinaturaProximaCobranca = new DateTime(pedido.DataPedido.Year, pedido.DataPedido.Month, pedido.DataPedido.Day, 0, 0, 0).AddMonths(1);
							pedidoDetalhe.CodigoReferenciaFatura = model.data.invoice.id;

							_pedidoDetalheRepositorio.Insert(pedidoDetalhe);
							_pedidoDetalheRepositorio.SaveChanges();

							// UsuarioProduto
							var usuarioProduto = _usuarioProdutoNegocio.FirstNoTracking(x => x.IdUsuario == usuario.IdUsuario && x.Ativo);

							usuarioProduto.DataAssinatura = pedido.DataPedido;
							usuarioProduto.AssinaturaDe = new DateTime(pedido.DataPedido.Year, pedido.DataPedido.Month, pedido.DataPedido.Day, 0, 0, 0);
							usuarioProduto.AssinaturaAte = new DateTime(assinaturaAte.Year, assinaturaAte.Month, assinaturaAte.Day, 23, 59, 59);
							usuarioProduto.AssinaturaProximaCobranca = new DateTime(pedido.DataPedido.Year, pedido.DataPedido.Month, pedido.DataPedido.Day, 0, 0, 0).AddMonths(1);
							usuarioProduto.AssinaturaHabilitada = true;

							_usuarioProdutoNegocio.Update(usuarioProduto);
							_usuarioProdutoNegocio.SaveChanges();

							_subscriptionService.ExecuteDistribution(usuarioProduto.IdUsuarioProduto, null);
						}
					}

					if (model.data.recurrence_cycle == "subsequent")
					{
						if (!_pedidoDetalheRepositorio.Get(x => x.CodigoReferenciaFatura == model.data.invoice.id).Any())
						{
							var pedido = _pedidoNegocio.FirstNoTracking(x => x.CodigoReferenciaAssinatura == subscriptionId);

							var usuarioProduto = _usuarioProdutoNegocio.FirstNoTracking(x => x.IdUsuario == pedido.IdUsuario && x.Ativo);
							usuarioProduto.AssinaturaDe = usuarioProduto.AssinaturaDe is not null ? usuarioProduto.AssinaturaDe.Value.AddMonths(1) : null;
							usuarioProduto.AssinaturaAte = usuarioProduto.AssinaturaAte is not null ? usuarioProduto.AssinaturaAte.Value.AddMonths(1) : null;
							usuarioProduto.AssinaturaProximaCobranca = usuarioProduto.AssinaturaProximaCobranca is not null ? usuarioProduto.AssinaturaProximaCobranca.Value.AddMonths(1) : null;

							_usuarioProdutoNegocio.Update(usuarioProduto);
							_usuarioProdutoNegocio.SaveChanges();

							var pedidoDetalhe = new PedidoDetalhe
							{
								IdPedido = pedido.IdPedido,
								DataAtualizacao = DateTime.UtcNow.HorarioBrasilia(),
								Ativo = true,
								Descricao = $"Assinatura do plano renovada",
								IdStatus = (int)StatusTransacaoEnum.Aprovado,
								IdUsuario = pedido.IdUsuario,
								DataAssinatura = pedido.DataPedido,
								AssinaturaDe = usuarioProduto.AssinaturaDe,
								AssinaturaAte = usuarioProduto.AssinaturaAte,
								AssinaturaProximaCobranca = usuarioProduto.AssinaturaProximaCobranca,
								CodigoReferenciaFatura = model.data.invoice.id
							};

							_pedidoDetalheRepositorio.Insert(pedidoDetalhe);
							_pedidoDetalheRepositorio.SaveChanges();

							_subscriptionService.ExecuteDistribution(usuarioProduto.IdUsuarioProduto, null);
						}
					}
				}
			}
			else
			{
				throw new Exception("Houve um problema com os dados enviados");
			}

			return Ok(new ApiResponse<object>().SuccessResponse(null, "WebHook processado com sucesso."));
		}
		catch (Exception ex)
		{
			return StatusCode(500, new ApiResponse<object>().ErrorResponse($"Erro ao processar o WebHook: {ex.Message}"));
		}
	}
}