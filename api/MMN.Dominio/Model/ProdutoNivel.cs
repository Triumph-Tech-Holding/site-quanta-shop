namespace MMN.Dominio.Model
{
    public class ProdutoNivel
    {
        public int IdProdutoNivel { get; set; }
        public int Nivel { get; set; }
        public int IdProduto { get; set; }
        public decimal PorcentagemCashback { get; set; }
        public decimal? PorcentagemAdesao { get; set; }

        public virtual Produto Produto { get; set; }
    }
}
