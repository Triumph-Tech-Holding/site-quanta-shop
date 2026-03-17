using AutoMapper;
using Mapster;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MMN.Dominio.Model;
using MMN.Dominio.ViewModel;
using MMN.INegocio.Negocio;
using MMN.IRepositorio.Repositorio;
using MMN.Negocio.Base;
using MMN.Repositorio.Repositorio;
using MMN.Util.Cache;
using MMN.Util.Jwt;
using MMN.Util.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMN.Negocio.Negocio
{
    public class PreCadastroNegocio : IPreCadastroNegocio
    {
        private readonly ICupomCashbackNegocio _cupomCashbackNegocio;
        private readonly IUsuarioNegocio _usuarioNegocio;
        private readonly IMapper _mapper;
        private readonly ICredenciamentoNegocio _credenciamentoNegocio;

        public PreCadastroNegocio(
            ICupomCashbackNegocio cupomCashbackNegocio,
            IUsuarioNegocio usuarioNegocio,
            IMapper mapper,
            ICredenciamentoNegocio credenciamentoNegocio)
        {
            _cupomCashbackNegocio = cupomCashbackNegocio;
            _usuarioNegocio = usuarioNegocio;
            _credenciamentoNegocio = credenciamentoNegocio;
        }

        public Usuario RegistrarFacilitado(UsuarioCadastroFacilitadoViewModel model)
        {
            try
            {
                var usuario = _usuarioNegocio.RegistrarFacilitado(model);
                var credeciamento = _credenciamentoNegocio.FirstNoTracking(c => c.IdUsuario == usuario.IdUsuarioPai);
                var usuarioEstabelecimentoVm = _usuarioNegocio.FirstNoTracking(u => u.IdUsuario == credeciamento.IdUsuario);
                var usuarioEstabelecimento = usuarioEstabelecimentoVm.Adapt<Usuario>();

                if (model.ValorCompra > 0 && !string.IsNullOrEmpty(model.ComprovanteCompra))
                    _cupomCashbackNegocio.CriarCuponCadastroFacilitadoAsync(model.ValorCompra, model.ComprovanteCompra, usuarioEstabelecimento, usuario);

                return usuario;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
