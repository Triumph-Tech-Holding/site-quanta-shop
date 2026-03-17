using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MMN.Dominio.Model;

namespace MMN.Repositorio.Mapping
{
    public class CuponCashbackPedidoMapping : IEntityTypeConfiguration<CuponCashbackPedido>
    {
        public void Configure(EntityTypeBuilder<CuponCashbackPedido> builder)
        {
            builder.HasKey(e => e.IdCuponCashback);
            builder.Property(e => e.IdPedido).IsRequired();
            builder.Property(e => e.IdCuponCashback).IsRequired();

            builder.HasOne(d => d.CuponCashback)
                .WithOne(d => d.CuponCashbackPedido)
                .HasForeignKey<CuponCashbackPedido>(d => d.IdCuponCashback)
                .HasPrincipalKey<CupomCashback>(p => p.IdCuponCashback)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(d => d.Pedido)
                .WithOne(o => o.CuponCashbackPedido)
                .HasForeignKey<CuponCashbackPedido>(d => d.IdPedido)
                .HasPrincipalKey<Pedido>(p => p.IdPedido)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
