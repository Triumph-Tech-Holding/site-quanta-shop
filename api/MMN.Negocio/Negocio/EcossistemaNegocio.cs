using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MMN.Dominio.Model;
using MMN.Dominio.ViewModel;
using MMN.INegocio.Negocio;
using MMN.IRepositorio.Repositorio;
using MMN.Negocio.Base;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using MMN.Dominio.WebHook;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Linq;


namespace MMN.Negocio.Negocio
{
    public class EcossistemaNegocio : BaseNegocio<EcossistemaViewModel, Ecossistema>, IEcossistemaNegocio
    {
        private readonly IEcossistemaRepositorio _repositorio;
        private readonly IMapper _mapper;

        public EcossistemaNegocio(IEcossistemaRepositorio repositorio, IMapper mapper) : base(repositorio, mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Ecossistema>> BuscarEcossistemas(FiltroViewModel.FiltroEcossistemas filtro)
        {
            var query = _repositorio.GetAllNoTracking().AsQueryable();

            if (!string.IsNullOrEmpty(filtro.Nome))
            {
                query = query.Where(e => e.Nome.Contains(filtro.Nome));
            }

            if (filtro.Ativo.HasValue)
            {
                query = query.Where(e => e.Ativo == filtro.Ativo.Value);
            }

            return await query.ToListAsync();

        }

        public async Task<Ecossistema> BuscarEcossistemaPorId(int id)
        {
            return  _repositorio.FirstNoTracking(x => x.IdEcossistema == id);
        }

        public async Task CriarEcossistema(Ecossistema ecossistema)
        {
            _repositorio.Insert(ecossistema);
            await _repositorio.SaveChangesAsync();
        }

        public async Task AtualizarEcossistema(Ecossistema ecossistema)
        {
            var update = await BuscarEcossistemaPorId(ecossistema.IdEcossistema);

            update.Ativo = ecossistema.Ativo;
            update.Nome = ecossistema.Nome;
            update.Regiao = ecossistema.Regiao;
            
            _repositorio.Update(update);
            await _repositorio.SaveChangesAsync();
        }

        public async Task DeletarEcossistema(int id)
        {
            var ecossistema = _repositorio.FirstNoTracking(x => x.IdEcossistema == id);
            if (ecossistema != null)
            {
                _repositorio.Delete(ecossistema.IdEcossistema);
                await _repositorio.SaveChangesAsync();
            }
        }
    }
}
