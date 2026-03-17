using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MMN.Dominio.Model
{
    public class QuantaAmizade
    {
        [Key]
        public int IdQuantaAmizade { get; set; }
        public Guid IdUsuarioPai { get ; set; }
        public Guid IdUsuarioFilho { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime DataFim { get; set; }
        public bool ObjetivoAtingido { get; set; }
        
        [ForeignKey("IdUsuarioPai")]
        public virtual Usuario UsuarioPai { get; set; }

        [ForeignKey("IdUsuarioFilho")]
        public virtual Usuario UsuarioFilho { get; set; }
    }
}
