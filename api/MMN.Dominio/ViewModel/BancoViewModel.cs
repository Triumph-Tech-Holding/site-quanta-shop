namespace MMN.Dominio.ViewModel
{
    public class BancoViewModel
    {
        public int IdBanco { get; set; }
        public int? Febraban { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public int? Ordem { get; set; }
        public bool Ativo { get; set; }
    }
}
