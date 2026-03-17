namespace MMN.Dominio.Model
{
    public class PercentualBonusResidualCredenciamento
    {
        public int IdPercentualBonusResidualCredenciamento { get; set; }
        public int IdProduto { get; set; }
        public int Nivel { get; set; }
        public decimal Percentual { get; set; }

        public Produto Produto { get; set; }
    }
}
