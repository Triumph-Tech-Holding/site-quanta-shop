using Dapper;
using Mapster;
using MMN.Api.Models.Request;
using MMN.Dominio.Model;
using MMN.INegocio.Negocio;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MMN.Api.Services;

public interface IQuantaFriendshipService
{
    IEnumerable<object> GetFriendshipInfo(Guid IdUsuario);

    IEnumerable<object> GetFriendshipHistory(Guid IdUsuario, string loginUsuarioFilho);

    object GetCheckUserEligibility(Guid IdUsuario);
}

public class QuantaFriendshipService : IQuantaFriendshipService
{
    private readonly IDbConnection _dbConnection;
    private readonly IQuantaAmizadeNegocio _quantaAmizadeNegocio;

    public QuantaFriendshipService(IDbConnection dbConnection, IQuantaAmizadeNegocio quantaAmizadeNegocio)
    {
        _dbConnection = dbConnection;
        _quantaAmizadeNegocio = quantaAmizadeNegocio;
    }

    public IEnumerable<object> GetFriendshipInfo(Guid IdUsuario)
    {
        const string sql = @"              
             SELECT 
	            U.Login, 
	            U.Nome, 
	            U.Celular, 
	            U.URLIMG, 
	            U.DataCadastro,
	            DATEADD(DAY, 30, U.DataCadastro) AS DataLimite,
	            ISNULL(SUM(QAH.ValorCashback), 0) AS ValorCashbackAcumulado
            FROM QuantaAmizade QA
            LEFT OUTER JOIN QuantaAmizadeHistorico QAH ON QA.IdQuantaAmizade = QAH.IdQuantaAmizade
            LEFT OUTER JOIN Usuario U ON U.IdUsuario = QA.IdUsuarioFilho
            WHERE QA.IdUsuarioPai = @IdUsuario
            GROUP BY U.Login, U.Nome, U.Celular, U.URLIMG, U.DataCadastro
            HAVING GETDATE() <= DATEADD(DAY, 35, U.DataCadastro);";

        var result = _dbConnection.Query(sql, new { IdUsuario }).ToList();

        return result;
    }

    public IEnumerable<object> GetFriendshipHistory(Guid IdUsuario, string loginUsuarioFilho)
    {
        const string sql = @"
            DECLARE @IdUsuarioFilho as UNIQUEIDENTIFIER
            DECLARE @IdQuantaAmizade as INT

            SELECT @IdUsuarioFilho = IdUsuario FROM Usuario WHERE Login = @loginUsuarioFilho;

            SELECT @IdQuantaAmizade = IdQuantaAmizade FROM QuantaAmizade WHERE IdUsuarioPai = @IdUsuario AND IdUsuarioFilho = @IdUsuarioFilho;
                        
            SELECT * FROM QuantaAmizadeHistorico WHERE IdQuantaAmizade = @IdQuantaAmizade;                  
        ";

        var result = _dbConnection.Query(sql, new { IdUsuario, loginUsuarioFilho }).ToList();

        return result;
    }

    public object GetCheckUserEligibility(Guid IdUsuario)
    {
        const string sql = @"
            DECLARE @ValorMetaMinuto DECIMAL(14,2);

            SELECT @ValorMetaMinuto = Valor FROM Configuracao WHERE Chave = 'VALOR_QUANTA_AMIZADE';

            SELECT SUM(Valor) AS Total, @ValorMetaMinuto AS Valor
            FROM Lancamento 
            WHERE IdTipo = 52 AND IdUsuario = @IdUsuario;";

        var result = _dbConnection.QuerySingleOrDefault(sql, new { IdUsuario });

        return result;
    }
}
