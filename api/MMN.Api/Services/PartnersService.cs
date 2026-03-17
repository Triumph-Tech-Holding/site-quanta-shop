using Dapper;
using Mapster;
using MMN.Api.Models.Request;
using MMN.Dominio.Model;
using MMN.INegocio.Negocio;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MMN.Api.Services;

public interface IPartnersService
{
    IEnumerable<dynamic> GetPartners(string type = null, string name = null, string category = null, int page = 1, int limit = 12);
    IEnumerable<object> GetPartner(int id);
    IEnumerable<object> GetNewPartners();
    IEnumerable<object> GetFeaturedPartners();
    IEnumerable<object> GetTopSellersPartners();
    IEnumerable<object> GetLocalPartners();
    IEnumerable<object> GetBestDiscountsLocalPartners();
    IEnumerable<object> GetTopSellersLocalPartners();
    IEnumerable<object> GetFeaturedLocalPartners();
    IEnumerable<object> GetShopsNearby(float latitude, float longitude, int raio);
}

public class PartnersService : IPartnersService
{
    private readonly IDbConnection _dbConnection;
    private readonly ICategoriaNegocio _categoriaNegocio;

    public PartnersService(IDbConnection dbConnection, ICategoriaNegocio categoriaNegocio)
    {
        _dbConnection = dbConnection;
        _categoriaNegocio = categoriaNegocio;
    }

    public IEnumerable<dynamic> GetPartners(string type = null, string name = null, string category = null, int page = 1, int limit = 12)
    {
        const string sql = "sp_GetAnunciantes";

        // Crie um objeto DynamicParameters para armazenar os parâmetros da stored procedure
        var parameters = new DynamicParameters();
        parameters.Add("@Tipo", type, DbType.String, ParameterDirection.Input);
        parameters.Add("@Nome", name, DbType.String, ParameterDirection.Input);
        parameters.Add("@Categoria", category, DbType.String, ParameterDirection.Input);
        parameters.Add("@Offset", page, DbType.Int32, ParameterDirection.Input);
        parameters.Add("@PageSize", limit, DbType.Int32, ParameterDirection.Input);

        // Execute a stored procedure e obtenha o resultado
        using (IDbConnection dbConnection = _dbConnection) // Supondo que _dbConnection é um IDbConnection já configurado
        {
            var result = dbConnection.Query(sql, parameters, commandType: CommandType.StoredProcedure).ToList();
            return result;
        }
    }

    public IEnumerable<object> GetPartner(int id)
    {
        const string sql = @"
            SELECT
                A.IdAnunciante,
                A.Nome,
                A.ImagemUrl AS Icone,
                AC.MinCashback,
                AC.MaxCashback,
	            AC.Tipo,
	            (SELECT TOP 1 C.Nome 
                 FROM Categoria C
                 INNER JOIN CategoriaAnunciante CA ON CA.IdCategoria = C.IdCategoria
                 WHERE CA.IdAnunciante = A.IdAnunciante) AS Categoria
            FROM Anunciante AS A
            INNER JOIN (
                SELECT 
                    IdAnunciante,
                    MIN(Percentual) AS MinCashback,
                    MAX(Percentual) AS MaxCashback,
		            Tipo
                FROM AnuncianteCashback
                WHERE Ativo = 1
                GROUP BY IdAnunciante, Tipo
            ) AS AC ON AC.IdAnunciante = A.IdAnunciante
            WHERE A.Ativo = 1 AND A.IdAnunciante = @IdAnunciante
            ORDER BY A.DataCadastro DESC";

        var parameters = new
        {
            IdAnunciante = id,
        };

        var result = _dbConnection.Query(sql, parameters).ToList();

        return result;
    }

    public IEnumerable<object> GetNewPartners()
    {
        const string sql = @"
            SELECT TOP 8
                A.IdAnunciante,
                A.Nome,
                A.ImagemUrl AS Icone,
                AC.MinCashback,
                AC.MaxCashback,
	            AC.Tipo,
	            (SELECT TOP 1 C.Nome 
                 FROM Categoria C
                 INNER JOIN CategoriaAnunciante CA ON CA.IdCategoria = C.IdCategoria
                 WHERE CA.IdAnunciante = A.IdAnunciante) AS Categoria,
                'https://www.awin1.com/cread.php?awinaffid=689359&awinmid=' + A.IdAwin + '&clickref={userId}' AS link
            FROM Anunciante AS A
            INNER JOIN (
                SELECT 
                    IdAnunciante,
                    MIN(Percentual) AS MinCashback,
                    MAX(Percentual) AS MaxCashback,
		            Tipo
                FROM AnuncianteCashback
                WHERE Ativo = 1
                GROUP BY IdAnunciante, Tipo
            ) AS AC ON AC.IdAnunciante = A.IdAnunciante
            WHERE A.Ativo = 1
            ORDER BY A.DataCadastro DESC";

        var result = _dbConnection.Query(sql).ToList();

        return result;
    }

    public IEnumerable<object> GetFeaturedPartners()
    {
        const string sql = @"
            SELECT TOP 8
                A.IdAnunciante,
                A.Nome,
                A.ImagemUrl AS Icone,
                AC.MinCashback,
                AC.MaxCashback,
	            AC.Tipo,
	            (SELECT TOP 1 C.Nome 
                 FROM Categoria C
                 INNER JOIN CategoriaAnunciante CA ON CA.IdCategoria = C.IdCategoria
                 WHERE CA.IdAnunciante = A.IdAnunciante) AS Categoria,
                'https://www.awin1.com/cread.php?awinaffid=689359&awinmid=' + A.IdAwin + '&clickref={userId}' AS link
            FROM Anunciante AS A
            INNER JOIN (
                SELECT 
                    IdAnunciante,
                    MIN(Percentual) AS MinCashback,
                    MAX(Percentual) AS MaxCashback,
		            Tipo
                FROM AnuncianteCashback
                WHERE Ativo = 1
                GROUP BY IdAnunciante, Tipo
            ) AS AC ON AC.IdAnunciante = A.IdAnunciante
            WHERE A.Ativo = 1 AND A.Ancora = 1 AND AC.Tipo = 'percentage'
            ORDER BY AC.MaxCashback DESC";

        var result = _dbConnection.Query(sql).ToList();

        return result;
    }

    public IEnumerable<object> GetTopSellersPartners()
    {
        const string sql = @"
            SELECT TOP 8
                A.IdAnunciante,
                A.Nome,
                A.ImagemUrl AS Icone,
                AC.MinCashback,
                AC.MaxCashback,
                AC.Tipo,
                (SELECT TOP 1 C.Nome 
                 FROM Categoria C
                 INNER JOIN CategoriaAnunciante CA ON CA.IdCategoria = C.IdCategoria
                 WHERE CA.IdAnunciante = A.IdAnunciante) AS Categoria,
                'https://www.awin1.com/cread.php?awinaffid=689359&awinmid=' + A.IdAwin + '&clickref={userId}' AS link
            FROM Anunciante AS A
            INNER JOIN (
                SELECT 
                    IdAnunciante,
                    MIN(Percentual) AS MinCashback,
                    MAX(Percentual) AS MaxCashback,
                    Tipo
                FROM AnuncianteCashback
                WHERE Ativo = 1
                GROUP BY IdAnunciante, Tipo
            ) AS AC ON AC.IdAnunciante = A.IdAnunciante
            WHERE A.Ativo = 1
                AND AC.Tipo = 'percentage'
                AND A.IdAnunciante IN (
                    SELECT TOP 8 IdAnunciante
                    FROM Pedido
                    WHERE IdAnunciante IS NOT NULL
                    GROUP BY IdAnunciante
                    ORDER BY COUNT(*) DESC
                )
            ORDER BY AC.MaxCashback DESC;";

        var result = _dbConnection.Query(sql).ToList();

        return result;
    }

    public IEnumerable<object> GetLocalPartners()
    {
        const string sql = @"
            SELECT TOP 12
	            Credenciamento.IdCredenciamento,
	            UPPER(Credenciamento.Estabelecimento) AS Estabelecimento,
	            Credenciamento.PercentualCashback,
	            UPPER(Categoria.Nome) AS Categoria,
                'https://escritorio.quantashop.com.br/anunciante?idAnunciante=' + CAST(Credenciamento.IdCredenciamento AS VARCHAR) AS link,
                Credenciamento.LogoUrl AS Imagem,
	            COUNT(*) AS Quantidade
            FROM CuponCashback
            INNER JOIN Credenciamento ON Credenciamento.IdUsuario = CuponCashback.IdComerciante
            INNER JOIN Categoria ON Categoria.IdCategoria = Credenciamento.IdCategoria
            WHERE Credenciamento.Status = 2 AND MONTH(CuponCashback.DataCompra) >= MONTH(GETDATE()) -1 AND YEAR(CuponCashback.DataCompra) = YEAR(GETDATE()) AND Credenciamento.PercentualCashback >= 10
            GROUP BY Credenciamento.IdCredenciamento, Credenciamento.Estabelecimento, Credenciamento.PercentualCashback, Categoria.Nome, Credenciamento.LogoUrl
            ORDER BY NEWID()";

        var result = _dbConnection.Query(sql).ToList();

        return result;
    }

    public IEnumerable<object> GetBestDiscountsLocalPartners()
    {
        const string sql =
            @"SELECT TOP 3
                Credenciamento.IdCredenciamento,
                UPPER(Credenciamento.Estabelecimento) AS Estabelecimento,
                Credenciamento.PercentualCashback,
                UPPER(Categoria.Nome) AS Categoria,
                'https://escritorio.quantashop.com.br/anunciante?idAnunciante=' + CAST(Credenciamento.IdCredenciamento AS VARCHAR) AS link,
                Credenciamento.LogoUrl AS Imagem,
                COUNT(*) AS Quantidade
            FROM CuponCashback
            INNER JOIN Credenciamento ON Credenciamento.IdUsuario = CuponCashback.IdComerciante
            INNER JOIN Categoria ON Categoria.IdCategoria = Credenciamento.IdCategoria
            WHERE 
	            Credenciamento.PercentualCashback >= 10 AND Credenciamento.Status = 2
            GROUP BY Credenciamento.IdCredenciamento, Credenciamento.Estabelecimento, Credenciamento.PercentualCashback, Categoria.Nome, Credenciamento.LogoUrl
            HAVING COUNT(*) > 15
            ORDER BY Credenciamento.PercentualCashback DESC, NEWID()";

        var result = _dbConnection.Query(sql).ToList();

        return result;
    }

    public IEnumerable<object> GetFeaturedLocalPartners()
    {
        const string sql = @"
            SELECT TOP 3
                Credenciamento.IdCredenciamento,
                UPPER(Credenciamento.Estabelecimento) AS Estabelecimento,
                Credenciamento.PercentualCashback,
                UPPER(Categoria.Nome) AS Categoria,
                'https://escritorio.quantashop.com.br/anunciante?idAnunciante=' + CAST(Credenciamento.IdCredenciamento AS VARCHAR) AS link,
                Credenciamento.LogoUrl AS Imagem,
                COUNT(*) AS Quantidade
            FROM CuponCashback
            INNER JOIN Credenciamento ON Credenciamento.IdUsuario = CuponCashback.IdComerciante
            INNER JOIN Categoria ON Categoria.IdCategoria = Credenciamento.IdCategoria
            WHERE Credenciamento.PercentualCashback <= 10 AND Credenciamento.Status = 2
            GROUP BY Credenciamento.IdCredenciamento, Credenciamento.Estabelecimento, Credenciamento.PercentualCashback, Categoria.Nome, Credenciamento.LogoUrl
            HAVING COUNT(*) >= 50
            ORDER BY 5 DESC, NEWID()";

        var result = _dbConnection.Query(sql).ToList();

        return result;
    }

    public IEnumerable<object> GetTopSellersLocalPartners()
    {
        const string sql = @"
            SELECT TOP 12
                Credenciamento.IdCredenciamento,
                UPPER(Credenciamento.Estabelecimento) AS Estabelecimento,
                Credenciamento.PercentualCashback,
                UPPER(Categoria.Nome) AS Categoria,
                'https://escritorio.quantashop.com.br/anunciante?idAnunciante=' + CAST(Credenciamento.IdCredenciamento AS VARCHAR) AS link,
                Credenciamento.LogoUrl AS Imagem,
                COUNT(*) AS Quantidade
            FROM CuponCashback
            INNER JOIN Credenciamento ON Credenciamento.IdUsuario = CuponCashback.IdComerciante
            INNER JOIN Categoria ON Categoria.IdCategoria = Credenciamento.IdCategoria
            WHERE Credenciamento.PercentualCashback <= 10 AND Credenciamento.Status = 2
            GROUP BY Credenciamento.IdCredenciamento, Credenciamento.Estabelecimento, Credenciamento.PercentualCashback, Categoria.Nome, Credenciamento.LogoUrl
            HAVING COUNT(*) > 50
            ORDER BY NEWID()";

        var result = _dbConnection.Query(sql).ToList();

        return result;
    }

    public IEnumerable<object> GetShopsNearby(float latitude = -22.9663825f, float longitude = -43.386027f, int raio = 3)
    {
        const string sql = @"
             SELECT 
                 IdCredenciamento AS id,
                 UPPER(estabelecimento) AS name,
	             Rua AS street,
	             Numero AS number,
	             Bairro AS neighborhood,
	             CEP AS zipcode,
	             Cidade.Nome AS city,
	             UF AS state,
                 Categoria.Nome AS category,
                 Telefone AS phone,
                 TRY_CAST(Latitude AS FLOAT) AS latitude,
                 TRY_CAST(Longitude AS FLOAT) AS longitude,
                 ( 
                     6371 * ACOS(
                         COS(RADIANS(@userLatitude)) * COS(RADIANS(TRY_CAST(Latitude AS FLOAT))) 
                         * COS(RADIANS(TRY_CAST(Longitude AS FLOAT)) - RADIANS(@userLongitude)) 
                         + SIN(RADIANS(@userLatitude)) * SIN(RADIANS(TRY_CAST(Latitude AS FLOAT)))
                     )
                 ) AS distance,
                 CASE
                     WHEN (
                         6371 * ACOS(
                             COS(RADIANS(@userLatitude)) * COS(RADIANS(TRY_CAST(Latitude AS FLOAT))) 
                             * COS(RADIANS(TRY_CAST(Longitude AS FLOAT)) - RADIANS(@userLongitude)) 
                             + SIN(RADIANS(@userLatitude)) * SIN(RADIANS(TRY_CAST(Latitude AS FLOAT)))
                         ) < 1
                     ) THEN 
                         CAST(ROUND(6371 * ACOS(
                             COS(RADIANS(@userLatitude)) * COS(RADIANS(TRY_CAST(Latitude AS FLOAT))) 
                             * COS(RADIANS(TRY_CAST(Longitude AS FLOAT)) - RADIANS(@userLongitude)) 
                             + SIN(RADIANS(@userLatitude)) * SIN(RADIANS(TRY_CAST(Latitude AS FLOAT)))
                         ) * 1000, 0) AS VARCHAR(10)) + 'm'
                     ELSE
                         CAST(ROUND(6371 * ACOS(
                             COS(RADIANS(@userLatitude)) * COS(RADIANS(TRY_CAST(Latitude AS FLOAT))) 
                             * COS(RADIANS(TRY_CAST(Longitude AS FLOAT)) - RADIANS(@userLongitude)) 
                             + SIN(RADIANS(@userLatitude)) * SIN(RADIANS(TRY_CAST(Latitude AS FLOAT)))
                         ), 1) AS VARCHAR(10)) + 'km'
                 END AS formatted_distance
             FROM Credenciamento
             INNER JOIN Categoria ON Categoria.IdCategoria = Credenciamento.IdCategoria
             INNER JOIN Cidade ON Cidade.IdCidade = Credenciamento.IdCidade
             INNER JOIN Estado ON Estado.IdEstado = Cidade.IdEstado
             WHERE 
                 TRY_CAST(Latitude AS FLOAT) IS NOT NULL 
                 AND TRY_CAST(Longitude AS FLOAT) IS NOT NULL 
                 AND 6371 * ACOS(
                     COS(RADIANS(@userLatitude)) * COS(RADIANS(TRY_CAST(Latitude AS FLOAT))) 
                     * COS(RADIANS(TRY_CAST(Longitude AS FLOAT)) - RADIANS(@userLongitude)) 
                     + SIN(RADIANS(@userLatitude)) * SIN(RADIANS(TRY_CAST(Latitude AS FLOAT)))
                 ) <= @radius
             ORDER BY distance ASC;";

        var parameters = new
        {
            userLatitude = latitude,
            userLongitude = longitude,
            radius = raio
        };

        var result = _dbConnection.Query(sql, parameters).ToList();

        return result;
    }
}
