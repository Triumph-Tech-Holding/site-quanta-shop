using System;
using System.ComponentModel.DataAnnotations;

namespace MMN.Dominio.Model
{
    public class Objetivo
    {
        [Key]
        public int IdObjetivo { get; set; }
        public string Descricao { get; set; }
        public string Grupo { get; set; }
        public decimal Quantidade { get; set; }
        public DateTime? PrazoDe { get; set; }
        public DateTime? PrazoAte { get; set; }
        public int Nivel { get; set; }
        public int Pontos { get; set; }
        public bool Ativo { get; set; }
        public int Ordem { get; set; }
        
    }
}
