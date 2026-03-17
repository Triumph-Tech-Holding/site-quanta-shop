using System;
using System.Collections.Generic;

namespace MMN.Dominio.Model
{
    public class UsuarioBanco
    {
        public int IdUsuarioBanco { get; set; }
        public Guid? IdUsuario { get; set; }
        public int? IdBanco { get; set; }
        public string Agencia { get; set; }
        public string DigitoAgencia { get; set; }
        public string Conta { get; set; }
        public string DigitoConta { get; set; }
        public string Cpfcnpj { get; set; }
        public string NomeConta { get; set; }
        public bool Ativo { get; set; }
        public int IdTipo { get; set; }

        public virtual Banco Banco { get; set; }
        public virtual Usuario Usuario { get; set; }
        public virtual Tipo Tipo { get; set; }
        public ICollection<Saque> Saque { get; set; }
    }
}
