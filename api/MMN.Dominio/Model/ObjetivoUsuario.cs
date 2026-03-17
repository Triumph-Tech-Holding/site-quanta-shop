using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MMN.Dominio.Model
{
    public class ObjetivoUsuario
    {
        [Key]
        public int IdObjetivoUsuario { get; set; }
        public int IdObjetivo { get; set; }
        public Guid IdUsuario { get; set; }
        public Guid? IdUsuarioFilho { get; set; }
        public int? IdPedido { get; set; }
        public DateTime DataRegistro { get; set; }

        [ForeignKey("IdUsuario")]
        public virtual Usuario UsuarioPai { get; set; }

        [ForeignKey("IdUsuarioFilho")]
        public virtual Usuario UsuarioFilho { get; set; }
    }

}
