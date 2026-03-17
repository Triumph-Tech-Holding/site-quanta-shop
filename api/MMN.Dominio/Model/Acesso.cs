using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMN.Dominio.Model;
public class Acesso
{
    public int Id { get; set; }
    public Guid IdUsuario { get; set; }
    public DateTime DataAcesso { get; set; }
    public string IP { get; set; }
    public string Agente { get; set; }
}

public class AccessSummary
{
    public int Daily { get; set; }
    public int Weekly { get; set; }
    public int Monthly { get; set; }
}

public class AcessoDto
{
    public int Id { get; set; }
    public Guid IdUsuario { get; set; }
    public DateTime DataAcesso { get; set; }
    public string UsuarioNome { get; set; }
    public string Login { get; set; }
    public string Email { get; set; }
    public DateTime? DataUltimoAcesso { get; set; }
    public DateTime? DataUltimaCompra { get; set; }
}
