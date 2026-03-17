using System;

namespace MMN.Dominio.ViewModel
{
    public class RankUsuarioViewModel
    {
        public Guid IdUsuario { get; set; }
        public string Login { get; set; }
        public int Pontuacao { get; set; }
        public string Graduacao { get; set; }
        public decimal Consumo { get; set; }
    }
}
