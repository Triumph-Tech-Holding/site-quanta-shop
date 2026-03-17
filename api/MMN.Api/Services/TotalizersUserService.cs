using Dapper;
using Mapster;
using MMN.Api.Models.Request;
using MMN.Dominio.Model;
using MMN.INegocio.Negocio;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace MMN.Api.Services;

public interface ITotalizersUserService
{
    object GetTotalizersUser(Guid IdUsuario);
    object GetMonthCashback(Guid IdUsuario, DateTime DataInicio, DateTime DataFim);
    List<object> GetTop5Connections(Guid userId);
    List<object> GetUserAvaliableLevel(Guid userId, decimal level);
    List<object> GetObjectivesByLevel(decimal level);
    List<object> GetUserObjectivesProgess(Guid userId);
}

public class TotalizersUserService : ITotalizersUserService
{
    private readonly IDbConnection _dbConnection;
    private readonly IQuantaAmizadeNegocio _quantaAmizadeNegocio;

    public TotalizersUserService(IDbConnection dbConnection, IQuantaAmizadeNegocio quantaAmizadeNegocio)
    {
        _dbConnection = dbConnection;
        _quantaAmizadeNegocio = quantaAmizadeNegocio;
    }

    public object GetTotalizersUser(Guid IdUsuario)
    {
        const string sql = @"
            -- Total de amigos
            SELECT ISNULL(COUNT(*), 0) AS Rede
            FROM dbo.fnc_GetUsuarioDownLine(@IdUsuario);

            -- Saldo
            SELECT ISNULL(SUM(Lancamento.Valor) - SUM(LancamentoRetido.Valor), 0) AS Saldo
            FROM Lancamento 
            LEFT OUTER JOIN LancamentoRetido ON LancamentoRetido.IdLancamento = Lancamento.IdLancamento AND LancamentoRetido.Ativo = 1
            WHERE IdUsuario = @IdUsuario;

            -- Ecossistemas
            SELECT ISNULL(COUNT(*), 0) AS Ecossitemas FROM Ecossistema WHERE Ativo = 1;

            DECLARE @PontosEmReais AS DECIMAL(14,2);
            SELECT @PontosEmReais = Valor FROM Configuracao WHERE Chave = 'VALOR_PONTO_EM_REAIS';

            SELECT ISNULL(CAST((SUM(Lancamento.Valor) - SUM(LancamentoRetido.Valor)) * @PontosEmReais AS INT), 0) AS Pontos
            FROM Lancamento 
            LEFT OUTER JOIN LancamentoRetido ON LancamentoRetido.IdLancamento = Lancamento.IdLancamento AND LancamentoRetido.Ativo = 1
            WHERE IdUsuario = @IdUsuario;";

        var multi = _dbConnection.QueryMultiple(sql, new { IdUsuario });

        var totalizers = new
        {
            Rede = multi.ReadSingle<int>(),
            Saldo = multi.ReadSingle<decimal>(),
            Ecossistemas = multi.ReadSingle<int>(),
            Pontos = multi.ReadSingle<int>()
        };

        return totalizers;
    }

    public object GetMonthCashback(Guid IdUsuario, DateTime DataInicio, DateTime DataFim)
    {
        const string sql = @"              
            SELECT Total FROM dbo.fnc_ObterResumoConsumoMensal(@IdUsuario, @DataInicio, @DataFim) ORDER BY Ano DESC, Mes DESC
        ";

        var parameters = new
        {
            IdUsuario = IdUsuario.ToString(),
            DataInicio = DataInicio.Date.ToString("yyyy-MM-dd"),
            DataFim = DataFim.Date.ToString("yyyy-MM-dd")
        };
        decimal total = _dbConnection.QuerySingleOrDefault<decimal>(sql, parameters);

        return total;
    }

    public List<object> GetTop5Connections(Guid userId)
    {
        const string storedProcedure = "sp_ObterTop5Rede";

        var parameters = new
        {
            IdUsuario = userId
        };

        var result = _dbConnection.Query<object>(
            storedProcedure,
            parameters,
            commandType: CommandType.StoredProcedure
        ).ToList();

        return result;
    }

    public List<object> GetUserAvaliableLevel(Guid userId, decimal level)
    {
        const string sql = @"              
            SELECT 
                O.Grupo,
                COUNT(DISTINCT O.IdObjetivo) AS ObjetivosEsperados,
                COUNT(DISTINCT OU.IdObjetivo) AS ObjetivosConcluidos
            FROM 
                Objetivo AS O
            LEFT OUTER JOIN 
                ObjetivoUsuario AS OU ON O.IdObjetivo = OU.IdObjetivo
                AND OU.IdUsuario = @IdUsuario
            WHERE 
                O.Ativo = 1 -- Considerando apenas objetivos ativos
                AND O.Nivel = @Nivel
            GROUP BY 
                O.Grupo
            HAVING 
                COUNT(DISTINCT OU.IdObjetivo) = 0;
        ";

        var parameters = new
        {
            IdUsuario = userId,
            Nivel = level
        };

        var result = _dbConnection.Query<object>(
            sql,
            parameters
        ).ToList();

        return result;
    }

    public List<object> GetObjectivesByLevel(decimal level)
    {
        const string sql = @"SELECT * FROM Objetivo WHERE Nivel = @Nivel AND Ativo = 1 ORDER BY Ordem;";

        var parameters = new
        {
            Nivel = level
        };

        var result = _dbConnection.Query<object>(
            sql,
            parameters
        ).ToList();

        return result;
    }

    public List<object> GetUserObjectivesProgess(Guid userId)
    {
        const string storedProcedure = "sp_ObterObjetivosUsuario";

        var parameters = new
        {
            IdUsuario = userId
        };

        var result = _dbConnection.Query<object>(
            storedProcedure,
            parameters,
            commandType: CommandType.StoredProcedure
        ).ToList();

        return result;
    }
}
