using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Linq;
using Dapper;
using Microsoft.EntityFrameworkCore;
using MMN.Dominio.Model;
using MMN.Dominio.ViewModel;
using MMN.IRepositorio.Repositorio;
using MMN.Repositorio.Base;
using MMN.Repositorio.Contexto;
using MMN.Util.Enum;

namespace MMN.Repositorio.Repositorio
{
    public class CategoriaRepositorio : BaseRepositorio<Categoria>, ICategoriaRepositorio
    {
        public CategoriaRepositorio(DatabaseContext ctx) : base(ctx)
        {
        }

        public List<Categoria> BuscarAtivos(string chavePai)
        {
            if(chavePai == "ASS")
            {
                return _ctx.Categoria.Where(c => c.Ativo && c.Nome == "Assinatura").Include("Produtos").ToList();
            }
            else
            {
                return _ctx.Categoria.Where(c => c.Ativo && c.CategoriaPai.Chave.Equals(chavePai)).Include("Produtos").ToList();
            }
            
        }

        public List<Categoria> BuscarCategorias()
        {
            return _ctx.Categoria.Include(c => c.CategoriaPai).ToList();   
        }

        public List<Credenciamento> GetCredenciamentosParaCategorias(int id)
        {
            return _ctx.Credenciamento.Include(x => x.Usuario).Where(c => c.IdCategoria == id && c.Usuario.Ativo).ToList();
        }

        public List<Credenciamento> GetStatusCredenciamentos(List<long?> idCredenciamentos)
        {
            return _ctx.Credenciamento.Include(x => x.Usuario).Where(c => c.Usuario.Ativo && idCredenciamentos.Contains(c.IdCredenciamento)).ToList();
        }

        public List<CategoriaViewModel> ObterCategorias(FiltroHomeCategoriaViewModel filtro)
        {
            var connectionString = ConexaoDataBase.Connection();

            using var connection = new SqlConnection(connectionString);
            connection.Open();

            var query = @"
                        SELECT
                            C.*,
                            (SELECT ISNULL(COUNT(1), 0) FROM CategoriaAnunciante 
                                LEFT OUTER JOIN Anunciante ON Anunciante.IdAnunciante = CategoriaAnunciante.IdAnunciante
                                WHERE CategoriaAnunciante.IdCategoria = C.IdCategoria AND Anunciante.Ativo = 1)  +
                            (SELECT ISNULL(COUNT(1), 0) FROM Credenciamento WHERE IdCategoria = C.IdCategoria AND Status = 2) AS [TotalCadastros]
                        FROM Categoria AS C
                        WHERE C.IdCategoriaPai = 3 AND C.Ativo = 1 AND C.Nome LIKE '%' + ISNULL(@NomeCategoria, C.Nome)  + '%'
                        ORDER BY C.Nome";

            return connection.Query<CategoriaViewModel>(query, new { NomeCategoria = string.IsNullOrEmpty(filtro.Nome) ? null : filtro.Nome }).ToList();
        }

        public List<CategoriaAnunciante> ObterMapeamentos()
        {
            return _ctx.CategoriaAnunciante
                .Include(c => c.Anunciante)
                .Include(c => c.Categoria)
                .Where(c => !string.IsNullOrEmpty(c.Anunciante.IdAfilio) || !string.IsNullOrEmpty(c.Anunciante.IdAwin))
                .ToList();
        }

        public List<CategoriaAnunciante> ObterStatus()
        {
            return _ctx.CategoriaAnunciante
                .Include(c => c.Anunciante)
                .Include(c => c.Categoria)
                .Where(c => !string.IsNullOrEmpty(c.Anunciante.IdAfilio) || !string.IsNullOrEmpty(c.Anunciante.IdAwin))
                .ToList();
        }
    }
}
