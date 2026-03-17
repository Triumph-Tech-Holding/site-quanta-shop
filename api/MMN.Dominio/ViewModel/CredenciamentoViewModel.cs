using FluentValidation;
using Microsoft.Identity.Client;
using Microsoft.VisualBasic;
using MMN.Dominio.Model;
using MMN.Util.Enum;
using MMN.Util.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MMN.Dominio.ViewModel
{
    public class CredenciamentoViewModel
    {
        public long IdCredenciamento { get; set; }
        public string Estabelecimento { get; set; }
        public decimal FaturamentoMensal { get; set; }
        public string Email { get; set; }
        public string LogoUrl { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Rua { get; set; }
        public string Numero { get; set; }
        public string Bairro { get; set; }
        public string Estado { get; set; }
        public string Cep { get; set; }
        public string Complemento { get; set; }
        public string Telefone { get; set; }
        public string Cnpj { get; set; }
        public StatusCredenciamento Status { get; set; }
        public decimal? PercentualCashback { get; set; }
        public int IdCidade { get; set; }
        public CidadeViewModel CidadeViewModel { get; set; }
        public int IdCategoria { get; set; }
        public CategoriaViewModel CategoriaViewModel { get; set; }
        public Guid IdUsuarioPai { get; set; }
        public UsuarioViewModel UsuarioPaiViewModel { get; set; }
        public Guid? IdUsuario { get; set; }
        public UsuarioViewModel UsuarioViewModel { get; set; }
        public string LoginPatrocinador { get; set; }
        public string MotivoRecusa { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime? DataAtualizacao { get; set; }
        public DateTime? DataAtivacao { get; set; }
        public string ImageBase64 { get; set; }
        public string NomeResponsavel { get; set; }
        public string SenhaResponsavel { get; set; }
        public string ConfirmSenhaResponsavel { get; set; }
        public string ConfirmEmail { get; set; }
        public string LoginResponsavel { get; set; }
        public string CelularContato { get; set; }
        public string BreveDescricao { get; set; }
        public string DescricaoCompleta { get; set; }
        public bool AceitaPgtoComSaldo { get; set; }
        public bool ScanGo { get; set; }
        public int? IdEcossistema { get; set; } = null;

        public IEnumerable<PedidoViewModel> Pedidos { get; set; }
    }

    public class CredenciamentoViewModelValidator : AbstractValidator<CredenciamentoViewModel>
    {
        public CredenciamentoViewModelValidator()
        {
            RuleFor(c => c.NomeResponsavel).NotEmpty().WithMessage("nome_requerido");
            RuleFor(c => c.Estabelecimento).NotEmpty().WithMessage("estabelecimento_requerido");
            RuleFor(c => c.LoginPatrocinador).NotEmpty().WithMessage("patrocinador_requerido");
            RuleFor(c => c.LoginResponsavel).NotEmpty().WithMessage("login_requerido")
                .Must(RequisitosLogin).WithMessage("login_nao_permitido");
            RuleFor(c => c.Cnpj).Must(UtilBase.IsValidCpfCnpj).WithMessage("cpf_cnpj_invalido");

            RuleFor(c => c.PercentualCashback).NotEmpty().WithMessage("cashback_requerido")
                .Must(PercentualValido).WithMessage("cashback_percentual_minimo");

            RuleFor(c => c.Email).NotEmpty().WithMessage("email_requerido").EmailAddress().WithMessage("email_invalido")
                .Must(UtilBase.EmailDominioValido).WithMessage("email_nao_permitido")
                .Must(UtilBase.EmailCorreto).WithMessage("email_requisito");
            RuleFor(c => c.ConfirmEmail).NotEmpty().WithMessage("email_confirmacao_requerida").Equal(c => c.Email).WithMessage("email_nao_confere");

            RuleFor(c => c.SenhaResponsavel).Must(RequisitosSenha).WithMessage("senha_requisitos");
            RuleFor(c => c.ConfirmSenhaResponsavel).Equal(c => c.SenhaResponsavel).WithMessage("senha_nao_confere");

            RuleFor(c => c.Cep).NotEmpty().Must(TamanhoCepSemMascara).WithMessage("cep_invalido");
            RuleFor(c => c.Bairro).NotEmpty().WithMessage("bairro_requerido");
            RuleFor(c => c.Rua).NotEmpty().WithMessage("rua_requerida");
            RuleFor(c => c.Numero).NotEmpty().WithMessage("numero_requerido");
            RuleFor(c => c.Telefone).NotEmpty().WithMessage("telefone_requerido");
        }

        private bool PercentualValido(decimal? percentual)
        {
            if (percentual == null)
                return false;

            return percentual > 0;
        }

        private bool TamanhoCepSemMascara(string text)
        {
            return text.Replace("-", "").Length == 8;
        }

        public bool RequisitosSenha(string senha)
        {
            if (string.IsNullOrEmpty(senha)) return false;

            if (senha.Length < 8 || !UtilBase.HasDigit(senha) || !UtilBase.HasLetter(senha)) return false;

            return true;
        }

        public bool RequisitosLogin(string login)
        {
            return !login.ToLower().Contains("bigcash")
                && !login.ToLower().Contains("bigcashme")
                && !login.ToLower().Contains("cashme")
                && !login.ToLower().Contains("big")
                && !login.ToLower().Contains("cash")
                && !login.ToLower().Contains("admin")
                && !login.ToLower().Contains("admins");
        }
    }

    public class CredenciarViewModel
    {
        public string Estabelecimento { get; set; }
        public string NomeResponsavel { get; set; }
        public string Email { get; set; }
        public string ConfirmEmail { get; set; }
        public string CelularContato { get; set; }
        public string Documento { get; set; }
        public int IdCategoria { get; set; }
        public decimal? PercentualCashback { get; set; }
        public int IdCidade { get; set; }
        public string Bairro { get; set; }
        public string Cep { get; set; }
        public string Rua { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public string ImageBase64 { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
    }

    public class CredenciarNovoUsuarioViewModel
    {
        public string Estabelecimento { get; set; }
        public string NomeResponsavel { get; set; }
        public string Email { get; set; }
        public string ConfirmEmail { get; set; }
        public string CelularContato { get; set; }
        public string Cnpj { get; set; }
        public int IdCategoria { get; set; }
        public decimal? PercentualCashback { get; set; }
        public int IdCidade { get; set; }
        public string Bairro { get; set; }
        public string Cep { get; set; }
        public string Rua { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string LoginPatrocinador { get; set; }
        public string LoginResponsavel { get; set; }
        public string SenhaResponsavel { get; set; }
        public string ConfirmSenhaResponsavel { get; set; }
        public string Telefone { get; set; }
        public string ImageBase64 { get; set; }
    }

    public class NovoCredenciamentoViewModel
    { 
        //Dados da empresa
        public string CNPJ { get; set; }
        public string RazaoSocial { get; set; }
        public string NomeFantasia { get; set; }
        public string CEP { get; set; }
        public string Logradouro { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public string Bairro { get; set; }
        public int IdEstado { get; set; }
        public int IdCidade { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Logomarca { get; set; }
        
        //Dados do responsável
        public string NomeResponsavel { get; set; }
        public DateTime DataNascimento { get; set; }
        public string TelefoneResponsavel { get; set; }
        public string EmailResponsavel { get; set; }
        public string CPFResponsavel { get; set; }
        public string SenhaResponsavel { get; set; }
       
        //Dados do credenciamento
        public int PercentualCashback { get; set; }
        public string TelefoneEmpresa { get; set; }
        public int IdCategoria { get; set; }
        public string Indicador { get; set; }
    }

    public class CredenciarNovoUsuarioViewModelValidator : AbstractValidator<CredenciarNovoUsuarioViewModel>
    {
        public CredenciarNovoUsuarioViewModelValidator()
        {
            RuleFor(c => c.Estabelecimento).NotEmpty().WithMessage("estabelecimento_requerido");
            RuleFor(c => c.NomeResponsavel).NotEmpty().WithMessage("nome_requerido");
            RuleFor(c => c.Email).NotEmpty().WithMessage("email_requerido")
                .EmailAddress().WithMessage("email_invalido")
                .Must(UtilBase.EmailDominioValido).WithMessage("email_nao_permitido")
                .Must(UtilBase.EmailCorreto).WithMessage("email_requisito");
            RuleFor(c => c.ConfirmEmail).NotEmpty().WithMessage("email_confirmacao_requerida")
                .Equal(c => c.Email).WithMessage("email_nao_confere");
            RuleFor(c => c.CelularContato).NotEmpty().WithMessage("telefone_requerido");
            RuleFor(c => c.Cnpj).Must(UtilBase.IsValidCpfCnpj).WithMessage("cpf_cnpj_invalido");
            RuleFor(c => c.PercentualCashback).NotEmpty().WithMessage("cashback_requerido")
                .GreaterThan(0).WithMessage("cashback_percentual_minimo");
            RuleFor(c => c.Cep).NotEmpty().Must(TamanhoCepSemMascara).WithMessage("cep_invalido");
            RuleFor(c => c.Bairro).NotEmpty().WithMessage("bairro_requerido");
            RuleFor(c => c.Rua).NotEmpty().WithMessage("rua_requerida");
            RuleFor(c => c.Numero).NotEmpty().WithMessage("numero_requerido");
            RuleFor(c => c.Latitude).NotEmpty().WithMessage("latitude_requerida");
            RuleFor(c => c.Longitude).NotEmpty().WithMessage("longitude_requerida");
            RuleFor(u => u.SenhaResponsavel).Must(UtilBase.RequisitosSenha).WithMessage("senha_requisitos")
                .Equal(m => m.ConfirmSenhaResponsavel).WithMessage("senha_nao_confere");
            RuleFor(u => u.LoginResponsavel).MinimumLength(5).WithMessage("login_tamanho_minimo")
                .Must(UtilBase.RequisitosLogin).WithMessage("login_nao_permitido");
            RuleFor(u => u.LoginPatrocinador).NotNull().NotEmpty();
        }

        private bool TamanhoCepSemMascara(string text)
        {
            return text.Where(w => char.IsNumber(w)).Count() == 8;
        }
    }

    public class CredenciarViewModelValidator : AbstractValidator<CredenciarViewModel>
    {
        public CredenciarViewModelValidator()
        {
            RuleFor(c => c.Estabelecimento).NotEmpty().WithMessage("estabelecimento_requerido");
            RuleFor(c => c.NomeResponsavel).NotEmpty().WithMessage("nome_requerido");
            RuleFor(c => c.Email).NotEmpty().WithMessage("email_requerido")
                .EmailAddress().WithMessage("email_invalido")
                .Must(UtilBase.EmailDominioValido).WithMessage("email_nao_permitido")
                .Must(UtilBase.EmailCorreto).WithMessage("email_requisito");
            RuleFor(c => c.ConfirmEmail).NotEmpty().WithMessage("email_confirmacao_requerida")
                .Equal(c => c.Email).WithMessage("email_nao_confere");
            RuleFor(c => c.CelularContato).NotEmpty().WithMessage("telefone_requerido");
            RuleFor(c => c.Documento).Must(UtilBase.IsValidCpfCnpj).WithMessage("cpf_cnpj_invalido");
            RuleFor(c => c.PercentualCashback).NotEmpty().WithMessage("cashback_requerido")
                .GreaterThan(0).WithMessage("cashback_percentual_minimo");
            RuleFor(c => c.Cep).NotEmpty().Must(TamanhoCepSemMascara).WithMessage("cep_invalido");
            RuleFor(c => c.Bairro).NotEmpty().WithMessage("bairro_requerido");
            RuleFor(c => c.Rua).NotEmpty().WithMessage("rua_requerida");
            RuleFor(c => c.Numero).NotEmpty().WithMessage("numero_requerido");
            RuleFor(c => c.Latitude).NotEmpty().WithMessage("latitude_requerida");
            RuleFor(c => c.Longitude).NotEmpty().WithMessage("longitude_requerida");
        }

        private bool TamanhoCepSemMascara(string text)
        {
            return text.Where(w => char.IsNumber(w)).Count() == 8;
        }
    }

    public class CredenciamentoViewModelUpdateValidator : AbstractValidator<CredenciamentoViewModel>
    {
        public CredenciamentoViewModelUpdateValidator()
        {
            RuleFor(c => c.IdCredenciamento).NotNull().WithMessage("credenciamento_id_requerido");
            RuleFor(c => c.Cep).NotEmpty().Must(TamanhoCepSemMascara).WithMessage("cep_invalido");
            RuleFor(c => c.Email).NotEmpty().WithMessage("email_requerido").EmailAddress().WithMessage("email_invalido");
            RuleFor(c => c.Bairro).NotEmpty().WithMessage("bairro_requerido");
            RuleFor(c => c.Rua).NotEmpty().WithMessage("rua_requerida");
            RuleFor(c => c.Numero).NotEmpty().WithMessage("numero_requerido");
            RuleFor(c => c.Telefone).NotEmpty().WithMessage("telefone_requerido");
            RuleFor(c => c.Estabelecimento).NotEmpty().WithMessage("estabelecimento_requerido");
        }
        private bool TamanhoCepSemMascara(string text)
        {
            return text.Replace("-", "").Length == 8;
        }
    }

    public class CredenciamentoViewModelUpdateStatusValidator : AbstractValidator<CredenciamentoViewModel>
    {
        public CredenciamentoViewModelUpdateStatusValidator()
        {
            RuleFor(c => c.IdCredenciamento).NotNull().WithMessage("credenciamento_id_requerido");
            RuleFor(c => c.Status).IsInEnum().WithMessage("status_novo_requerido");
            //RuleFor(c => c.Latitude).NotEmpty().When(c => c.Status == StatusCredenciamento.Aprovado).WithMessage("latitude_requerida");
            //RuleFor(c => c.Longitude).NotEmpty().When(c => c.Status == StatusCredenciamento.Aprovado).WithMessage("longitude_requerida");
            //RuleFor(c => c.ImageBase64).NotEmpty().When(c => c.Status == StatusCredenciamento.Aprovado).WithMessage("logo_requerida");
            //RuleFor(c => c.PercentualCashback).NotNull().When(c => c.Status == StatusCredenciamento.Aprovado).WithMessage("cashback_requerido");
            RuleFor(c => c.MotivoRecusa).NotEmpty().When(c => c.Status == StatusCredenciamento.Reprovado).WithMessage("recusa_motivo_requerido");
        }
    }
}
