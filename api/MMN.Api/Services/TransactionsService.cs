using Dapper;
using Microsoft.Data.SqlClient;
using MMN.Api.Models.Request;
using MMN.Dominio.Model;
using MMN.INegocio.Negocio;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MMN.Api.Services;

public interface ITransactionsService
{
    Task<(IEnumerable<object> Transactions, int TotalCount)> GetTransactions(GetTransactionsSearchParameters parameters);
    Task<object> GetReleases(long transactionId);
    Task DeleteOrderWithCashbackCoupon(int orderId);
}

public class TransactionsService : ITransactionsService
{
    private readonly IDbConnection _dbConnection;
    private readonly ILancamentoNegocio _lancamentoNegocio;

    public TransactionsService(IDbConnection dbConnection, ILancamentoNegocio lancamentoNegocio)
    {
        _dbConnection = dbConnection;
        _lancamentoNegocio = lancamentoNegocio;
    }

    public async Task<(IEnumerable<object> Transactions, int TotalCount)> GetTransactions(GetTransactionsSearchParameters parameters)
    {
        var sql = @"
            DECLARE @Tipos AS TABLE (Tipo NVARCHAR(MAX));
            DECLARE @Status AS TABLE (Status NVARCHAR(MAX));

            -- Preencher @Tipos se Tipos não for nulo
            IF @TiposParam IS NOT NULL
            BEGIN
                INSERT INTO @Tipos
                SELECT value FROM STRING_SPLIT(@TiposParam, ',');
            END

            -- Preencher @Status se Status não for nulo
            IF @StatusParam IS NOT NULL
            BEGIN
                INSERT INTO @Status
                SELECT value FROM STRING_SPLIT(@StatusParam, ',');
            END

            -- Query para obter o total de registros
            SELECT COUNT(DISTINCT T.IdTransacao) AS TotalCount
            FROM Transacao AS T
            LEFT OUTER JOIN Pedido AS P ON P.IdTransacao = T.IdTransacao
            LEFT OUTER JOIN Credenciamento AS C ON C.IdUsuario = P.IdUsuarioComerciante
            LEFT OUTER JOIN Anunciante AS A ON A.IdAnunciante = T.IdAnunciante
            LEFT OUTER JOIN AnuncianteCashback AS AC ON AC.IdAnunciante = A.IdAnunciante AND AC.Ativo = 1
            LEFT OUTER JOIN Usuario AS U ON U.IdUsuario = T.IdUsuario
            LEFT OUTER JOIN Tipo AS TP ON TP.IdTipo = T.IdTipo
            LEFT OUTER JOIN Status AS S ON S.IdStatus = T.IdStatus
            WHERE
                (@IdTransacao IS NULL OR T.IdTransacao = @IdTransacao) AND
                (@Login IS NULL OR LOWER(U.Login) LIKE '%' + @Login + '%' COLLATE Latin1_General_CI_AI) AND
                (@Nome IS NULL OR UPPER(U.Nome) LIKE '%' + @Nome + '%' COLLATE Latin1_General_CI_AI) AND
                (@Estabelecimento IS NULL OR UPPER(ISNULL(ISNULL(C.Estabelecimento, A.Nome), 'Quanta Shop')) LIKE '%' + @Estabelecimento + '%') AND
                (@TiposParam IS NULL OR UPPER(T.IdTipo) IN (SELECT Tipo FROM @Tipos)) AND
                (@StatusParam IS NULL OR UPPER(T.IdStatus) IN (SELECT Status FROM @Status)) AND
                (@StartDate IS NULL OR CAST(T.DataTransacao AS DATE) >= CAST(@StartDate AS DATE)) AND
                (@EndDate IS NULL OR CAST(T.DataTransacao AS DATE) <= CAST(@EndDate AS DATE));

            -- Query para obter os dados paginados
            SELECT DISTINCT
                T.IdTransacao,
                P.IdPedido,
                T.IdUsuario,
                LOWER(U.Login) AS Login,
                UPPER(U.Nome) AS Nome,
                UPPER(ISNULL(ISNULL(C.Estabelecimento, A.Nome), 'Quanta Shop')) AS Estabelecimento,
                T.IdTipo,
                UPPER(TP.Descricao) AS Tipo,
                T.IdStatus,
                UPPER(S.Nome) AS Status,
                UPPER(TRIM(T.Descricao)) AS Descricao,
                P.ValorPedido AS ValorCompra,
                T.ValorPrincipal AS ValorCashback,
                IIF(P.IdUsuarioComerciante IS NULL, 1, 2) AS TipoParceiro,
                IIF(P.IdUsuarioComerciante IS NULL, 'Parceiro online', 'Parceiro local') AS Parceiro,
                ISNULL(C.PercentualCashback, AC.Percentual) AS Percentual,
                T.DataTransacao,
                ISNULL(P.Cancelado, 0) AS Cancelado
            FROM Transacao AS T
            LEFT OUTER JOIN Pedido AS P ON P.IdTransacao = T.IdTransacao
            LEFT OUTER JOIN Credenciamento AS C ON C.IdUsuario = P.IdUsuarioComerciante
            LEFT OUTER JOIN Anunciante AS A ON A.IdAnunciante = T.IdAnunciante
            LEFT OUTER JOIN AnuncianteCashback AS AC ON AC.IdAnunciante = A.IdAnunciante AND AC.Ativo = 1
            LEFT OUTER JOIN Usuario AS U ON U.IdUsuario = T.IdUsuario
            LEFT OUTER JOIN Tipo AS TP ON TP.IdTipo = T.IdTipo
            LEFT OUTER JOIN Status AS S ON S.IdStatus = T.IdStatus
            WHERE
                (@IdTransacao IS NULL OR T.IdTransacao = @IdTransacao) AND
                (@Login IS NULL OR LOWER(U.Login) LIKE '%' + @Login + '%' COLLATE Latin1_General_CI_AI) AND
                (@Nome IS NULL OR UPPER(U.Nome) LIKE '%' + @Nome + '%' COLLATE Latin1_General_CI_AI) AND
                (@Estabelecimento IS NULL OR UPPER(ISNULL(ISNULL(C.Estabelecimento, A.Nome), 'Quanta Shop')) LIKE '%' + @Estabelecimento + '%') AND
                (@TiposParam IS NULL OR UPPER(T.IdTipo) IN (SELECT Tipo FROM @Tipos)) AND
                (@StatusParam IS NULL OR UPPER(T.IdStatus) IN (SELECT Status FROM @Status)) AND
                (@StartDate IS NULL OR CAST(T.DataTransacao AS DATE) >= CAST(@StartDate AS DATE)) AND
                (@EndDate IS NULL OR CAST(T.DataTransacao AS DATE) <= CAST(@EndDate AS DATE))
            ORDER BY T.DataTransacao DESC
            OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;";

        var offset = (parameters.PageNumber - 1) * parameters.PageSize;

        var queryParameters = new
        {
            parameters.IdTransacao,
            parameters.Login,
            parameters.Nome,
            parameters.Estabelecimento,
            parameters.Descricao,
            parameters.StartDate,
            parameters.EndDate,
            TiposParam = parameters.Tipos?.Any() == true ? string.Join(",", parameters.Tipos) : null,
            StatusParam = parameters.Status?.Any() == true ? string.Join(",", parameters.Status) : null,
            Offset = offset,
            PageSize = parameters.PageSize
        };

        using (var multi = await _dbConnection.QueryMultipleAsync(sql, queryParameters))
        {
            var totalCount = await multi.ReadSingleAsync<int>();
            var transactions = await multi.ReadAsync<object>();
            return (transactions, totalCount);
        }
    }

    public async Task<object> GetReleases(long transactionId)
    {
        var storedProcedure = "sp_OrdenarLancamentosTransacao";

        await _dbConnection.QueryAsync<Lancamento>(
           storedProcedure,
           new { IdTransacao = transactionId },
           commandType: CommandType.StoredProcedure
        );

        var sql = @"
            SELECT Lancamento.*, Usuario.Login, Usuario.Nome AS NomeUsuario, Tipo.Descricao AS Tipo, Status.Nome AS Status
            FROM Lancamento 
            INNER JOIN Usuario ON Usuario.IdUsuario = Lancamento.IdUsuario
            INNER JOIN Tipo ON Tipo.IdTipo = Lancamento.IdTipo
            INNER JOIN Status ON Status.IdStatus = Lancamento.IdStatus
            WHERE IdTransacao = @IdTransacao
            ORDER BY OrdemExibicao, IdTipo, Valor";

        var result = await _dbConnection.QueryAsync(sql, new { IdTransacao = transactionId });

        return result;
    }

	public async Task DeleteOrderWithCashbackCoupon(int orderId)
	{
		try
		{
			await _dbConnection.ExecuteAsync(
				"sp_ExcluirPedidoComCupomCashback",
				new { IdPedido = orderId },
				commandType: CommandType.StoredProcedure
			);
		}
		catch (SqlException ex)
		{
			throw new Exception($"Erro ao excluir pedido: {ex.Message}");
		}
	}
}
