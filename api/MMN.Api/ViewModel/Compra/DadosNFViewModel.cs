using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MMN.Api.ViewModel.Compra;

public class ItemNFViewModel
{
    [JsonPropertyName("nomeProduto")]
    public string NomeProduto { get; set; }

    [JsonPropertyName("codigoProduto")]
    public string CodigoProduto { get; set; }

    [JsonPropertyName("quantidadeProduto")]
    public double QuantidadeProduto { get; set; }

    [JsonPropertyName("valorUnitarioProduto")]
    public double ValorUnitarioProduto { get; set; }

    [JsonPropertyName("subTotal")]
    public double SubTotal { get; set; }
}

public class DadosNFViewModel
{
    [JsonPropertyName("chaveDeAcesso")]
    public string ChaveDeAcesso { get; set; }

    [JsonPropertyName("CNPJ")]
    public string CNPJ { get; set; }

    [JsonPropertyName("CPF")]
    public string CPF { get; set; }

    [JsonPropertyName("itens")]
    public List<ItemNFViewModel> Itens { get; set; }

    [JsonPropertyName("qtdTotalDeItens")]
    public double QtdTotalDeItens { get; set; }

    [JsonPropertyName("valorAPagar")]
    public double ValorAPagar { get; set; }

    [JsonPropertyName("valorTotal")]
    public double ValorTotal { get; set; }

    [JsonPropertyName("formasPagamento")]
    public List<object> FormasPagamento { get; set; }

    [JsonPropertyName("descontos")]
    public double Descontos { get; set; }
}