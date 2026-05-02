using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MMN.Api.Helpers;
using MMN.Dominio.Excecao;
using MMN.Dominio.ViewModel;
using MMN.Util.Util;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MMN.Api.Controllers.v1
{
    public partial class UsuarioController
    {
        [HttpPost]
        [Route("atualizarDadosUsuario")]
        public async Task<IActionResult> AtualizarDadosUsuario([FromBody] UsuarioCadastroCompletoViewModel model)
        {
            var valid = new UsuarioCadastroCompletoViewModelValidator();
            var result = valid.Validate(model);
            if (!result.IsValid)
                throw new AggregateException(result.Errors.Select(s => new PadraoException(s.ErrorMessage)));

            if (!string.IsNullOrEmpty(model.NovaSenha))
            {
                if (model.NovaSenha == model.NovaSenhaConfirma)
                {
                    if (!_negocio.VerificarSenha(model.IdUsuario, model.SenhaAntiga))
                        throw new UnauthorizedException("senha_incorreta");
                }
                else
                    throw new PadraoException("senha_nao_confere");
            }

            var image = string.Empty;
            if (!string.IsNullOrEmpty(model.Imagem))
                image = await AzureStorage.CreateBlob(model.Imagem, IdUsuarioLogado, _appSettings.StorageAccountConnectionString);

            var dadosUsuario = new UsuarioViewModel
            {
                IdUsuario = IdUsuarioLogado,
                Nome = model.Nome,
                Senha = model.NovaSenha,
                Celular = model.Celular,
                AssinaturaEletronica = model.AssinaturaEletronica,
                UrlImg = image,
                NomeSocial = model.NomeSocial,
                Login = model.Login,
                Genero = model.Genero,
                DataNascimento = model.DataNascimento,
            };
            _negocio.AtualizarDados(dadosUsuario);

            var enderecoUsuario = new UsuarioEnderecoViewModel
            {
                IdCidade = model.IdCidade,
                IdUsuario = IdUsuarioLogado,
                Rua = model.Rua,
                Numero = model.Numero,
                Bairro = model.Bairro,
                Cep = model.Cep,
                Complemento = model.Complemento
            };
            if (!_negocioEndereco.CadastrarEndereco(enderecoUsuario))
                throw new Exception();

            return Ok(new { message = _location.GetTranslation("DadosUsuarioSalvo") });
        }

        [HttpGet]
        [Route("obterDadosPessoais")]
        public IActionResult ObterDadosPessoais()
        {
            return Ok(_negocio.ObterDadosPessoais(IdUsuarioLogado));
        }

        [HttpGet]
        [Route("obterFotoPerfil")]
        public IActionResult ObterFotoPerfil()
        {
            return Ok(_negocio.ObterFotoPerfil(IdUsuarioLogado));
        }

        [HttpGet]
        [Route("obterPerfilPainel")]
        public IActionResult ObterPerfilPainel()
        {
            return Ok(_proceduresRepositorio.spc_obterPerfilPainel(IdUsuarioLogado));
        }

        [HttpGet]
        [Route("checkCpfCnpj/{cpfCnpj}")]
        public IActionResult CheckCpfCnpj(string cpfCnpj)
        {
            return Ok(_negocio.ObterDadosPessoaisPorCpfCnpj(cpfCnpj));
        }

        [HttpGet]
        [Route("assinaturaEletronicaAleatoria")]
        public IActionResult AssinaturaEletronicaAleatoria()
        {
            var pass = RandomPassword.Generate(8, 10);
            var usuario = _negocio.AssinaturaEletronicaAleatoria(IdUsuarioLogado, pass);
            if (usuario != null)
            {
                EnviarEmailAssinaturaEletronica(usuario, pass);
                return Ok(new { message = _location.GetTranslation("AssinaturaEletronoicaEnviadaPorEmail") });
            }
            throw new Exception();
        }

        [HttpPost]
        [Route("editarUsuario")]
        public IActionResult EditarUsuario([FromBody] UsuarioEditarViewModel viewModel)
        {
            var valid = new UsuarioEditarViewModelValidator();
            var result = valid.Validate(viewModel);
            if (!result.IsValid)
                throw new AggregateException(result.Errors.Select(s => new PadraoException(s.ErrorMessage)));

            viewModel.IdUsuario = IdUsuarioLogado;

            if (!string.IsNullOrEmpty(viewModel.NovaSenha))
            {
                if (viewModel.NovaSenha == viewModel.NovaSenhaConfirma)
                {
                    if (!_negocio.VerificarSenha(viewModel.IdUsuario, viewModel.SenhaAntiga))
                        throw new UnauthorizedException("senha_incorreta");
                }
                else
                    throw new PadraoException("senha_nao_confere");
            }

            var updateUsuario = _negocio.EditarDadosUsuario(viewModel);
            if (updateUsuario)
            {
                UsuarioViewModel user = _negocio.GetById(IdUsuarioLogado);
                string phone = Regex.Replace(user.Celular, @"\D", "");
                var subscriber = _botConversa.GetSubscriberByPhoneAsync($"55{phone}").Result;
                if (subscriber is not null && user.DataNascimento is not null)
                    _botConversa.SetCustomFieldAsync(subscriber.Id, 4166457, user.DataNascimento.Value.ToString("yyyy-MM-dd"));
                return Ok(new { message = _location.GetTranslation("DadosUsuariosSalvo") });
            }
            throw new Exception();
        }

        [HttpPost]
        [Route("updateImage")]
        public async Task<IActionResult> UpdateImage(UsuarioCadastroCompletoViewModel viewModel)
        {
            if (!string.IsNullOrEmpty(viewModel.Imagem))
            {
                var image = SalvaImagem(viewModel.Imagem, IdUsuarioLogado.ToString());
                if (_negocio.UpdateImage(IdUsuarioLogado, image))
                    return Ok(new { message = _location.GetTranslation("ImageAtualizada"), url = image });
                throw new Exception();
            }
            throw new PadraoException("imagem_requerida");
        }

        [HttpPost]
        [Route("updateAssinaturaEletronica")]
        public IActionResult UpdateAssinaturaEletronica(AssinaturaEletronicaViewModel viewModel)
        {
            var valid = new AssinaturaEletronicaViewModelValidator();
            var result = valid.Validate(viewModel);
            if (result.IsValid)
            {
                if (_negocio.VerificarAssinaturaEletronica(IdUsuarioLogado, viewModel.AssinaturaAtual))
                {
                    if (_negocio.UpdateAssinaturaEletronica(IdUsuarioLogado, viewModel.AssinaturaEletronica))
                        return Ok(new { message = _location.GetTranslation("AssinaturaEletronicaAtualizada") });
                }
                throw new Exception();
            }
            throw new PadraoException("senha_nao_confere");
        }

        [HttpGet]
        [Route("alterarPerfil/{perfil}")]
        public IActionResult AlterarPerfil(char perfil)
        {
            var usuario = _negocio.FirstNoTracking(f => f.IdUsuario == IdUsuarioLogado);
            if (usuario != null)
            {
                usuario.Perfil = perfil;
                _negocio.Update(usuario);
                return Ok();
            }
            throw new NotFoundException("usuario_nao_encontrado");
        }

        [HttpGet]
        [Route("obterPlanoAtivo")]
        public IActionResult ObterPlanoAtivo()
        {
            var result = _usuarioProdutoNegocio.BuscarProdutoAtivo(IdUsuarioLogado);
            var json = JsonConvert.SerializeObject(result, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            return Ok(json);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("primeiraCompra")]
        public async Task<IActionResult> RegistrarFacilitado(UsuarioCadastroFacilitadoViewModel model)
        {
            try
            {
                UsuarioCadastroFacilitadoViewModelValidator validator = new UsuarioCadastroFacilitadoViewModelValidator();
                var result = validator.Validate(model);
                if (!result.IsValid)
                    throw new AggregateException(result.Errors.Select(s => new PadraoException(s.ErrorMessage)));

                string comprovanteBlob = null;
                try
                {
                    if (!string.IsNullOrEmpty(model.ComprovanteCompra))
                    {
                        comprovanteBlob = SalvaImagemComprovante(model.ComprovanteCompra, model.CPF);
                        model.ComprovanteCompra = comprovanteBlob;
                    }

                    var usuario = _preCadastroNegocio.RegistrarFacilitado(model);
                    await EnviarEmailConfirmacao(usuario.IdUsuario, "Quanta Shop - Confirmação de email");

                    var phone = "+55" + usuario.Celular;
                    var username = usuario.Nome.Split(' ');
                    var firstName = username.Length > 0 ? username[0] : string.Empty;
                    var lastName  = username.Length > 1 ? username[1] : string.Empty;
                    if (string.IsNullOrEmpty(firstName)) firstName = usuario.Email;
                    if (string.IsNullOrEmpty(lastName))  lastName  = usuario.Celular;

                    var subscriber = await _botConversa.CreateSubscriberAsync(phone, firstName, lastName);
                    if (subscriber is not null)
                    {
                        await _botConversa.SubscribeCampaignAsync(subscriber.Id, 275782);
                        await _botConversa.SendFlowAsync(subscriber.Id, 7109853);
                        await _botConversa.SetCustomFieldAsync(subscriber.Id, 3683041, usuario.Email);
                        string documento = Regex.Replace(usuario.Documento, @"\D", "");
                        if (documento.Length == 11) await _botConversa.SetCustomFieldAsync(subscriber.Id, 3683018, documento);
                        if (documento.Length == 14) await _botConversa.SetCustomFieldAsync(subscriber.Id, 3683017, documento);
                        await _botConversa.SetCustomFieldAsync(subscriber.Id, 4165889, $"https://quantashop.com.br/register/{usuario.Login}");
                        await _botConversa.AddTagToSubscriberAsync(subscriber.Id, 14784191);
                    }
                    return Ok(usuario.IdUsuario);
                }
                catch
                {
                    if (comprovanteBlob != null) ApagaImagemComprovante(comprovanteBlob);
                    throw;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
