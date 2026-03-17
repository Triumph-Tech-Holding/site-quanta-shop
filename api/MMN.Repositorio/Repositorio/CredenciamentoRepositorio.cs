using MMN.Dominio.Excecao;
using MMN.Dominio.Model;
using MMN.Dominio.ViewModel;
using MMN.IRepositorio.Repositorio;
using MMN.Repositorio.Base;
using MMN.Repositorio.Contexto;
using MMN.Util.Enum;
using MMN.Util.Extensions;
using MMN.Util.Model;
using MMN.Util.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using static MMN.Dominio.ViewModel.FiltroViewModel;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MMN.Repositorio.Repositorio
{
    public class CredenciamentoRepositorio : BaseRepositorio<Credenciamento>, ICredenciamentoRepositorio
    {
        public CredenciamentoRepositorio(DatabaseContext ctx) : base(ctx)
        {
        }

        public object BuscarPorEstabelecimento(FiltroCredenciamento filtroCredenciamento)
        {

            var listaCredenciamento = Get(c =>
                (string.IsNullOrEmpty(filtroCredenciamento.Estabelecimento) || c.Estabelecimento.Contains(filtroCredenciamento.Estabelecimento)) &&
                (string.IsNullOrEmpty(filtroCredenciamento.Patrocinador) || c.UsuarioPai.Login.Contains(filtroCredenciamento.Patrocinador)) &&
                (string.IsNullOrEmpty(filtroCredenciamento.Login) || c.Usuario.Login.Contains(filtroCredenciamento.Login)) &&
                (!filtroCredenciamento.Status.HasValue || filtroCredenciamento.Status == c.Status) &&
                (!filtroCredenciamento.IdCategoria.HasValue || filtroCredenciamento.IdCategoria == c.IdCategoria), "Cidade.Estado", "Categoria", "Usuario", "UsuarioPai", "Ecossistema").Select(s => new
                {
                    s.IdCredenciamento,
                    s.Estabelecimento,
                    s.Latitude,
                    s.Longitude,
                    s.PercentualCashback,
                    s.IdCidade,
                    s.Email,
                    Cidade = s.Cidade.Nome,
                    s.Cidade.Estado.Uf,
                    s.Cidade.Estado.IdEstado,
                    s.Categoria.IdCategoria,
                    Categoria = s.Categoria.Nome,
                    s.Rua,
                    s.Bairro,
                    s.Numero,
                    s.Complemento,
                    s.Cep,
                    s.Telefone,
                    s.Status,
                    s.Cnpj,
                    s.DataAtivacao,
                    s.DataCadastro,
                    s.LogoUrl,
                    s.IdEcossistema,
                    NomeEcossistema = s.Ecossistema.Nome,
                    StatusDesc = s.Status.GetDescription(),
                    LoginPatrocinador = s.UsuarioPai.Login,
                    LoginResponsavel = s.Usuario != null ? s.Usuario.Login : "",
                    NomeResponsavel = s.Usuario != null ? s.Usuario.Nome : "",
                    IdUsuario = s.Usuario != null ? s.Usuario.IdUsuario : new Guid(),
                    CelularContato = s.Usuario != null ? s.Usuario.Celular : "",
                });

            var totalPages = 0;

            var credenciamentoFiltrado = listaCredenciamento            
               .OrderByDescending(x => x.DataCadastro)
               .ToList();

            return new { totalPages, filtroCredenciamento.Quantidade, filtroCredenciamento.Page, credenciamentoFiltrado, quantidadeTotal = listaCredenciamento.Count() };
        }

        public async Task<(bool status, string message)> Credenciar(NovoCredenciamentoViewModel credenciamento)
        {
            using (var transaction = _ctx.Database.BeginTransaction())
            {
                try
                {
                    var indicador = _ctx.Usuario.FirstOrDefault(x => x.Login == credenciamento.Indicador);

                    var salt = Hash.Get_SALT();

                    //Usuário
                    var novoUsuario = new Usuario
                    {
                        IdUsuarioPai = indicador.IdUsuario,
                        IdGrupo = 10,
                        Nome = credenciamento.NomeResponsavel.ToUpper(),
                        Email = credenciamento.EmailResponsavel.ToLower(),
                        Login = credenciamento.CPFResponsavel,
                        Senha = Hash.Get_HASH_SHA512(credenciamento.SenhaResponsavel, credenciamento.EmailResponsavel, salt),
                        SaltKey = salt,
                        Documento = credenciamento.CPFResponsavel,
                        Ativo = true,
                        Celular = credenciamento.TelefoneResponsavel,
                        Bloqueado = false,
                        DataBloqueio = null,
                        TentativasIncorretas = 0,
                        AssinaturaEletronica = null,
                        IdGraduacao = 1,
                        Cultura = "pt-BR",
                        PosicaoBinario = 0,
                        DataReferencia = DateTime.Now.HorarioBrasilia(),
                        DataQualificacao = null,
                        DataCadastro = DateTime.Now.HorarioBrasilia(),
                        EmailConfirmado = false,
                        Master = false,
                        UrlImg = "https://bigcash.blob.core.windows.net/imagens-credenciamento/6886c534-1f67-4e5f-8273-c37544ed36622024-05-07-15-19-13.jpeg",
                        EstadoRG = null,
                        OrgaoEmissorRG = null,
                        RG = null,
                        Empreendedor = true,
                        Perfil = 'C',
                        TermosDeAceite = false,
                        DataNascimento = credenciamento.DataNascimento
                    };

                    await _ctx.Usuario.AddAsync(novoUsuario);
                    await _ctx.SaveChangesAsync();

                    var novoCredenciamento = new Credenciamento
                    {
                        Cnpj = credenciamento.CNPJ,
                        RazaoSocial = credenciamento.RazaoSocial.ToUpper(),
                        Estabelecimento = string.IsNullOrEmpty(credenciamento.NomeFantasia) ? credenciamento.RazaoSocial.ToUpper() : credenciamento.NomeFantasia.ToUpper(),
                        FaturamentoMensal = 0,
                        Rua = credenciamento.Logradouro.ToUpper(),
                        Numero = credenciamento.Numero,
                        Bairro = credenciamento.Bairro.ToUpper(),
                        Cep = credenciamento.CEP,
                        Complemento = credenciamento.Complemento.ToUpper(),
                        Telefone = credenciamento.TelefoneEmpresa,
                        IdCategoria = credenciamento.IdCategoria,
                        IdCidade = credenciamento.IdCidade,
                        IdUsuario = novoUsuario.IdUsuario,
                        IdUsuarioPai = novoUsuario.IdUsuarioPai,
                        Email = credenciamento.EmailResponsavel.ToLower(),
                        Latitude = credenciamento.Latitude.ToString(),
                        Longitude = credenciamento.Longitude.ToString(),
                        LogoUrl = string.IsNullOrEmpty(credenciamento.Logomarca) ? "https://bigcash.blob.core.windows.net/imagens-credenciamento/6886c534-1f67-4e5f-8273-c37544ed36622024-05-07-15-19-13.jpeg" : credenciamento.Logomarca,
                        PercentualCashback = credenciamento.PercentualCashback,
                        Status = StatusCredenciamento.Pendente,
                        MotivoRecusa = null,
                        DataAtualizacao = null,
                        DataCadastro = DateTime.Now.HorarioBrasilia(),
                        DataAtivacao = null,
                        CelularContato = credenciamento.TelefoneEmpresa,
                        AceitaPgtoComSaldo = true,
                        ScanGo = true
                    };

                    await _ctx.Credenciamento.AddAsync(novoCredenciamento);
                    await _ctx.SaveChangesAsync();

                    //OrdenacaoAnuncio
                    var ordenacaoAnuncio = new OrdenacaoAnuncio
                    {
                        IdCredenciamento = novoCredenciamento.IdCredenciamento,
                        IdAnunciante = null,
                        Ordenacao = 0,
                        ParceiroOnline = false
                    };

                    await _ctx.OrdenacaoAnuncio.AddAsync(ordenacaoAnuncio);
                    await _ctx.SaveChangesAsync();

                    transaction.Commit();

                    return (true, "");
                }
                catch (Exception ex)
                {
                    transaction.Rollback();

                    var message = ex.InnerException is null ? ex.Message : ex.InnerException.Message;
                    return (false, message);
                }
            }
        }
    }
}
