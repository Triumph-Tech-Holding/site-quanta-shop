using Dapper;
using Flurl.Http;
using Mapster;
using Microsoft.Extensions.Caching.Memory;
using MMN.Api.Models.Request;
using MMN.Dominio.Model;
using MMN.INegocio.Negocio;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MMN.Api.Services;

public interface IAwinFeedService
{
    object GetProduct(string id);
    IEnumerable<object> GetRandomProducts(int quantity);
    (IEnumerable<object> Products, int TotalCount, object PriceInterval) GetProducts(int quantity = 12, int page = 1, decimal minPrice = 0, decimal maxPrice = decimal.MaxValue, string order = null, string category = null);
    (IEnumerable<object> Products, int TotalCount, object PriceInterval) GetSearchedProducts(int quantity = 12, int page = 1, decimal minPrice = 0, decimal maxPrice = decimal.MaxValue, string order = null, string searchText = null);
    IEnumerable<object> GetCategories();
    IEnumerable<object> GetBestBrands();
}

public class AwinFeedService : IAwinFeedService
{
    private readonly CacheManager _cache;
    private readonly IDbConnection _dbConnection;

    public AwinFeedService(IDbConnection dbConnection, IMemoryCache cache)
    {
        _dbConnection = dbConnection;
        _cache = new CacheManager(cache);
    }

    public object GetProduct(string id)
    {
        string sql = @"
            SELECT
	            AF.aw_product_id,
	            AF.product_name AS product_name,
	            AF.description,
	            AF.merchant_image_url,
	            UPPER(IIF(AF.brand_name = '', REPLACE(AF.merchant_name, ' BR', ''), REPLACE(AF.brand_name, ' BR', ''))) AS brand_name,
	            AF.category_id,
	            UPPER(AF.category_name) AS category_name,
	            AF.merchant_id,
	            UPPER(REPLACE(AF.merchant_name, ' BR', '')) AS merchant_name,
                (SELECT MAX(Percentual) AS MaxCashback FROM AnuncianteCashback 
		        INNER JOIN Anunciante ON Anunciante.IdAnunciante = AnuncianteCashBack.IdAnunciante
		        WHERE Anunciante.Ativo = 1 AND AnuncianteCashBack.Ativo = 1 AND AnuncianteCashBack.Tipo = 'percentage' AND Anunciante.IdAwin = AF.merchant_id) AS cashback,
	            REPLACE(REPLACE(REPLACE(REPLACE(AF.product_price_old, 'BRL', ''), 'R$ ', ''), '.', ''), ',', '.') AS product_price_old,
	            AF.search_price,
	            AF.aw_deep_link + '&awinaffid=689359' + '&clickref={userId}' AS aw_deep_link
            FROM AwinFeed AS AF
            WHERE AF.aw_product_id = @Id AND AF.data_feed_id NOT IN (48703, 60707, 48705) AND CAST(AF.search_price AS DECIMAL(14,2)) > 0;";

        var queryParameters = new
        {
            Id = id
        };

        return _dbConnection.QueryFirstOrDefault(sql, queryParameters);
    }

    public IEnumerable<object> GetRandomProducts(int quantity)
    {
        string sql = @"
            WITH NumberedFeed AS (
            SELECT 
                AF.aw_product_id,
                AF.product_name,
                AF.description,
                AF.merchant_image_url,
                UPPER(IIF(AF.brand_name = '', REPLACE(AF.merchant_name, ' BR', ''), REPLACE(AF.brand_name, ' BR', ''))) AS brand_name,
                AF.category_id,
                UPPER(AF.category_name) AS category_name,
                AF.merchant_id,
                UPPER(REPLACE(AF.merchant_name, ' BR', '')) AS merchant_name,
                (
                    SELECT MAX(Percentual) 
                    FROM AnuncianteCashback 
                    INNER JOIN Anunciante 
                        ON Anunciante.IdAnunciante = AnuncianteCashBack.IdAnunciante
                    WHERE Anunciante.Ativo = 1 
                      AND AnuncianteCashBack.Ativo = 1 
                      AND AnuncianteCashBack.Tipo = 'percentage' 
                      AND Anunciante.IdAwin = AF.merchant_id
                ) AS cashback,
                REPLACE(REPLACE(REPLACE(REPLACE(AF.product_price_old, 'BRL', ''), 'R$ ', ''), '.', ''), ',', '.') AS product_price_old,
                AF.search_price,
                AF.aw_deep_link + '&awinaffid=689359' + '&clickref={userId}' AS aw_deep_link,
                ROW_NUMBER() OVER (ORDER BY NEWID()) AS RowNum
            FROM AwinFeed AS AF
            WHERE AF.data_feed_id NOT IN (48703, 60707, 48705)
              AND CAST(AF.search_price AS DECIMAL(14, 2)) > 0
        )
        SELECT TOP @quantity *
        FROM NumberedFeed
        WHERE RowNum IS NOT NULL;";

        sql = sql.Replace("@quantity", quantity.ToString());

        return _dbConnection.Query(sql);
    }

    public (IEnumerable<object> Products, int TotalCount, object PriceInterval) GetProducts(int quantity = 12, int page = 1, decimal minPrice = 0, decimal maxPrice = decimal.MaxValue, string order = null, string category = null)
    {
        string categoryFilter = string.IsNullOrEmpty(category) ? "" : "AND AF.category_name LIKE '%' + @Category + '%'";
        string sql = @"

            SELECT
	            COUNT(*)
            FROM AwinFeed AS AF
            WHERE data_feed_id NOT IN (48703, 60707, 48705)
            AND CAST(AF.search_price AS DECIMAL(14,2)) BETWEEN @MinPrice AND @MaxPrice
            " + categoryFilter + @";
            
            SELECT TOP 1
            MIN(CAST(AF.search_price AS DECIMAL(14,2))) AS minValue,
            MAX(CAST(AF.search_price AS DECIMAL(14,2))) AS maxValue
            FROM AwinFeed AS AF;
                
            SELECT
	            AF.aw_product_id,
	            AF.product_name AS product_name,
	            AF.description,
	            AF.merchant_image_url,
	           UPPER(IIF(AF.brand_name = '', REPLACE(AF.merchant_name, ' BR', ''), REPLACE(AF.brand_name, ' BR', ''))) AS brand_name,
	            AF.category_id,
	            UPPER(AF.category_name) AS category_name,
	            AF.merchant_id,
	            UPPER(REPLACE(AF.merchant_name, ' BR', '')) AS merchant_name,
                (SELECT MAX(Percentual) AS MaxCashback FROM AnuncianteCashback 
		        INNER JOIN Anunciante ON Anunciante.IdAnunciante = AnuncianteCashBack.IdAnunciante
		        WHERE Anunciante.Ativo = 1 AND AnuncianteCashBack.Ativo = 1 AND AnuncianteCashBack.Tipo = 'percentage' AND Anunciante.IdAwin = AF.merchant_id) AS cashback,
	            REPLACE(REPLACE(REPLACE(REPLACE(AF.product_price_old, 'BRL', ''), 'R$ ', ''), '.', ''), ',', '.') AS product_price_old,
	            AF.search_price,
	            AF.aw_deep_link + '&awinaffid=689359' + '&clickref={userId}' AS aw_deep_link
            FROM AwinFeed AS AF
            WHERE AF.data_feed_id NOT IN (48703, 60707, 48705)
		    AND CAST(AF.search_price AS DECIMAL(14,2)) > 0
            AND CAST(AF.search_price AS DECIMAL(14,2)) BETWEEN @MinPrice AND @MaxPrice
            " + categoryFilter + @"
            {orderBy} 
            OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;";

        var offset = (page - 1) * quantity;

        var queryParameters = new
        {
            Offset = offset,
            PageSize = quantity,
            MinPrice = minPrice,
            MaxPrice = maxPrice,
            Category = category
        };

        switch (order)
        {
            case "default-sorting":
                sql = sql.Replace("{orderBy}", "ORDER BY AF.aw_product_id");
                break;

            case "low-to-high":
                sql = sql.Replace("{orderBy}", "ORDER BY CAST(AF.search_price AS DECIMAL(14,2)) ASC");
                break;

            case "high-to-low":
                sql = sql.Replace("{orderBy}", "ORDER BY CAST(AF.search_price AS DECIMAL(14,2)) DESC");
                break;
            default:
                sql = sql.Replace("{orderBy}", "ORDER BY NEWID()");
                break;
        }

        using (var multi = _dbConnection.QueryMultiple(sql, queryParameters))
        {
            var totalCount = multi.ReadSingle<int>();
            var priceInterval = multi.ReadSingle<object>();
            var products = multi.Read<object>();

            return (products, totalCount, priceInterval);
        }
    }

    public (IEnumerable<object> Products, int TotalCount, object PriceInterval) GetSearchedProducts(int quantity = 12, int page = 1, decimal minPrice = 0, decimal maxPrice = decimal.MaxValue, string order = null, string searchText = null)
    {
        string searchTextCredenciamentoFilter = string.IsNullOrEmpty(searchText) ? "" : " AND Estabelecimento COLLATE Latin1_General_CI_AI LIKE '%' + ISNULL(@SearchText, Estabelecimento) + '%'";
        string searchTextAnuncianteFilter = string.IsNullOrEmpty(searchText) ? "" : " AND Nome COLLATE Latin1_General_CI_AI LIKE '%' + ISNULL(@SearchText, Nome) + '%'";
        string searchTextFilter = string.IsNullOrEmpty(searchText) ? "" : " AND product_name COLLATE Latin1_General_CI_AI LIKE '%' + ISNULL(@SearchText, product_name) + '%'";

        string sqlCount = @" 
            SELECT COUNT(*)	AS Total
            FROM Credenciamento AS C
            WHERE 1 = 1 AND C.Status = 2" + searchTextCredenciamentoFilter + @"

            UNION ALL

            SELECT COUNT(*)	AS Total
            FROM Anunciante AS A
            WHERE 1 = 1 AND A.Ativo = 1 " + searchTextAnuncianteFilter + @"
    
            UNION ALL            

            SELECT
	            COUNT(*) AS Total
            FROM AwinFeed AS AF
            WHERE data_feed_id NOT IN (48703, 60707, 48705)" + searchTextFilter;

        var countResult = _dbConnection.Query(sqlCount, new { MinPrice = minPrice, MaxPrice = maxPrice, SearchText = searchText }).ToList().Sum(x => x.Total);

        string sqlMinMax = @"
            SELECT TOP 1
                MIN(CAST(AF.search_price AS DECIMAL(14,2))) AS minValue,
                MAX(CAST(AF.search_price AS DECIMAL(14,2))) AS maxValue
            FROM AwinFeed AS AF;";

        var minMaxResult = _dbConnection.QueryFirstOrDefault(sqlMinMax, new { });

        string sql = @"
            SELECT *, TOTAL_ROWS = COUNT(*) OVER() FROM (
	            SELECT 
		            C.IdCredenciamento AS aw_product_id,
		            UPPER(C.Estabelecimento) AS product_name,
		            C.DescricaoCompleta AS description,
		            C.LogoUrl AS merchant_image_url,
		            'PARCEIRO LOCAL' AS brand_name,
		            C.IdCategoria AS category_id,
		            UPPER(Categoria.Nome) AS category_name,
		            '' AS merchant_id,
		            '' AS merchant_name,
                    C.PercentualCashback AS cashback,
		            '' AS product_price_old,
		            C.PercentualCashback  AS search_price,
		            'https://quantashop.com.br/anunciante?idAnunciante=' + CAST(C.IdCredenciamento AS VARCHAR) AS aw_deep_link,
		            'LOCAL' AS type
	            FROM Credenciamento AS C
	            INNER JOIN Categoria ON Categoria.IdCategoria = C.IdCategoria
	            WHERE
		            1 = 1
		            AND C.Status = 2
		            --AND	LOWER(C.Estabelecimento) COLLATE Latin1_General_CI_AI LIKE '%' + ISNULL(@SearchText, C.Estabelecimento) + '%'

	            UNION ALL

	            SELECT 
		            A.IdAnunciante AS aw_product_id,
		            UPPER(A.Nome) AS product_name,
		            '' AS description,
		            A.ImagemUrl AS merchant_image_url,
		            'PARCEIRO ONLINE' AS brand_name,
		            '' AS category_id,
		            (SELECT TOP 1 UPPER(C.Nome)
		             FROM Categoria C
		             INNER JOIN CategoriaAnunciante CA ON CA.IdCategoria = C.IdCategoria
		             WHERE CA.IdAnunciante = A.IdAnunciante) AS category_name,
		            '' AS merchant_id,
		            '' AS merchant_name,
                    (SELECT MAX(Percentual) AS MaxCashback FROM AnuncianteCashback WHERE Ativo = 1 AND Tipo = 'percentage' AND IdAnunciante = A.IdAnunciante) AS cashback,
		            '' AS product_price_old,
		            (SELECT MAX(Percentual) AS MaxCashback FROM AnuncianteCashback WHERE Ativo = 1 AND Tipo = 'percentage' AND IdAnunciante = A.IdAnunciante) AS search_price,
		            'https://www.awin1.com/cread.php?awinaffid=689359&awinmid=' + A.IdAwin + '&clickref={userId}' AS aw_deep_link,
		            'ONLINE' AS type
	            FROM Anunciante AS A
	            WHERE
		            1 = 1
		            AND A.Ativo = 1
		            --AND	LOWER(A.Nome) COLLATE Latin1_General_CI_AI LIKE '%' + ISNULL(@SearchText, A.Nome) + '%'

	            UNION ALL

	            SELECT
		            AF.aw_product_id,
		            UPPER(AF.product_name) AS product_name,
		            AF.description,
		            AF.merchant_image_url,
		            UPPER(IIF(AF.brand_name = '', REPLACE(AF.merchant_name, ' BR', ''), REPLACE(AF.brand_name, ' BR', ''))) AS brand_name,
		            AF.category_id,
		            UPPER(AF.category_name) AS category_name,
		            AF.merchant_id,
		            UPPER(REPLACE(AF.merchant_name, ' BR', '')) AS merchant_name,
                    (SELECT MAX(Percentual) AS MaxCashback FROM AnuncianteCashback 
		            INNER JOIN Anunciante ON Anunciante.IdAnunciante = AnuncianteCashBack.IdAnunciante
		            WHERE Anunciante.Ativo = 1 AND AnuncianteCashBack.Ativo = 1 AND AnuncianteCashBack.Tipo = 'percentage' AND Anunciante.IdAwin = AF.merchant_id) AS cashback,
		            REPLACE(REPLACE(REPLACE(REPLACE(AF.product_price_old, 'BRL', ''), 'R$ ', ''), '.', ''), ',', '.') AS product_price_old,
		            AF.search_price,
		            AF.aw_deep_link + '&awinaffid=689359' + '&clickref={userId}' AS aw_deep_link,
		            'PRODUCT' AS type
	            FROM AwinFeed AS AF
	            WHERE 
		            1 = 1
		            AND AF.data_feed_id NOT IN (48703, 60707, 48705)
		            AND CAST(AF.search_price AS DECIMAL(14,2)) > 0
		            --AND LOWER(AF.product_name) COLLATE Latin1_General_CI_AI LIKE '%' + ISNULL(@SearchText, AF.product_name) + '%'
        ) AS R
        WHERE 
	        1 = 1
	        --AND CAST(search_price AS DECIMAL(14,2)) BETWEEN @MinPrice AND @MaxPrice
	        AND product_name COLLATE Latin1_General_CI_AI LIKE '%' + ISNULL(@SearchText, product_name) + '%'
        {orderBy}
        OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;";

        var offset = (page - 1) * quantity;

        var queryParameters = new
        {
            Offset = offset,
            PageSize = quantity,
            MinPrice = minPrice,
            MaxPrice = maxPrice,
            SearchText = searchText
        };

        switch (order)
        {
            case "default-sorting":
                sql = sql.Replace("{orderBy}", @"
            ORDER BY 
                CASE 
                    WHEN type = 'ONLINE' THEN 1 
                    WHEN type = 'LOCAL' THEN 2 
                    WHEN type = 'PRODUCT' THEN 3 
                    ELSE 4 
                END,
                aw_product_id, 
                product_name");
                break;

            case "low-to-high":
                sql = sql.Replace("{orderBy}", @"
            ORDER BY 
                CASE 
                    WHEN type = 'ONLINE' THEN 1 
                    WHEN type = 'LOCAL' THEN 2 
                    WHEN type = 'PRODUCT' THEN 3 
                    ELSE 4 
                END,
                CAST(search_price AS DECIMAL(14,2)) ASC");
                break;

            case "high-to-low":
                sql = sql.Replace("{orderBy}", @"
            ORDER BY 
                CASE 
                    WHEN type = 'ONLINE' THEN 1 
                    WHEN type = 'LOCAL' THEN 2 
                    WHEN type = 'PRODUCT' THEN 3 
                    ELSE 4 
                END,
                CAST(search_price AS DECIMAL(14,2)) DESC");
                break;

            default:
                sql = sql.Replace("{orderBy}", @"
            ORDER BY 
                CASE 
                    WHEN type = 'ONLINE' THEN 1 
                    WHEN type = 'LOCAL' THEN 2 
                    WHEN type = 'PRODUCT' THEN 3 
                    ELSE 4 
                END,
                aw_product_id DESC");
                break;
        }


        var products = _dbConnection.Query(sql, queryParameters).ToList();

        return (products, countResult, minMaxResult);
    }

    public IEnumerable<object> GetCategories()
    {
        const string sql = @"
            SELECT 
	            AF.category_id,
	            UPPER(AF.category_name) AS category_name,
	            COUNT(*) AS total_categories
            FROM AwinFeed AS AF
            WHERE data_feed_id NOT IN (48703, 60707, 48705)
            GROUP BY AF.category_id, AF.category_name
            ORDER BY AF.category_name";

        var result = _dbConnection.Query(sql, new { });

        return result;
    }

    public IEnumerable<object> GetBestBrands()
    {
        const string sql = @"
            SELECT
                AF.brand_id,
	            UPPER(AF.brand_name) AS brand_name,
	            Anunciante.ImagemUrl AS brand_image_url,
	            COUNT(*) AS total_products
            FROM AwinFeed AS AF
            LEFT OUTER JOIN Anunciante ON Anunciante.Nome = AF.brand_name
            WHERE 
	            1 = 1 
	            AND data_feed_id NOT IN (48703, 60707, 48705) 
	            AND AF.brand_id > 0 
	            AND Anunciante.ImagemUrl <> ''
            GROUP BY AF.brand_id, AF.brand_name, Anunciante.ImagemUrl
            ORDER BY COUNT(*) DESC";

        var result = _dbConnection.Query(sql, new { });

        return result;
    }
}