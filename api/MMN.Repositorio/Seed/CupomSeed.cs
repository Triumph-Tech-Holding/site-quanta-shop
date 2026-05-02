using System;
using Microsoft.EntityFrameworkCore;
using MMN.Dominio.Model;

namespace MMN.Repositorio.Seed
{
    public static class CupomSeed
    {
        public static void SeedCupons(this ModelBuilder modelBuilder)
        {
            var seedDate = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            modelBuilder.Entity<Cupom>().HasData(
                new Cupom
                {
                    IdCupom = -1,
                    Codigo = "QUANTA10",
                    Tipo = "percent",
                    Valor = 10m,
                    Descricao = "10% de desconto na primeira compra",
                    MinimoPedido = 50m,
                    MaxUsosPorCliente = 1,
                    Ativo = true,
                    DataCadastro = seedDate,
                },
                new Cupom
                {
                    IdCupom = -2,
                    Codigo = "BEMVINDO",
                    Tipo = "fixed",
                    Valor = 25m,
                    Descricao = "R$ 25 OFF para novos clientes",
                    MinimoPedido = 100m,
                    MaxUsosPorCliente = 1,
                    Ativo = true,
                    DataCadastro = seedDate,
                }
            );
        }
    }
}
