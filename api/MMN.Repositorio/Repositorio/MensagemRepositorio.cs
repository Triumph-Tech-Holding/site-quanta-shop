using Microsoft.EntityFrameworkCore;
using MMN.Dominio.Model;
using MMN.IRepositorio.Repositorio;
using MMN.Repositorio.Base;
using MMN.Repositorio.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MMN.Repositorio.Repositorio
{
    public class MensagemRepositorio : BaseRepositorio<Mensagem>, IMensagemRepositorio
    {
        public MensagemRepositorio(DatabaseContext ctx) : base(ctx)
        {
        }

        public IList<Mensagem> BuscarComunicados()
        {
            return _ctx.Mensagem.Where(m => m.TipoMensagem == Util.Enum.TipoMensagem.Comunicado).Include("MensagemGraduacao").ToList();
        }

        public IList<Mensagem> BuscarPorIdUsuario(Guid idUsuario)
        {
            throw new NotImplementedException();
        }

        public IList<Mensagem> BuscarPorIdUsuarioETipo(Guid idUsuario, long idTipo)
        {
            throw new NotImplementedException();
        }
    }
}
