using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using MMN.Dominio.Model;
using MMN.Repositorio.Contexto;
using MMN.Util.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMN.Api.Services;

public interface ISubscriptionsService
{
	IEnumerable<object> GetSubscriptionsHistory(Guid userId);
	object CreateNewSubscription(Guid userId, DateTime? startDate, DateTime? endDate, bool performLaunch = false, bool manual = false);
	bool IsSubscriber(string login);
	void ExecuteDistribution(int usuarioProdutoId, IDbTransaction transaction);
	Task<IEnumerable<LatestSubscriptionDto>> GetLatestSubscriptionsSummary(string login, string name, bool? active, int pageNumber, int pageSize);
	Task LaunchNewCycleAsync(Guid userId, LaunchCycleDto dto);
	Task<IEnumerable<SubscriptionCycleDto>> GetSubscriptionCyclesAsync(Guid userId);
	Task DeleteSubscriptionCycleAsync(long pedidoDetalheId);
	Task UpdateVirtualAssistantLinkAsync(Guid userId, string link);
}

public class SubscriptionsService : ISubscriptionsService
{
	private readonly IServiceProvider _serviceProvider;
	private readonly IDbConnection _dbConnection;

	public SubscriptionsService(IDbConnection dbConnection, IServiceProvider serviceProvider)
	{
		_dbConnection = dbConnection;
		_serviceProvider = serviceProvider;
	}

	public object CreateNewSubscription(Guid userId, DateTime? startDate, DateTime? endDate, bool performLaunch = false, bool manual = false)
	{
		_dbConnection.Open();

		using (var transaction = _dbConnection.BeginTransaction())
		{
			try
			{
				#region Verificar se já existe uma assinatura habilitada
				var sqlVerificaAssinatura = @"
                SELECT TOP 1 1
                FROM UsuarioProduto
                WHERE IdUsuario = @IdUsuario 
                  AND Ativo = 1 
                  AND AssinaturaHabilitada = 1";

				var assinaturaExistente = _dbConnection.QueryFirstOrDefault<int?>(
					sqlVerificaAssinatura,
					new { IdUsuario = userId },
					transaction
				);

				if (assinaturaExistente.HasValue)
					throw new Exception("Usuário já possui uma assinatura ativa.");
				#endregion Verificar se já existe uma assinatura habilitada

				var hoje = DateTime.Today;
				var agora = DateTime.Now;

				#region Inserir Transacao
				var sqlTransacao = @"
                        INSERT INTO Transacao (
                            IdUsuario, IdTipo, ValorPrincipal, DataTransacao, Descricao, IdStatus,
                            Ativo, IdVendaZanox, ComissaoTotal, IdAnunciante, IdVendaAfilio,
                            IdVendaAwin, DataReferencia
                        ) VALUES (
                            @IdUsuario, @IdTipo, @ValorPrincipal, @DataTransacao, @Descricao, @IdStatus,
                            @Ativo, @IdVendaZanox, @ComissaoTotal, @IdAnunciante, @IdVendaAfilio,
                            @IdVendaAwin, @DataReferencia
                        );
                        SELECT CAST(SCOPE_IDENTITY() as int)";

				var transacao = new Transacao
				{
					IdUsuario = userId,
					IdTipo = 57,
					ValorPrincipal = 99.90m,
					DataTransacao = agora,
					Descricao = "Assinatura mensal [Vision Plus]",
					IdStatus = 1,
					Ativo = true,
					IdVendaZanox = null,
					ComissaoTotal = null,
					IdAnunciante = null,
					IdVendaAfilio = null,
					IdVendaAwin = null,
					DataReferencia = agora
				};

				var transacaoId = _dbConnection.QuerySingle<int>(sqlTransacao, transacao, transaction);
				#endregion Inserir Transacao

				#region Inserir Pedido
				var sqlPedido = @"
                        INSERT INTO Pedido (
                            IdUsuario, IdTransacao, DataPedido, Codigo, ValorTaxa, ValorPedido,
                            ValorPago, DataPagamento, Pago, Ativo, EnderecoDeposito, MeioPagamento,
                            Quantidade, UrlPagamento, Cotacao, CodigoReferenciaBoleto,
                            LinhaDigitavelBoleto, UrlBoleto, IdVendaZanox, IdVendaAfilio, Cashback,
                            IdAwinTransaction, IdUsuarioComerciante, PercentualCashback,
                            NumeroParcelas, ValorProduto, Cancelado, ContabilizarPontuacao,
                            GeradoManualmente, ReaisPorPonto, DataReferencia, Status, Tipo,
                            IdAnunciante, CodigoReferenciaAssinatura
                        ) VALUES (
                            @IdUsuario, @IdTransacao, @DataPedido, @Codigo, @ValorTaxa, @ValorPedido,
                            @ValorPago, @DataPagamento, @Pago, @Ativo, @EnderecoDeposito, @MeioPagamento,
                            @Quantidade, @UrlPagamento, @Cotacao, @CodigoReferenciaBoleto,
                            @LinhaDigitavelBoleto, @UrlBoleto, @IdVendaZanox, @IdVendaAfilio, @Cashback,
                            @IdAwinTransaction, @IdUsuarioComerciante, @PercentualCashback,
                            @NumeroParcelas, @ValorProduto, @Cancelado, @ContabilizarPontuacao,
                            @GeradoManualmente, @ReaisPorPonto, @DataReferencia, @Status, @Tipo,
                            @IdAnunciante, @CodigoReferenciaAssinatura
                        );
                        SELECT CAST(SCOPE_IDENTITY() as int)";

				var pedido = new Pedido
				{
					IdUsuario = userId,
					IdTransacao = transacaoId,
					DataPedido = agora,
					ValorTaxa = 0m,
					ValorPedido = 99.9m,
					ValorPago = null,
					DataPagamento = agora,
					Pago = true,
					Ativo = true,
					EnderecoDeposito = null,
					MeioPagamento = 12,
					Quantidade = 1,
					UrlPagamento = null,
					Cotacao = 0m,
					CodigoReferenciaBoleto = null,
					LinhaDigitavelBoleto = null,
					UrlBoleto = null,
					IdVendaZanox = null,
					IdVendaAfilio = null,
					Cashback = null,
					IdAwinTransaction = null,
					IdUsuarioComerciante = null,
					PercentualCashback = null,
					NumeroParcelas = 0,
					ValorProduto = 99.9m,
					Cancelado = false,
					ContabilizarPontuacao = false,
					GeradoManualmente = true,
					ReaisPorPonto = null,
					DataReferencia = null,
					Status = 2,
					Tipo = 57,
					IdAnunciante = null,
				};

				pedido.Codigo = StringExtensions.GenerateOrderCode();
				pedido.CodigoReferenciaAssinatura = StringExtensions.GenerateSubscriptionCode(pedido.Codigo);

				var pedidoId = _dbConnection.QuerySingle<int>(sqlPedido, pedido, transaction);
				#endregion Inserir Pedido

				#region Inserir PedidoDetalhe
				var sqlPedidoDetalhe = @"
                        INSERT INTO PedidoDetalhe (
                            Descricao, DataAtualizacao, Ativo, IdPedido, IdStatus, IdUsuario,
                            AssinaturaAte, AssinaturaDe, AssinaturaProximaCobranca, DataAssinatura,
                            CodigoReferenciaFatura
                        ) VALUES (
                            @Descricao, @DataAtualizacao, @Ativo, @IdPedido, @IdStatus, @IdUsuario,
                            @AssinaturaAte, @AssinaturaDe, @AssinaturaProximaCobranca, @DataAssinatura,
                            @CodigoReferenciaFatura
                        )";

				var pedidoDetalhe = new
				{
					Descricao = "Assinatura do plano realizada",
					DataAtualizacao = agora,
					Ativo = true,
					IdPedido = pedidoId,
					IdStatus = 2,
					IdUsuario = userId,
					AssinaturaAte = endDate ?? hoje.AddDays(29).AddHours(23).AddMinutes(59).AddSeconds(59),
					AssinaturaDe = startDate ?? hoje,
					AssinaturaProximaCobranca = endDate?.AddDays(1) ?? hoje.AddDays(30),
					DataAssinatura = agora,
					CodigoReferenciaFatura = StringExtensions.GenerateInvoiceCode(pedido.CodigoReferenciaAssinatura)
				};

				_dbConnection.Execute(sqlPedidoDetalhe, pedidoDetalhe, transaction);
				#endregion Inserir PedidoDetalhe

				#region Inserir Lancamento
				var sqlLancamento = @"
                        INSERT INTO Lancamento (
                            IdUsuario, IdTransacao, IdTipo, Valor, DataLancamento, Ativo,
                            Descricao, IdStatus, Bloqueado, DataReferencia, OrdemExibicao
                        ) VALUES (
                            @IdUsuario, @IdTransacao, @IdTipo, @Valor, @DataLancamento, @Ativo,
                            @Descricao, @IdStatus, @Bloqueado, @DataReferencia, @OrdemExibicao
                        )";

				var lancamento = new
				{
					IdUsuario = userId,
					IdTransacao = transacaoId,
					IdTipo = 57,
					Valor = -99.9m,
					DataLancamento = agora,
					Ativo = false,
					Descricao = "Pagamento manual de assinatura",
					IdStatus = 1,
					Bloqueado = false,
					DataReferencia = agora,
					OrdemExibicao = 1
				};

				if (performLaunch)
					_dbConnection.Execute(sqlLancamento, lancamento, transaction);
				#endregion Inserir Lancamento

				#region Recuperar e atualizar UsuarioProduto
				var sqlUsuarioProduto = @"
                        SELECT TOP 1 *
                        FROM UsuarioProduto
                        WHERE Ativo = 1 AND IdUsuario = @IdUsuario";

				var usuarioProduto = _dbConnection.QueryFirstOrDefault<UsuarioProduto>(
					sqlUsuarioProduto,
					new { IdUsuario = userId },
					transaction
				);

				var sqlUpdateUsuarioProduto = @"
                            UPDATE UsuarioProduto
                            SET 
                                AssinaturaHabilitada = @AssinaturaHabilitada,
                                AssinaturaAte = @AssinaturaAte,
                                AssinaturaDe = @AssinaturaDe,
                                AssinaturaProximaCobranca = @AssinaturaProximaCobranca,
                                DataAssinatura = @DataAssinatura,
								AssinaturaManual = @AssinaturaManual
                            WHERE IdUsuarioProduto = @Id";

				_dbConnection.ExecuteAsync(
					sqlUpdateUsuarioProduto,
					new
					{
						Id = usuarioProduto.IdUsuarioProduto,
						AssinaturaHabilitada = true,
						AssinaturaAte = endDate ?? hoje.AddDays(30),
						AssinaturaDe = startDate ?? hoje,
						AssinaturaProximaCobranca = endDate?.AddDays(1) ?? hoje.AddDays(30),
						DataAssinatura = agora,
						AssinaturaManual = manual
					},
					transaction
				);
				#endregion Recuperar e atualizar UsuarioProduto
				
				#region Executar distribuição
				ExecuteDistribution(usuarioProduto.IdUsuarioProduto, transaction);
				#endregion Executar distribuição

				transaction.Commit();

				return pedidoId;
			}
			catch (Exception)
			{
				transaction.Rollback();
				throw;
			}
			finally
			{
				_dbConnection.Close();
			}
		}
	}

	public IEnumerable<object> GetSubscriptionsHistory(Guid userId)
	{
		const string query = @"
                SELECT
                    PedidoDetalhe.*
                FROM Pedido 
                INNER JOIN PedidoDetalhe ON PedidoDetalhe.IdPedido = Pedido.IdPedido
                WHERE 
                    Pedido.IdUsuario = @IdUsuario
	                AND Pedido.Tipo = 57
                    AND Pedido.CodigoReferenciaAssinatura IS NOT NULL 
                ORDER BY PedidoDetalhe.DataAtualizacao DESC";

		var subscriptionHistory = _dbConnection.Query(query, new { IdUsuario = userId.ToString() }).ToList();

		return subscriptionHistory;
	}

	public bool IsSubscriber(string login)
	{
		const string query = @"
                SELECT * FROM Usuario 
                INNER JOIN UsuarioProduto ON UsuarioProduto.IdUsuario = Usuario.IdUsuario
                WHERE Login = @login AND AssinaturaHabilitada = 1";

		var isSubscriber = _dbConnection.Query(query, new { login }).ToList();

		return isSubscriber.Count != 0;
	}

	public void ExecuteDistribution(int usuarioProdutoId, IDbTransaction transaction)
	{
		#region Executar distribuição
		var parameters = new { IdUsuarioProduto = usuarioProdutoId, SomenteVisualizacao = false };

		_dbConnection.Execute("Spc_LancarCashback_Assinatura", parameters, commandType: CommandType.StoredProcedure, transaction: transaction);
		#endregion Executar distribuição
	}

	public void ExecuteDistribution(int usuarioProdutoId)
	{
		#region Executar distribuição
		var parameters = new { IdUsuarioProduto = usuarioProdutoId, SomenteVisualizacao = false };

		_dbConnection.Execute("Spc_LancarCashback_Assinatura", parameters, commandType: CommandType.StoredProcedure);
		#endregion Executar distribuição
	}

	public async Task<IEnumerable<LatestSubscriptionDto>> GetLatestSubscriptionsSummary(string login, string nome, bool? ativa, int pageNumber, int pageSize)
	{
		var parameters = new DynamicParameters();
		var sqlBuilder = new StringBuilder();

		// A CTE base continua a mesma
		sqlBuilder.AppendLine(@"
			WITH UltimaAssinatura AS (
				SELECT 
					U.IdUsuario, U.Login, U.Nome, U.LinkAssistenteVirtual, PD.DataAssinatura, 
					PD.AssinaturaDe, PD.AssinaturaAte,
					(SELECT UP.AssinaturaHabilitada FROM UsuarioProduto UP WHERE UP.IdUsuario = PE.IdUsuario AND UP.Ativo = 1) AS AssinaturaHabilitada,
					(SELECT UP.AssinaturaManual FROM UsuarioProduto UP WHERE UP.IdUsuario = PE.IdUsuario AND UP.Ativo = 1) AS AssinaturaManual,
					(SELECT CASE WHEN UP.AssinaturaManual = 1 THEN 'MANUAL' ELSE 'PAGAR.ME/ASAAS' END FROM UsuarioProduto UP WHERE UP.IdUsuario = PE.IdUsuario AND UP.Ativo = 1) AS OrigemAssinatura,
					ROW_NUMBER() OVER (PARTITION BY U.Login ORDER BY PD.AssinaturaDe DESC) AS RN
				FROM Pedido PE
				INNER JOIN PedidoDetalhe PD ON PE.IdPedido = PD.IdPedido
				INNER JOIN Usuario U ON PE.IdUsuario = U.IdUsuario
				WHERE PE.CodigoReferenciaAssinatura IS NOT NULL
			)
			SELECT IdUsuario, Login, Nome, LinkAssistenteVirtual, DataAssinatura, AssinaturaDe, AssinaturaAte, AssinaturaHabilitada, AssinaturaManual, OrigemAssinatura
			FROM UltimaAssinatura
			WHERE RN = 1
		");

		// Adiciona filtros dinamicamente
		if (!string.IsNullOrWhiteSpace(login))
		{
			sqlBuilder.Append(" AND Login LIKE @Login");
			parameters.Add("Login", $"%{login}%");
		}

		if (!string.IsNullOrWhiteSpace(nome))
		{
			sqlBuilder.Append(" AND Nome LIKE @Nome");
			parameters.Add("Nome", $"%{nome}%");
		}

		if (ativa.HasValue)
		{
			sqlBuilder.Append(" AND AssinaturaHabilitada = @Ativa");
			parameters.Add("Ativa", ativa.Value);
		}

		// Ordenação e Paginação
		sqlBuilder.AppendLine(" ORDER BY Nome");
		//sqlBuilder.AppendLine(" OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY");
		//parameters.Add("Offset", (pageNumber - 1) * pageSize);
		//parameters.Add("PageSize", pageSize);

		return await _dbConnection.QueryAsync<LatestSubscriptionDto>(sqlBuilder.ToString(), parameters);
	}

	public async Task LaunchNewCycleAsync(Guid userId, LaunchCycleDto dto)
	{
		using (var scope = _serviceProvider.CreateScope())
		{
			var _dbContext = _serviceProvider.GetRequiredService<DatabaseContext>();

			using (var transaction = await _dbContext.Database.BeginTransactionAsync())
			{
				try
				{
					// 1. ATUALIZAR UsuarioProduto
					var userProduct = await _dbContext.UsuarioProduto.FirstOrDefaultAsync(up => up.IdUsuario == userId && up.Ativo && up.AssinaturaHabilitada) ?? throw new Exception("Assinatura ativa do usuário não encontrada.");

					DateTime newStartDate = userProduct.AssinaturaAte.Value.AddDays(1);

					userProduct.AssinaturaDe = newStartDate;
					userProduct.AssinaturaAte = dto.NewEndDate;
					userProduct.AssinaturaHabilitada = true;
					userProduct.AssinaturaProximaCobranca = newStartDate.AddMonths(1);
					userProduct.AssinaturaManual = true;

					_dbContext.UsuarioProduto.Update(userProduct);

					var pedido = _dbContext.Pedido.Where(p => p.IdUsuario == userId && !string.IsNullOrEmpty(p.CodigoReferenciaAssinatura)).OrderByDescending(p => p.IdPedido).FirstOrDefault();

					// 2. INSERIR PedidoDetalhe
					var pedidoDetalhe = new PedidoDetalhe
					{
						Descricao = "Lançamento de ciclo manual",
						DataAtualizacao = DateTime.Now,
						Ativo = true,
						IdPedido = pedido.IdPedido,
						IdStatus = 2,
						IdUsuario = userId,
						DataAssinatura = userProduct.DataAssinatura,
						AssinaturaDe = newStartDate,
						AssinaturaAte = dto.NewEndDate,
						AssinaturaProximaCobranca = newStartDate.AddMonths(1),
						CodigoReferenciaFatura = ""
					};

					_dbContext.PedidoDetalhe.Add(pedidoDetalhe);

					#region Executar distribuição
					ExecuteDistribution(userProduct.IdUsuarioProduto);
					#endregion Executar distribuição

					await _dbContext.SaveChangesAsync();
					await transaction.CommitAsync();
				}
				catch (Exception ex)
				{
					await transaction.RollbackAsync();
					throw new Exception($"Erro ao lançar novo ciclo: {ex.Message}", ex);
				}
			}
		}
	}

	public async Task<IEnumerable<SubscriptionCycleDto>> GetSubscriptionCyclesAsync(Guid userId)
	{
		using (var scope = _serviceProvider.CreateScope())
		{
			var _dbContext = _serviceProvider.GetRequiredService<DatabaseContext>();
			return await _dbContext.PedidoDetalhe.Include(pd => pd.Pedido)
			.Where(pd => pd.IdUsuario == userId && pd.Pedido.CodigoReferenciaAssinatura != null)
			.OrderByDescending(pd => pd.IdPedidoDetalhe)
			.Select(pd => new SubscriptionCycleDto
			{
				IdPedidoDetalhe = pd.IdPedidoDetalhe,
				DataAssinatura = pd.DataAssinatura,
				AssinaturaDe = pd.AssinaturaDe,
				AssinaturaAte = pd.AssinaturaAte,
				Valor = pd.Pedido.ValorPedido
			})
			.ToListAsync();
		}
	}

	public async Task DeleteSubscriptionCycleAsync(long pedidoDetalheId)
	{
		using (var scope = _serviceProvider.CreateScope())
		{
			var _dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

			// 1. INICIAR A TRANSAÇÃO
			// Garante que todas as operações seguintes sejam tratadas como uma única unidade.
			using (var transaction = await _dbContext.Database.BeginTransactionAsync())
			{
				try
				{
					// 2. ENCONTRAR O CICLO A SER DELETADO
					var cycleToDelete = await _dbContext.PedidoDetalhe.FirstOrDefaultAsync(pd => pd.IdPedidoDetalhe == pedidoDetalheId);

					if (cycleToDelete == null)
						throw new Exception("Ciclo não encontrado");

					var userId = cycleToDelete.IdUsuario;

					// 3. REMOVER O CICLO DO CONTEXTO
					// Ele só será removido do banco no momento do SaveChangesAsync.
					_dbContext.PedidoDetalhe.Remove(cycleToDelete);

					// 4. ENCONTRAR O "NOVO" ÚLTIMO CICLO QUE RESTOU
					// Buscamos o ciclo mais recente para este usuário, EXCLUINDO o que acabamos de remover.
					var newLatestCycle = await _dbContext.PedidoDetalhe
						.Where(pd => pd.IdUsuario == userId && pd.IdPedidoDetalhe != pedidoDetalheId)
						.OrderByDescending(pd => pd.AssinaturaAte)
						.FirstOrDefaultAsync();

					// 5. ENCONTRAR E ATUALIZAR A ASSINATURA PRINCIPAL (UsuarioProduto)
					var userSubscription = await _dbContext.UsuarioProduto.FirstOrDefaultAsync(up => up.IdUsuario == userId && up.Ativo && up.AssinaturaHabilitada);

					if (userSubscription == null)
					{
						// Se não houver uma assinatura principal, algo está inconsistente.
						// É mais seguro reverter a transação.
						throw new Exception("Assinatura principal do usuário não encontrada");
					}

					if (newLatestCycle != null)
					{
						// Se um ciclo anterior foi encontrado, atualiza a assinatura principal
						// para refletir as datas desse ciclo.
						userSubscription.AssinaturaDe = newLatestCycle.AssinaturaDe;
						userSubscription.AssinaturaAte = newLatestCycle.AssinaturaAte;
						userSubscription.AssinaturaHabilitada = true; // Garante que está habilitada
						userSubscription.AssinaturaProximaCobranca = newLatestCycle.AssinaturaProximaCobranca ?? newLatestCycle.AssinaturaDe?.AddMonths(1);
						userSubscription.DataAssinatura = newLatestCycle.DataAssinatura ?? DateTime.UtcNow;
						userSubscription.AssinaturaManual = true; // Presumindo que o ciclo é manual
					}
					else
					{
						// Se NENHUM ciclo anterior foi encontrado, significa que removemos o ÚNICO ciclo.
						// A assinatura deve ser desativada.
						userSubscription.AssinaturaHabilitada = false;
						userSubscription.DataAssinatura = null;
						userSubscription.AssinaturaDe = null;
						userSubscription.AssinaturaAte = null;
						userSubscription.AssinaturaProximaCobranca = null;
					}

					// 6. SALVAR TODAS AS MUDANÇAS ATOMICAMENTE
					await _dbContext.SaveChangesAsync();

					// 7. COMMITAR A TRANSAÇÃO
					await transaction.CommitAsync();
				}
				catch (Exception)
				{
					// 8. ROLLBACK EM CASO DE ERRO
					await transaction.RollbackAsync();
					throw;
				}
			}
		}
	}

	public async Task UpdateVirtualAssistantLinkAsync(Guid userId, string link)
	{
		using (var scope = _serviceProvider.CreateScope())
		{
			var _dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
			var user = await _dbContext.Usuario.FindAsync(userId);
			
			if (user == null)
				throw new Exception("Usuário não encontrado");

			// Atualiza o campo específico
			user.LinkAssistenteVirtual = link;

			// Salva a alteração no banco de dados
			await _dbContext.SaveChangesAsync();
		}
	}
}

public class LatestSubscriptionDto
{
	public Guid IdUsuario { get; set; }
	public string Login { get; set; }
	public string Nome { get; set; }
	public DateTime DataAssinatura { get; set; }
	public DateTime AssinaturaDe { get; set; }
	public DateTime AssinaturaAte { get; set; }
	public bool AssinaturaHabilitada { get; set; }
	public bool AssinaturaManual { get; set; }
	public string OrigemAssinatura { get; set; }
	public string LinkAssistenteVirtual { get; set; }
}

public class CreateManualSubscriptionDto
{
	public string Login { get; set; }
	public Guid UserId { get; set; }
	public DateTime StartDate { get; set; }
	public DateTime EndDate { get; set; }
	public bool PerformLaunch { get; set; }
}

public class LaunchCycleDto
{
	[Required]
	public DateTime NewEndDate { get; set; }
}

public class SubscriptionCycleDto
{
	public long IdPedidoDetalhe { get; set; }
	public DateTime? DataAssinatura { get; set; }
	public DateTime? AssinaturaDe { get; set; }
	public DateTime? AssinaturaAte { get; set; }
	public decimal? Valor { get; set; } // Supondo que você tenha um valor
}

public class UpdateVirtualAssistantLinkDto
{
	public string Link { get; set; }
}