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

public interface ICategoriesService
{
    IEnumerable<object> GetFeaturedCategories();
}

public class CategoriesService : ICategoriesService
{
    private readonly IDbConnection _dbConnection;
    private readonly ICategoriaNegocio _categoriaNegocio;

    public CategoriesService(IDbConnection dbConnection, ICategoriaNegocio categoriaNegocio)
    {
        _dbConnection = dbConnection;
        _categoriaNegocio = categoriaNegocio;
    }

    public IEnumerable<object> GetFeaturedCategories()
    {
        const string sql = @"
            WITH AnunciantesPorCategoria AS (
                SELECT 
                    c.IdCategoria,             
                    COUNT(a.IdAnunciante) AS TotalAnunciantes
                FROM 
                    Categoria c
                LEFT JOIN 
                    CategoriaAnunciante ca ON c.IdCategoria = ca.IdCategoria
                LEFT JOIN 
                    Anunciante a ON ca.IdAnunciante = a.IdAnunciante
	            WHERE
		            c.Ativo = 1 AND C.Destaque = 1
                GROUP BY 
                    c.IdCategoria
                HAVING 
                    COUNT(a.IdAnunciante) > 0
            ),
            CredenciamentosPorCategoria AS (
                SELECT 
                    c.IdCategoria,             
                    COUNT(cr.IdCredenciamento) AS TotalCredenciamentos
                FROM 
                    Categoria c
                LEFT JOIN 
                    Credenciamento cr ON c.IdCategoria = cr.IdCategoria
	            WHERE
		            c.Ativo = 1 AND C.Destaque = 1
                GROUP BY 
                    c.IdCategoria
            )

            SELECT 
                c.IdCategoria,   
                c.Nome,
	            c.Icone,
                COALESCE(a.TotalAnunciantes, 0) AS TotalAnunciantes,
                COALESCE(cr.TotalCredenciamentos, 0) AS TotalCredenciamentos,
	            COALESCE(a.TotalAnunciantes, 0) + COALESCE(cr.TotalCredenciamentos, 0) AS Total
            FROM 
                Categoria c
            LEFT JOIN 
                AnunciantesPorCategoria a ON c.IdCategoria = a.IdCategoria
            LEFT JOIN 
                CredenciamentosPorCategoria cr ON c.IdCategoria = cr.IdCategoria
            WHERE
	            c.Ativo = 1 AND C.Destaque = 1
            ORDER BY 
                COALESCE(a.TotalAnunciantes, 0) + COALESCE(cr.TotalCredenciamentos, 0) DESC, c.Nome;
        ";

        var result = _dbConnection.Query(sql).ToList();

        return result;
    }
}
