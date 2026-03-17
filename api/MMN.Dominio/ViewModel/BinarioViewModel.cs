using System;
using System.Collections.Generic;
using System.Text;

namespace MMN.Dominio.ViewModel
{
    public class BinarioViewModel
    {
        public int IdMatriz { get; set; }
        public int? IdMatrizPai { get; set; }
        public Guid IdUsuario { get; set; }
        public Guid? IdUsuarioPai { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
        public string Nome { get; set; }
        public DateTime DataCadastro { get; set; }
        public int IdGraduacao { get; set; }
        public string Graduacao { get; set; }
        public string UrlImagem { get; set; }
        public int Nivel { get; set; }
        public short Posicao { get; set; }
    }
}
