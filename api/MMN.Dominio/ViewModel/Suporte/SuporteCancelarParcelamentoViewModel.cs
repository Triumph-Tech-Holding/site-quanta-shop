using FluentValidation;
using MMN.Util.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace MMN.Dominio.ViewModel.Suporte
{
    public class SuporteCancelarParcelamentoViewModel
    {
        public int IdSuporte { get; set; }
        public DateTime? DataCompra { get; set; }
        public string CodigoPedido { get; set; }
        public string IdPedido { get; set; }
        public decimal ValorPedido { get; set; }
        public string Observacao { get; set; }
        public DateTime? DataSolicitacao { get; set; }
        public DateTime? DataAtualizacao { get; set; }
        public string ObservacaoAdmin { get; set; }
        public TipoContatoEnum TipoContato { get => TipoContatoEnum.CancelamentoParcelas; }
        public Guid? IdUsuario { get; set; }
        public int? IdStatus { get; set; }
        public Guid? IdUsuarioAcao { get; set; }
        public UsuarioViewModel Usuario { get; set; }
        public StatusViewModel Status { get; set; }
        public UsuarioViewModel UsuarioAcao { get; set; }
        public virtual ICollection<SuporteLogViewModel> SuporteLog { get; set; }
    }
    
    public class SuporteCancelarParcelaViewModel : AbstractValidator<SuporteCancelarParcelamentoViewModel>
    {
        public SuporteCancelarParcelaViewModel()
        {
            RuleFor(c => c.IdPedido).NotEmpty().When(c => c.TipoContato == TipoContatoEnum.CashbackNaoPago || c.TipoContato == TipoContatoEnum.CancelamentoParcelas || c.TipoContato == TipoContatoEnum.ReaberturaParcelas).WithMessage("Informe o código do pedido.");
            RuleFor(c => c.TipoContato).NotNull().IsInEnum().WithMessage("Informe o tipo de contato");
        }
    }
}
