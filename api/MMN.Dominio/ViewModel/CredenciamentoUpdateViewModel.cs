using FluentValidation;

namespace MMN.Dominio.ViewModel
{
    public class CredenciamentoUpdateViewModel
    {
        public string Cep { get; set; }
        public string Email { get; set; }
        public string Bairro { get; set; }
        public string Rua { get; set; }
        public string Numero { get; set; }
        public string Telefone { get; set; }
        public string CelularContato { get; set; }
        public string NomeResponsavel { get; set; }
        public string Estabelecimento { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string ImageBase64 { get; set; }
        public string Complemento { get; set; }
        public string Cnpj { get; set; }
        public int IdCidade { get; set; }
        public int IdEstado { get; set; }
        public int IdCategoria { get; set; }
        public decimal PercentualCashback { get; set; }
        public string BreveDescricao { get; set; }
        public string DescricaoCompleta { get; set; }
        public bool AceitaPgtoComSaldo { get; set; }
        public bool ScanGo { get; set; }
        public int? IdEcossistema { get; set; }
    }


    public class CredenciamentoUpdateViewModelValidator : AbstractValidator<CredenciamentoUpdateViewModel>
    {
        public CredenciamentoUpdateViewModelValidator()
        {
            RuleFor(c => c.Cep).NotEmpty().Must(TamanhoCepSemMascara).WithMessage("cep_invalido");
            RuleFor(c => c.Email).NotEmpty().WithMessage("email_requerido").EmailAddress().WithMessage("email_invalido");
            RuleFor(c => c.Bairro).NotEmpty().WithMessage("bairro_requerido");
            RuleFor(c => c.Rua).NotEmpty().WithMessage("rua_requerida");
            RuleFor(c => c.Numero).NotEmpty().WithMessage("numero_requerido");
            RuleFor(c => c.Telefone).NotEmpty().WithMessage("telefone_requerido");
            RuleFor(c => c.Estabelecimento).NotEmpty().WithMessage("estabelecimento_requerido");
            RuleFor(c => c.Latitude).NotEmpty().WithMessage("latitude_requerida");
            RuleFor(c => c.Longitude).NotEmpty().WithMessage("longitude_requerida");
        }

        private bool TamanhoCepSemMascara(string text)
        {
            return text.Replace("-", "").Length == 8;
        }
    }
}
