using MMN.Dominio.Model;
using MMN.Repositorio.Base;
using MMN.IRepositorio.Repositorio;
using System;
using System.Collections.Generic;
using System.Text;
using MMN.Repositorio.Contexto;

namespace MMN.Repositorio.Repositorio
{
    public class TutorialRepositorio : BaseRepositorio<Tutorial>, ITutorialRepositorio
    {
        public TutorialRepositorio(DatabaseContext ctx) : base(ctx)
        {
        }
    }
}
