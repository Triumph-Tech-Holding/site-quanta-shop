using System;

namespace MMN.Dominio.Model
{
    public class UsuarioEndereco
    {
        public int IdEndereco { get; set; }
        public int IdCidade { get; set; }
        public Guid IdUsuario { get; set; }
        public string Rua { get; set; }
        public string Numero { get; set; }
        public string Bairro { get; set; }
        public string Cep { get; set; }
        public string Complemento { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }

        public virtual Usuario Usuario { get; set; }
        public virtual Cidade Cidade { get; set; }
    }
}
