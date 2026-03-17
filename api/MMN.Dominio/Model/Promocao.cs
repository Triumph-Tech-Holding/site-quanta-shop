using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MMN.Dominio.Model;
public class Promocao
{
    [Key]
    public long IdPromocao { get; set; }
    public int IdAnunciante { get; set; }
    public int IdReferencia { get; set; }
    public string Titulo { get; set; }
    public string Descricao { get; set; }
    public string DataInicio { get; set; }
    public DateTime? DataFim { get; set; }
    public DateTime? URL { get; set; }
    public string Tipo { get; set; }
    public string CupomDesconto { get; set; }
    public bool Ativo { get; set; }
    public DateTime DataCriacao { get; set; }

    [ForeignKey("IdAnunciante")]
    public virtual Anunciante Anunciante { get; set; }
}
