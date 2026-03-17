namespace MMN.Dominio.Model
{
    public class PercentualBonusCredenciamento
    {
        public int IdPercentualBonusCredenciamento { get; set; }
        public int IdProduto { get; set; }
        public decimal Percentual { get; set; }

        public Produto Produto { get; set; }
    }
}
