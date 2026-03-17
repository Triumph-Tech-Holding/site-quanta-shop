namespace MMN.Dominio.ViewModel
{
    public class PontuacaoPorValorViewModel
    {
        public int PontosUsuario { get; set; }
        public int PontosRedeElegivel { get; set; }
        public int TotalPontosRede { get; set; }
        public int TotalPontosElegivel { get => PontosUsuario + PontosRedeElegivel; }
        public int TotalPontosUsuario { get => PontosUsuario + TotalPontosRede; }

    }
}
