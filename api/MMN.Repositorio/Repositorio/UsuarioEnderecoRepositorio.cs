using Microsoft.EntityFrameworkCore;
using MMN.Dominio.Model;
using MMN.Dominio.ViewModel;
using MMN.IRepositorio.Repositorio;
using MMN.Repositorio.Base;
using MMN.Repositorio.Contexto;
using System;
using System.Linq;

namespace MMN.Repositorio.Repositorio
{
    public class UsuarioEnderecoRepositorio : BaseRepositorio<UsuarioEndereco>, IUsuarioEnderecoRepositorio
    {
        public UsuarioEnderecoRepositorio(DatabaseContext ctx) : base(ctx)
        {
        }

        public UsuarioEndereco ObterEnderecoCompleto(Guid idUsuario)
        {
            var endereco = _ctx.UsuarioEndereco
                .Include(e => e.Cidade)
                    .ThenInclude(c => c.Estado)
                .FirstOrDefault(e => e.IdUsuario == idUsuario);

            return endereco;
        }
    }
}
