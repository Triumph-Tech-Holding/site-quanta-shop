using FluentValidation;
using MMN.Dominio.Model;
using MMN.Util.Enum;
using System;
using System.Collections.Generic;

namespace MMN.Dominio.ViewModel
{
    public class FiltroViewModel
    {
        public class BuscarExtrato
        {
            public string DataInicio { get; set; }
            public string DataFim { get; set; }
            public string Chave { get; set; }
            public Guid? IdUsuario { get; set; }
        }

        public class BuscarPedido
        {
            public string DataInicio { get; set; }
            public string DataFim { get; set; }
            public int? IdStatus { get; set; }
        }

        public class ConfirmarPagamento
        {
            public string CodigoPedido { get; set; }
            public decimal ValorPago { get; set; }
            public DateTime DataPagamento { get; set; }
        }

        public class FiltroLancamento
        {
            public string IdTransacao { get; set; }
            public string Login { get; set; }
            public string Nome { get; set; }
            public int Pagina {  get; set; }
            public int PorPagina {  get; set; }
    }

            public class FiltroUsuario
        {
            public string Login { get; set; }
            public string Nome { get; set; }
            public string Email { get; set; }
            public string Patrocinador { get; set; }
            public bool? Ativo { get; set; }
            public int Quantidade { get; set; }
            public int Page { get; set; }
            public EnumOrdenacaoUsuarios OrdenacaoUsuarios { get; set; }
            public bool OrderDesc { get; set; }
            public int? IdGraduacao { get; set; }
            public string Celular { get; set; }
            public int? IdProduto { get; set; }
        }

        public class FiltroSaque
        {
            public int IdStatus { get; set; }
            public DateTime? DataInicio { get; set; }
            public DateTime? DataFim { get; set; }
            public string Login { get; set; }
            public int Quantidade { get; set; }
            public int Page { get; set; }
        }

        public class FiltroRelatorio
        {
            public int IdStatus { get; set; }
            public DateTime? DataInicio { get; set; }
            public DateTime? DataFim { get; set; }
            public string Login { get; set; }
            public int Pagina { get; set; }
            public int ItensPorPagina { get; set; }
        }

        public class FiltroFaq
        {
            public int idCategoria { get; set; }
            public int Quantidade { get; set; }
            public int Page { get; set; }
            public int Ativo { get; set; }
        }


        public class AnunciantePaginado
        {
            public string Descricao { get; set; }
            public int Quantidade { get; set; }
            public int Page { get; set; }
            public int? IdCategoria { get; set; }
            public List<TipoAnuncianteEnum> TipoAnunciante { get; set; }
        }

        public class PedidoAfiliados
        {
            public DateTime? DataInicio { get; set; }
            public DateTime? DataFim { get; set; }
            public string Descricao { get; set; }
            public int? IdStatus { get; set; }
        }

        public class PedidoAfiliadosAdmin
        {
            public DateTime? DataInicio { get; set; }
            public DateTime? DataFim { get; set; }
            public int? IdStatus { get; set; }
            public string Login { get; set; }
            public string LoginPatrocinador { get; set; }
            public string LoginCredenciado { get; set; }
            public int Page { get; set; }
            public int PerPage { get; set; }
            public bool OrderDesc { get; set; }
            public EnumOrdenacaoComprasAdmin? Ordenacao { get; set; }
        }

        public class PedidoAfiliadosAdminValidator : AbstractValidator<PedidoAfiliadosAdmin>
        {
            public PedidoAfiliadosAdminValidator()
            {
                RuleFor(p => p.DataInicio).LessThan(p => p.DataFim).When(p => p.DataFim.HasValue && p.DataInicio.HasValue).WithMessage("data_inicial_final");
            }
        }

        public class FiltroRank
        {
            public string Login { get; set; }
            public string Ordenacao { get; set; }
        }

        public class FiltroCredenciamento
        {
            public string Estabelecimento { get; set; }
            public string Patrocinador { get; set; }
            public string Login { get; set; }
            public StatusCredenciamento? Status { get; set; }
            public int? IdCategoria { get; set; }
            public int Quantidade { get; set; }
            public int Page { get; set; }

            public EnumOrdenacaoCredenciamento? Ordenacao { get; set; }
            public bool? Asc { get; set; }
        }

        public class FiltroCredenciamentoApp
        {
            public int? IdCategoria { get; set; }
        }

        public class FiltroCredenciamentoContactar
        {
            public long IdCredenciamento { get; set; }
            public bool Contactado { get; set; }
        }

        public class FiltroPedidosAdmin
        {
            public string LoginEmail { get; set; }
            public string CodigoPedido { get; set; }
            public DateTime? DataPedido { get; set; }
            public int? IdStatus { get; set; }
            public int Page { get; set; }
            public int PerPage { get; set; }
            public EnumOrdenacaoPedidos Ordenacao { get; set; }
            public bool OrderDesc { get; set; }
        }

        public class FiltroSuporte
        {
            public string Login { get; set; }
            public int? IdStatus { get; set; }
            public int Page { get; set; }
            public int PerPage { get; set; }
        }

        public class FiltroVendasCredenciando
        {
            public EnumTipoPagamento? tipoPagamento { get; set; }
            public DateTime? DataInicio { get; set; }
            public DateTime? DataFim { get; set; }
            public int Pagina { get; set; }
            public int QuantidadePorPagina { get; set; }
            public EnumOrdenacaoVendas? Ordenacao { get; set; }
            public bool? Asc { get; set; }
            public bool? ObterTudo { get; set; }
            public Guid? IdCredenciado { get; set; }   
            public string Documento { get; set; }
            public int? Situacao { get; set; }
            public string Nome { get;set; }
            public string Login { get; set; }
        }

        public class FiltroComprasEmLojas
        {
            public string Estabelecimento { get; set; }
            public EnumTipoPagamento? TipoPagamento { get; set; }
            public decimal? ValorInicial { get; set; }
            public decimal? ValorFinal { get; set; }
            public int Pagina { get; set; }
            public int QuantidadePorPagina { get; set; }
        }

        public class FiltroComprasEmLojasValidator : AbstractValidator<FiltroComprasEmLojas>
        {
            public FiltroComprasEmLojasValidator()
            {
                RuleFor(f => f.ValorInicial).LessThanOrEqualTo(f => f.ValorFinal).When(f => f.ValorFinal.HasValue && f.ValorInicial.HasValue).WithMessage("valor_inicial_final");
                RuleFor(f => f.TipoPagamento).Must(TipoDePagamentoValido).When(f => f.TipoPagamento.HasValue).WithMessage("pagamento_tipo_invalido");
                RuleFor(f => f.ValorFinal).GreaterThan(0).When(f => f.ValorFinal.HasValue).WithMessage("valor_final_minimo");
            }

            private bool TipoDePagamentoValido(EnumTipoPagamento? tipoPagamento)
            {
                return tipoPagamento == EnumTipoPagamento.Saldo || tipoPagamento == EnumTipoPagamento.Dinheiro;
            }
        }

        public class FiltroFaturas
        {
            public DateTime? DataInicio { get; set; }
            public DateTime? DataFim { get; set; }
            public int Pagina { get; set; }
            public EnumStatusFatura? Status { get; set; }
            public int QuantidadePorPagina { get; set; }
        }


        public class FiltroMeusCredenciamentos
        {
            public string Estabelecimento { get; set; }
            public int? IdCategoria { get; set; }
            public StatusCredenciamento? IdStatus { get; set; }
        }

        public class FiltroEcossistemas
        {
            public string Nome { get; set; }
            public bool? Ativo { get; set; }            
        }

        public class FiltroFaturasAdmin
        {
            public string LoginPatrocinador { get; set; }
            public string LoginCredenciado { get; set; }
            public int? IdCidade { get; set; }
            public DateTime? DataInicial { get; set; }
            public DateTime? DataFinal { get; set; }
            public decimal? ValorInicial { get; set; }
            public decimal? ValorFinal { get; set; }
            public EnumStatusFatura? Status { get; set; }
            public int Pagina { get; set; }
            public int QuantidadePorPagina { get; set; }
            public bool? Asc { get; set; }
            public bool? ObterTodos { get; set; }
            public EnumOrdenacaoFaturas? Ordenacao { get; set; }

        }

        public class FiltroFaturasAdminValidator : AbstractValidator<FiltroFaturasAdmin>
        {
            public FiltroFaturasAdminValidator()
            {
                RuleFor(f => f.DataInicial).LessThanOrEqualTo(f => f.DataFinal).When(f => f.DataInicial.HasValue).WithMessage("data_inicial_final");
                RuleFor(f => f.ValorInicial)
                    .LessThanOrEqualTo(f => f.ValorFinal).When(f => f.ValorInicial.HasValue).WithMessage("valor_inicial_final")
                    .GreaterThanOrEqualTo(0).When(f => f.ValorInicial.HasValue).WithMessage("valor_inicial_minimo");

            }
        }
    }
}
