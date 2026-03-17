using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Diagnostics;
using MMN.Dominio.Model;
using MMN.IRepositorio.Repositorio;
using MMN.Repositorio.Base;
using MMN.Repositorio.Contexto;

namespace MMN.Repositorio.Repositorio
{
    public class StatusRepositorio : BaseRepositorio<Status>, IStatusRepositorio
    {
        public StatusRepositorio(DatabaseContext ctx) : base(ctx)
        {
        }
    }
}
