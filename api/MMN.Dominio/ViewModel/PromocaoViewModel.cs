using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMN.Dominio.ViewModel;
public class PromocaoViewModel
{
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
}
