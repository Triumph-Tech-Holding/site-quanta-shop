using Dapper;
using Microsoft.Extensions.Options;
using MMN.Api.Models;
using MMN.Api.Models.Request;
using MMN.Dominio.Model;
using MMN.INegocio.Negocio;
using MMN.Util.Extensions;
using MMN.Util.Model;
using MMN.Util.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MMN.Api.Services;

public interface IAdminService
{
    Task<IEnumerable<object>> GetEcosystemBilling(int ecosystemId);
    Task<IEnumerable<object>> GetCompaniesByEcosystem(int ecosystemId);
}

public class AdminService : IAdminService
{
    private readonly AppSettings _appSettings;
    private readonly IDbConnection _dbConnection;

    public AdminService(IOptions<AppSettings> appSettings, IDbConnection dbConnection)
    {
        _appSettings = appSettings.Value;
        _dbConnection = dbConnection;
    }

    public async Task<IEnumerable<object>> GetEcosystemBilling(int ecosystemId)
    {
        const string query = @"
            SELECT 
                SUM(CuponCashback.Valor) AS Faturamento,
	            FORMAT(SUM(CuponCashback.Valor), 'C2', 'PT-BR') AS FaturamentoFormatado,
                FORMAT(CAST(CuponCashback.DataCompra AS DATE), 'MM/yyyy') AS Período
            FROM Credenciamento
            INNER JOIN CuponCashback ON CuponCashback.IdComerciante = Credenciamento.IdUsuario
            WHERE IdEcossistema = @ecosystemId
            GROUP BY FORMAT(CAST(CuponCashback.DataCompra AS DATE), 'MM/yyyy'), 
                     YEAR(CAST(CuponCashback.DataCompra AS DATE)), 
                     MONTH(CAST(CuponCashback.DataCompra AS DATE))
            ORDER BY YEAR(CAST(CuponCashback.DataCompra AS DATE)) DESC, 
                     MONTH(CAST(CuponCashback.DataCompra AS DATE)) DESC;";

        var billing = await _dbConnection.QueryAsync(query, new { ecosystemId });

        return billing.ToList();
    }

    public async Task<IEnumerable<object>> GetCompaniesByEcosystem(int ecosystemId)
    {
        const string query = @"
            SELECT
                UPPER(Credenciamento.Estabelecimento) AS Estabelecimento,
                UPPER(Credenciamento.Rua) AS Rua,
                UPPER(Credenciamento.Numero) AS Numero,
                UPPER(Credenciamento.Complemento) AS Complemento,
                UPPER(Credenciamento.Bairro) AS Bairro,
                UPPER(Cidade.Nome) AS Cidade,
                UPPER(Estado.UF) AS Estado,
                Credenciamento.CelularContato,
                Credenciamento.Telefone,
                LOWER(Usuario.Login) AS Login,
                UPPER(Usuario.Nome) AS Responsavel,
                LOWER(Usuario.Email) AS Email,
                Usuario.Celular,
                LOWER(Pai.Login) AS Patrocinador
            FROM Credenciamento 
            INNER JOIN Cidade ON Cidade.IdCidade = Credenciamento.IdCidade
            INNER JOIN Estado ON Estado.IdEstado = Cidade.IdEstado
            INNER JOIN Usuario ON Usuario.IdUsuario = Credenciamento.IdUsuario
            INNER JOIN Usuario AS Pai ON Pai.IdUsuario = Credenciamento.IdUsuarioPai
            WHERE IdEcossistema = @ecosystemId AND Credenciamento.Status = 2
            ORDER BY Credenciamento.Estabelecimento;";

        var billing = await _dbConnection.QueryAsync(query, new { ecosystemId });

        return billing.ToList();
    }
}
