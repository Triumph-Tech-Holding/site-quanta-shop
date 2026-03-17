using AutoMapper;
using Flurl.Http;
using MMN.Dominio.Enum;
using MMN.Dominio.Excecao;
using MMN.Dominio.Model;
using MMN.Dominio.ViewModel;
using MMN.INegocio.Negocio;
using MMN.IRepositorio.Repositorio;
using MMN.Negocio.Base;
using MMN.Util;
using MMN.Util.Enum;
using MMN.Util.Extensions;
using MMN.Util.Model.LionBit;
using MMN.Util.Models.Request.Asaas;
using MMN.Util.Models.Request.Asaas.Customer;
using MMN.Util.Models.Response.Asaas;
using MMN.Util.Models.Response.Asaas.Customer;
using MMN.Util.Models.Response.Asaas.Payment;
using MMN.Util.Services;
using MMN.Util.Util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.ConstrainedExecution;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MMN.Negocio.Negocio
{
	public class PagamentoNegocio : BaseNegocio<PagamentoViewModel, Pagamento>, IPagamentoNegocio
	{
		private readonly IPagamentoRepositorio _repositorio;
		private readonly IUsuarioEnderecoNegocio _enderecoNegocio;
		private readonly IUsuarioNegocio _usuarioNegocio;
		private readonly IPedidoRepositorio _pedidoRepositorio;
		private readonly IMapper _mapper;
		private readonly IUsuarioProdutoNegocio _usuarioProdutoNegocio;

		public PagamentoNegocio(
			IPagamentoRepositorio repositorio,
			IUsuarioNegocio usuarioNegocio,
			IUsuarioEnderecoNegocio enderecoNegocio,
			IPedidoRepositorio pedidoRepositorio,
			IMapper mapper, IUsuarioProdutoNegocio usuarioProdutoNegocio) : base(repositorio, mapper)

		{
			_repositorio = repositorio;
			_enderecoNegocio = enderecoNegocio;
			_usuarioNegocio = usuarioNegocio;
			_pedidoRepositorio = pedidoRepositorio;
			_mapper = mapper;
			_usuarioProdutoNegocio = usuarioProdutoNegocio;
		}

		public IEnumerable<Pagamento> ObterPagamentos(long IdPedido)
		{
			return _repositorio.GetNoTracking(p => p.IdPedido == IdPedido);
		}

		public void InativarPagamento(Pagamento pagamento)
		{
			pagamento.Ativo = false;
			pagamento.Status = (int)StatusPagamento.Cancelado;
			_repositorio.Update(pagamento);
		}

		public Pagamento ObterParcela(int IdPedido, int NumeroParcela)
		{
			return _repositorio.First(p => p.IdPedido == IdPedido && p.NumeroParcela == NumeroParcela);
		}

		public Pagamento PagarParcela(int IdPedido, int NumeroParcela)
		{
			var parcela = _repositorio.First(p => p.IdPedido == IdPedido && p.NumeroParcela == NumeroParcela);

			if (parcela == null)
			{
				throw new KeyNotFoundException();
			}

			parcela.DataPagamento = DateTime.UtcNow.HorarioBrasilia();
			parcela.Pago = true;

			_repositorio.Update(parcela);
			_repositorio.SaveChanges();

			return parcela;
		}

		public Pagamento PagarParcela(int IdParcela)
		{
			var parcela = _repositorio.First(p => p.IdPagamento == IdParcela);

			if (parcela == null)
			{
				throw new KeyNotFoundException();
			}

			parcela.DataPagamento = DateTime.UtcNow.HorarioBrasilia();
			parcela.Pago = true;
			parcela.Status = (int)StatusPagamento.PagoProcessado;

			_repositorio.Update(parcela);
			_repositorio.SaveChanges();

			return parcela;
		}

		public Pagamento ObterProximaParcela(int IdPedido)
		{
			return _repositorio.Get(p => p.IdPedido == IdPedido && !p.Pago).OrderBy(p => p.DataValidade).First();
		}

		public async Task<IEnumerable<Pagamento>> CriarParcelasAsync(
			IEnumerable<Pagamento> parcelas,
			Pedido pedido,
			bool gerarBoleto = true)
		{
			var usuario = _usuarioNegocio.First(x => x.IdUsuario == pedido.IdUsuario);
			var endereco = _enderecoNegocio.First(x => x.IdUsuario == pedido.IdUsuario, "Cidade", "Cidade.Estado");

			if (endereco == null ||
				endereco.Cidade == null ||
				endereco.Cidade.Estado == null)
			{
				throw new PadraoException("usuario_sem_endereco");
			}

			var pagamentosCriados = new List<Pagamento>();

			foreach (var parcela in parcelas)
			{
				if (parcela.Pago)
				{
					pagamentosCriados.Add(parcela);
					_repositorio.Insert(parcela);
				}
				else
				{
					pagamentosCriados.Add(await CriarPagamentoAsync(parcela, pedido, gerarBoleto));
				}
			}
			_repositorio.SaveChanges();

			return pagamentosCriados;
		}

		//public async Task<IEnumerable<Pagamento>> CriarParcelaAssinaturaAsync(
		//    Pedido pedido, CartaoViewModel card)
		//{
		//    var usuario = _usuarioNegocio.First(x => x.IdUsuario == pedido.IdUsuario);
		//    var endereco = _enderecoNegocio.First(x => x.IdUsuario == pedido.IdUsuario, "Cidade", "Cidade.Estado");

		//    if (endereco == null ||
		//        endereco.Cidade == null ||
		//        endereco.Cidade.Estado == null)
		//    {
		//        throw new PadraoException("usuario_sem_endereco");
		//    }

		//    Pagamento pagamentosCriados = new Pagamento();

		//   var result = await CriarPagamentoAssinaturaAsync( pedido, card);

		//    if (result.Success)
		//    {
		//        pedido.CodigoReferenciaAssinatura = result.Data;
		//    }

		//    return pagamentosCriados;
		//}

		private async Task<Pagamento> CriarPagamentoAsync(
			Pagamento pagamento,
			Pedido pedido,
			bool gerarBoleto = true)
		{
			if (pedido.MeioPagamento == (int)EnumTipoPagamento.PGPAGARME || pedido.MeioPagamento == (int)EnumTipoPagamento.PGASAAS)
			{
				var viewModel = _mapper.Map<PagamentoViewModel>(pagamento);
				if (gerarBoleto)
				{
					viewModel = await CriarBoletoAsync(viewModel, pedido);

					pagamento.CodigoReferenciaBoleto = viewModel.CodigoReferenciaBoleto;
					pagamento.UrlBoleto = viewModel.UrlBoleto;
					pagamento.LinhaDigitavelBoleto = viewModel.LinhaDigitavelBoleto;
				}
			}
			pagamento.Status = (int)StatusPagamento.AguardandoPagamento;

			_repositorio.Insert(pagamento);

			return pagamento;
		}

		public async Task<PagamentoViewModel> CriarBoletoAsync(
			long idPedido,
			int numeroParcela,
			Guid IdUsuario,
			string observacao = "Compra de plano Quanta Shop")
		{

			var pedido = _pedidoRepositorio.FirstNoTracking(g => g.IdPedido == idPedido && g.IdUsuario == IdUsuario, "Pagamentos");

			if (pedido == null)
			{
				throw new NotFoundException("pedido_nao_encontrado");
			}
			if (pedido.Ativo != true)
			{
				throw new NotFoundException("pedido_inativo");
			}

			var pagamento = _mapper.Map<PagamentoViewModel>(
				pedido.Pagamentos.SingleOrDefault(s => s.Ativo && s.NumeroParcela == numeroParcela));

			if (pagamento == null)
			{
				throw new NotFoundException("pagamento_nao_encontrado");
			}

			if (pagamento.CodigoReferenciaBoleto != null)
			{
				throw new PadraoException("pagamento_possui_boleto");
			}

			if (pedido.MeioPagamento != (int)EnumTipoPagamento.PGPAGARME)
			{
				throw new PadraoException("meio_pagamento_invalido");
			}

			await CriarBoletoAsync(pagamento, pedido, observacao);

			Update(pagamento);
			SaveChanges();

			return pagamento;
		}

		public async Task<PagamentoViewModel> CriarBoletoAsync(PagamentoViewModel pagamento, Pedido pedido, string observacao = "Compra de plano Quanta Shop")
		{
			try
			{
				UsuarioEnderecoViewModel endereco;
				string documento;

				var userTipoPlano = _usuarioProdutoNegocio.FirstNoTracking(x => x.IdUsuario == pedido.IdUsuario && x.Ativo, "Produto");
				var tipo = userTipoPlano.Produto.Nome;

				var usuario = _usuarioNegocio
					.First(x => x.IdUsuario == pedido.IdUsuario,
					"Credenciamento.Cidade.Estado");

				if (pedido.Tipo == (int)TipoPedido.FaturaCashbackCredenciado &&
					usuario.Credenciamento != null)
				{
					endereco = new UsuarioEnderecoViewModel
					{
						Bairro = usuario.Credenciamento.Bairro,
						Cep = usuario.Credenciamento.Cep,
						Complemento = usuario.Credenciamento.Complemento,
						IdCidade = usuario.Credenciamento.IdCidade,
						Cidade = usuario.Credenciamento.CidadeViewModel,
						Numero = usuario.Credenciamento.Numero,
						Rua = usuario.Credenciamento.Rua,
						Usuario = usuario
					};
					documento = usuario.Credenciamento.Cnpj;
				}
				else
				{
					endereco = _enderecoNegocio.First(x => x.IdUsuario == pedido.IdUsuario, "Cidade", "Cidade.Estado");
					documento = usuario.Documento;
				}

				if (endereco == null ||
					endereco.Cidade == null ||
					endereco.Cidade.Estado == null)
				{
					throw new PadraoException("usuario_sem_endereco");
				}

				// Gera boleto

				if (!UtilBase.IsValidCpfCnpj(usuario.Documento))
				{
					throw new PadraoException("cpf_cnpj_invalido");
				}

				#region Pagar.me
				if (pedido.MeioPagamento == (int)EnumTipoPagamento.PGPAGARME)
				{
					var response = await Pagarme.GerarBoletoAsync(
									//observacao: "Compra de plano BIGCASH",
									observacao: "Compra de plano " + tipo,
									valor: pagamento.Valor,
									email: usuario.Email,
									documento: usuario.Documento,
									nome: usuario.Nome,
									ddd: UtilBase.FiltrarDigitos(usuario.Celular).Substring(0, 2),
									telefone: UtilBase.FiltrarDigitos(usuario.Celular).Substring(2),
									logradouro: endereco.Rua,
									numero: endereco.Numero,
									complemento: endereco.Complemento,
									bairro: endereco.Bairro,
									cidade: endereco.Cidade.Estado.Nome,
									uf: endereco.Cidade.Estado.Uf,
									cep: endereco.Cep,
									identificador: pedido.Codigo,
									parcela: pagamento.NumeroParcela,
									adicionarTaxa: pedido.Tipo == (int)TipoPedido.Baf);

					pagamento.Status = (int)StatusPagamento.AguardandoPagamento;
					pagamento.CodigoReferenciaBoleto = response.Id;
					pagamento.UrlBoleto = response.BoletoUrl + "?format=pdf";
					pagamento.LinhaDigitavelBoleto = response.BoletoBarcode;
				}
				#endregion Pagar.me

				#region Asaas
				if (pedido.MeioPagamento == (int)EnumTipoPagamento.PGASAAS)
				{
					AsaasService asaasService = new();
					CustomerListResponse customerListResponse = await asaasService.GetCustomerByQueryAsync(email: usuario.Email, cpfCnpj: UtilBase.FiltrarDigitos(documento));
					CustomerResponse customerResponse = null;

					if (customerListResponse.Data.Any())
						customerResponse = customerListResponse.Data.FirstOrDefault();
					else
					{
						CustomerRequest customerRequest = new()
						{
							Name = usuario.Nome,
							Email = usuario.Email,
							Document = UtilBase.FiltrarDigitos(documento),
							Phone = UtilBase.FiltrarDigitos(usuario.Celular),
							MobilePhone = UtilBase.FiltrarDigitos(usuario.Celular),
							PostalCode = endereco.Cep.Replace("-", ""),
							AddressNumber = endereco.Numero,
							Complement = endereco.Complemento
						};

						customerResponse = await asaasService.CreateCustomerAsync(customerRequest);
					}

					NewPaymentRequest newPaymentRequest = new(PaymentType.BOLETO)
					{
						Customer = customerResponse.Id,
						Value = Convert.ToDouble(pagamento.Valor),
						DueDate = pagamento.DataValidade,
						ExternalReference = pedido.Codigo
					};

					PaymentResponse paymentResponse = await asaasService.CreatePaymentAsync(newPaymentRequest);

					pagamento.Status = (int)StatusPagamento.AguardandoPagamento;
					pagamento.CodigoReferenciaBoleto = paymentResponse.Id;
					pagamento.UrlBoleto = paymentResponse.InvoiceUrl;
				}
				#endregion Asaas

				return pagamento;
			}
			catch (Exception ex)
			{
				var message = ex.InnerException is null ? ex.Message : ex.InnerException.ToString();
				return null;
			}
		}


		public async Task<DadosRetorno<string>> CancelarAssinatura(Pedido pedido)
		{
			DadosRetorno<string> retorno = new DadosRetorno<string>();

			var response = Pagarme.CancelarAssinatura(pedido.CodigoReferenciaAssinatura);

			if (response.Result.Success)
			{
				retorno.Success = true;
			}
			else
			{
				retorno.Success = false;
				retorno.Message = response.Result.Message;
			}

			return retorno;
		}
		public async Task<DadosRetorno<string>> CriarAssinaturaAsync(Pedido pedido, CartaoViewModel card, string observacao = "Assinatura de plano Quanta Plus")
		{
			var userTipoPlano = _usuarioProdutoNegocio.Get(x => x.IdUsuario == pedido.IdUsuario && x.Ativo, new[] { "Produto" }).FirstOrDefault();

			var usuario = _usuarioNegocio
				.First(x => x.IdUsuario == pedido.IdUsuario,
				"Credenciamento.Cidade.Estado");

			var usuarioEndereco = _enderecoNegocio.FirstNoTracking(x => x.IdUsuario == usuario.IdUsuario, ["Cidade", "Cidade.Estado"]);

			DadosRetorno<string> retornoAssinatura = new DadosRetorno<string>();

			try
			{
				var document_type = usuario.Documento.Length == 11 ? "CPF" : "CNPJ";
				var type = usuario.Documento.Length == 11 ? "individual" : "company";
				var document = usuario.Documento;
				var countryCode = "55";
				var areaCode = usuario.Celular[..2];
				var mobilePhone = usuario.Celular[2..];

				var endereco = new
				{
					Estado = usuarioEndereco.Cidade.Estado.Uf,
					Cidade = usuarioEndereco.Cidade.Nome,
					Bairro = usuarioEndereco.Bairro,
					Rua = usuarioEndereco.Rua,
					Numero = usuarioEndereco.Numero,
					Complemento = usuarioEndereco.Complemento,
					CEP = usuarioEndereco.Cep.Replace("-", "")
				};

				var retorno = Pagarme.CriarAssinatura(usuario.Email, usuario.Nome, card.Card_number, card.Card_holder_name, card.Card_expiration_month.ToString(), card.Card_expiration_year, card.Card_cvv, pedido.IdUsuario, document_type, document, type, countryCode, areaCode, mobilePhone, endereco);

				if (retorno.Result.Success)
				{
					retornoAssinatura.Success = true;
					retornoAssinatura.Data = retorno.Result.Data;
					retornoAssinatura.Response = retorno.Result.Response;
				}
				else
				{
					retornoAssinatura.Success = false;
					retornoAssinatura.Message = "erro :" + retorno.Result.Data;
				}
			}
			catch (Exception ex)
			{
				retornoAssinatura.Success = false;
				var error = ex;
			}

			return retornoAssinatura;
		}

		public async Task ReativarPagamentoAsync(Pagamento pagamento, Pedido pedido)
		{
			UsuarioEnderecoViewModel endereco;
			string documento;

			var usuario = _usuarioNegocio
				.First(x => x.IdUsuario == pedido.IdUsuario, "Credenciamento");

			if (pedido.Tipo == (int)TipoPedido.FaturaCashbackCredenciado &&
				usuario.Credenciamento != null)
			{
				endereco = new UsuarioEnderecoViewModel
				{
					Bairro = usuario.Credenciamento.Bairro,
					Cep = usuario.Credenciamento.Cep,
					Complemento = usuario.Credenciamento.Complemento,
					IdCidade = usuario.Credenciamento.IdCidade,
					Numero = usuario.Credenciamento.Numero,
					Rua = usuario.Credenciamento.Rua,
					Usuario = usuario
				};
				documento = usuario.Credenciamento.Cnpj;
			}
			else
			{
				endereco = _enderecoNegocio.First(x => x.IdUsuario == pedido.IdUsuario, "Cidade", "Cidade.Estado");
				documento = usuario.Documento;
			}

			if (endereco == null ||
				endereco.Cidade == null ||
				endereco.Cidade.Estado == null)
			{
				throw new PadraoException("usuario_sem_endereco");
			}

			if (!UtilBase.IsValidCpfCnpj(usuario.Documento))
			{
				throw new PadraoException("cpf_cnpj_invalido");
			}

			if (!pagamento.Pago)
			{
				// Gera um novo boleto
				var response = await Pagarme.GerarBoletoAsync(
					observacao: "Compra de plano BIGCASH",
					valor: pagamento.Valor,
					email: usuario.Email,
					documento: usuario.Documento,
					nome: usuario.Nome,
					ddd: usuario.Celular.Replace("(", "").Replace(")", "").Replace(" ", "").Replace("-", "").Substring(0, 2),
					telefone: usuario.Celular.Replace("(", "").Replace(")", "").Replace(" ", "").Replace("-", "").Substring(2),
					logradouro: endereco.Rua,
					numero: endereco.Numero,
					complemento: endereco.Complemento,
					bairro: endereco.Bairro,
					cidade: endereco.Cidade.Estado.Nome,
					uf: endereco.Cidade.Estado.Uf,
					cep: endereco.Cep,
					identificador: pedido.Codigo,
					parcela: pagamento.NumeroParcela,
					adicionarTaxa: pedido.Tipo == (int)TipoPedido.Baf);

				pagamento.CodigoReferenciaBoleto = response.Id;
				pagamento.UrlBoleto = response.BoletoUrl;
				pagamento.LinhaDigitavelBoleto = response.BoletoBarcode;
			}

			pagamento.Ativo = true;
			_repositorio.Update(pagamento);
		}

		public IEnumerable<Pagamento> ObterPagamentosUsuario(Guid IdUsuario)
		{
			throw new NotImplementedException();
		}
	}
}
