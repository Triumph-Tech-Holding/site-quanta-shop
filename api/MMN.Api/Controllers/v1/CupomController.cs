using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Hosting;
using MMN.Negocio.Services;
using MMN.Repositorio.Contexto;

namespace MMN.Api.Controllers.v1
{
    [Route("api/cupom")]
    [ApiController]
    public class CupomController : LoggedControllerBase
    {
        private readonly DatabaseContext _context;
        private readonly IWebHostEnvironment _env;

        public CupomController(DatabaseContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public class ValidarCupomRequest
        {
            public string Codigo { get; set; }
            public decimal? ValorPedido { get; set; }
        }

        [HttpPost]
        [Route("validar")]
        [AllowAnonymous]
        public IActionResult Validar([FromBody] ValidarCupomRequest req)
        {
            if (req == null || string.IsNullOrWhiteSpace(req.Codigo))
            {
                return Ok(new { valido = false, mensagem = "Informe um codigo de cupom." });
            }

            var codigo = req.Codigo.Trim().ToUpperInvariant();

            try
            {
                var cupom = _context.Set<Dominio.Model.Cupom>().AsQueryable()
                    .FirstOrDefault(c => c.Codigo.ToUpper() == codigo);

                var validacao = CupomValidacaoHelper.ValidarRegrasBasicas(cupom, req.ValorPedido, DateTime.UtcNow);
                if (!validacao.Valido)
                {
                    return Ok(new { valido = false, mensagem = validacao.Mensagem });
                }
                if (cupom.MaxUsosTotal.HasValue)
                {
                    var totalUsos = _context.Set<Dominio.Model.CupomUso>().Count(u => u.IdCupom == cupom.IdCupom);
                    if (totalUsos >= cupom.MaxUsosTotal.Value)
                    {
                        return Ok(new { valido = false, mensagem = "Cupom esgotado." });
                    }
                }
                if (cupom.MaxUsosPorCliente.HasValue && IdUsuarioLogado != Guid.Empty)
                {
                    var usosCliente = _context.Set<Dominio.Model.CupomUso>().Count(u => u.IdCupom == cupom.IdCupom && u.IdUsuario == IdUsuarioLogado);
                    if (usosCliente >= cupom.MaxUsosPorCliente.Value)
                    {
                        return Ok(new { valido = false, mensagem = "Voce ja usou este cupom." });
                    }
                }

                return Ok(new
                {
                    valido = true,
                    codigo = cupom.Codigo,
                    tipo = cupom.Tipo,
                    valor = cupom.Valor,
                    descricao = cupom.Descricao,
                    minimoPedido = cupom.MinimoPedido,
                });
            }
            catch (SqlException sqlEx) when (_env.IsDevelopment() && (sqlEx.Number == 208 || sqlEx.Number == 4060))
            {
                // Apenas em DEV e somente para "Invalid object name" (tabela ausente pre-migration).
                return ValidarFallback(codigo, req.ValorPedido);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "erro_validar_cupom", detail = ex.Message });
            }
        }

        private static readonly (string Codigo, string Tipo, decimal Valor, string Descricao, decimal MinimoPedido)[] _fallbackCupons =
        {
            ("QUANTA10", "percent", 10m, "10% de desconto", 50m),
            ("BEMVINDO", "fixed",   25m, "R$ 25 OFF",       100m),
        };

        private static IActionResult ValidarFallback(string codigo, decimal? valorPedido)
        {
            foreach (var c in _fallbackCupons)
            {
                if (c.Codigo != codigo) continue;

                if (valorPedido.HasValue && valorPedido.Value < c.MinimoPedido)
                    return new OkObjectResult(new { valido = false, mensagem = $"Pedido minimo de R$ {c.MinimoPedido:F2} para usar este cupom." });

                return new OkObjectResult(new { valido = true, codigo = c.Codigo, tipo = c.Tipo, valor = c.Valor, descricao = c.Descricao, minimoPedido = c.MinimoPedido });
            }
            return new OkObjectResult(new { valido = false, mensagem = "Cupom invalido ou expirado." });
        }
    }
}
